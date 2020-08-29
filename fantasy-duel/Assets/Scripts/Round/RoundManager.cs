using UnityEngine;
using Photon.Pun;

public class RoundManager : MonoBehaviourPunCallbacks
{
    private static RoundManager instance;
    private bool isMatchOver = false;
    [SerializeField] private int roundsToFinish = 2;

    public int RoundsToFinish { get { return roundsToFinish; } private set { roundsToFinish = value; } }
   
    public static RoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("RoundManager is NULL.");
            }

            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void SetRound()
    {
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            int life = playerManager.PlayerInfo.LifePoints;
            string message;

            if (life != 0)
            {
                playerManager.PlayerInfo.AddRound();
                message = playerManager.PlayerHUD.GetNotificationTranslation(LocalizationKeyNames.PlayerRoundWon);
            }
            else
            {
                message = playerManager.PlayerHUD.GetNotificationTranslation(LocalizationKeyNames.PlayerRoundLost);
            }

            playerManager.PlayerHUD.SetNotification(message);
            playerManager.PlayerHUD.ActiveNotification(true);
            playerManager.PlayerInfo.SetAttributes();
            playerManager.PlayerHUD.SetHUD();
            playerManager.PlayerBoardArea.ClearBoard();
        }
    }

    public void CheckRounds()
    {
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerManager playerManager = player.GetComponent<PlayerManager>();

            if (playerManager.PlayerInfo.WonRounds == roundsToFinish)
            {
                MatchManager.Instance.EndMatch();
            }
        }
    }
}
