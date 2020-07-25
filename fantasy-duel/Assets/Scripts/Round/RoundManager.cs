using UnityEngine;
using Photon.Pun;

public class RoundManager : MonoBehaviourPunCallbacks
{
    private static RoundManager instance;
    [SerializeField] private int roundsToFinish = 2;

    public int RoundsToFinish { get { return roundsToFinish; } private set { roundsToFinish = value; } }
    public bool isThereAPlayerLeavingMatch { get; set; } = false;
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
                message = "You won a round!";
            }
            else
            {
                message = "You lost a round!";
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
                EndMatch();
            }
        }
    }

    public void EndMatch()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            ExitGames.Client.Photon.Hashtable property = new ExitGames.Client.Photon.Hashtable();
            property.Add("IsMatchOver", true);
            player.SetCustomProperties(property);
        }
    }
}
