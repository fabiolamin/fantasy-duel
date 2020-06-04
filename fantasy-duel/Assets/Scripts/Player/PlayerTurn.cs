using UnityEngine;
using Photon.Pun;

public class PlayerTurn : MonoBehaviourPunCallbacks
{
    private PlayerManager playerManager;
    private float durationAux;
    private int round;
    private bool IsReadyToCountdown = false;

    [SerializeField] private float duration = 30f;
    [SerializeField] private float timeToShowDuration = 15f;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();

        durationAux = duration;
        round = 0;

        if (!PhotonNetwork.IsMasterClient)
        {
            EndTurn();
        }
    }

    private void Update()
    {
        if (IsReadyToCountdown)
        {
            CheckCountdown();
        }
    }

    public void EndTurn()
    {
        if (playerManager.PhotonView.IsMine)
        {
            playerManager.PlayerHUD.ActiveTurnDurationText(false);
            duration = durationAux;
            IsReadyToCountdown = false;
            playerManager.PlayerHUD.ActiveButtons(false);
            playerManager.PlayerHand.Lock(true);
            UpdateOpponentTurnProperty();
        }
    }

    private void CheckCountdown()
    {
        duration -= Time.deltaTime;

        if (duration <= timeToShowDuration)
        {
            playerManager.PlayerHUD.SetTurnDurationText((int)duration);

            if (duration <= 0)
            {
                EndTurn();
            }
        }
    }

    private void UpdateOpponentTurnProperty()
    {
        ExitGames.Client.Photon.Hashtable property = new ExitGames.Client.Photon.Hashtable();
        property.Add("IsReadyToPlayTurn", true);
        PhotonNetwork.PlayerListOthers[0].SetCustomProperties(property);
    }

    private void UpdateRoomTurnProperty()
    {
        ExitGames.Client.Photon.Hashtable roomProperty = PhotonNetwork.CurrentRoom.CustomProperties;

        if (roomProperty.ContainsKey("TurnNumber"))
        {
            round = (int)roomProperty["TurnNumber"];
            roomProperty.Remove("TurnNumber");
        }
        round++;
        roomProperty.Add("TurnNumber", round);
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperty);
    }

    public void StartTurn()
    {
        AudioManager.Instance.Play(Audio.SoundEffects, Clip.Turn, false);
        playerManager.PlayerHUD.ActiveMatchMessage(true);
        playerManager.PlayerHUD.SetMatchMessage("Your turn!");
        playerManager.PlayerHUD.ActiveButtons(true);
        playerManager.PlayerHand.Lock(false);
        playerManager.PlayerAction.CanPlayerDoAnAction = true;
        IsReadyToCountdown = true;
        UpdateRoomTurnProperty();
    }
}
