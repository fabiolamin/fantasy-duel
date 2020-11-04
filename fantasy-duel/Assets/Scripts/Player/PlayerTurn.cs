using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerTurn : MonoBehaviourPunCallbacks
{
    private PlayerManager playerManager;
    private float durationAux;
    private int round;
    private bool IsReadyToCountdown = false;

    [SerializeField] private float duration = 30f;
    [SerializeField] private float timeToShowDuration = 15f;

    public bool IsMyTurn { get; private set; } = false;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();

        durationAux = duration;
        round = 0;
    }

    private void Start()
    {
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
            playerManager.PlayerParticlesControl.StopAllCardsParticles();
            playerManager.PlayerHUD.ActiveTurnDurationText(false);
            playerManager.PlayerHUD.ActiveButtons(false);
            playerManager.PlayerHand.Lock(true);
            playerManager.PlayerAction.HideAvailableOpponentCardsToAttack();
            duration = durationAux;
            IsReadyToCountdown = false;
            ActiveTurn(false);
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
        ActiveTurn(true);
        AudioManager.Instance.Play(Audio.SoundEffects, Clip.Turn, false);
        playerManager.PlayerParticlesControl.StopAllCardsParticles();
        playerManager.PlayerHUD.ActiveNotification(true);
        string turnMessage = playerManager.PlayerHUD.GetNotificationTranslation(LocalizationKeyNames.PlayerTurn);
        playerManager.PlayerHUD.SetNotification(turnMessage);
        playerManager.PlayerHUD.ActiveButtons(true);
        playerManager.PlayerInfo.CanSacrifice = true;
        playerManager.PlayerHand.Lock(false);
        playerManager.PlayerAction.ShowAvailableOpponentCardsToAttack();
        playerManager.PlayerBoardArea.ShowAvailableCards();
        playerManager.PlayerParticlesControl.StopCharacterParticles();
        IsReadyToCountdown = true;
        UpdateRoomTurnProperty();
    }

    private void ActiveTurn(bool isActivated)
    {
        if (playerManager.PhotonView.IsMine)
            playerManager.PhotonView.RPC("ActiveTurnRPC", RpcTarget.AllBuffered, isActivated);
    }

    [PunRPC]
    private void ActiveTurnRPC(bool isActivated)
    {
        IsMyTurn = isActivated;
    }
}
