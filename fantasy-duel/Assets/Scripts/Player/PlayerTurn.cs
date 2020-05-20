using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerTurn : MonoBehaviourPunCallbacks
{
    private PlayerManager playerManager;
    private float durationAux;
    private int round;
    private bool IsReadyToCountdown = false;
    private ExitGames.Client.Photon.Hashtable property = new ExitGames.Client.Photon.Hashtable();
    private ExitGames.Client.Photon.Hashtable roomProperty = new ExitGames.Client.Photon.Hashtable();

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
            StartCountdown();
            VerifyCountdown();
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
            UpdateTurnProperty();
            PhotonNetwork.PlayerListOthers[0].SetCustomProperties(property);
        }
    }

    private void StartCountdown()
    {
        duration -= Time.deltaTime;
    }

    private void VerifyCountdown()
    {
        if (duration <= timeToShowDuration)
        {
            playerManager.PlayerHUD.SetTurnDurationText((int)duration);

            if (duration <= 0)
            {
                EndTurn();
            }
        }
    }

    private void UpdateTurnProperty()
    {
        if (property.ContainsKey("IsReadyToPlayTurn"))
        {
            property.Remove("IsReadyToPlayTurn");
        }
        property.Add("IsReadyToPlayTurn", true);
    }

    private void UpdateRoomTurnProperty()
    {
        if (roomProperty.ContainsKey("TurnNumber"))
        {
            round = (int)roomProperty["TurnNumber"];
            roomProperty.Remove("TurnNumber");
        }
        round++;
        roomProperty.Add("TurnNumber", round);
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperty);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer == photonView.Owner && photonView.IsMine && changedProps.ContainsKey("IsReadyToPlayTurn"))
        {
            playerManager.PlayerHUD.ActiveTurnMessage(true);
            playerManager.PlayerHUD.ActiveButtons(true);
            playerManager.PlayerHand.Lock(false);
            playerManager.PlayerAction.CanPlayerDoAnAction = true;
            IsReadyToCountdown = true;
            UpdateRoomTurnProperty();
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        roomProperty = propertiesThatChanged;
    }
}
