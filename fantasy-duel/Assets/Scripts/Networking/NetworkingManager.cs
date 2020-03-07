using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class NetworkingManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private UIManager uiManager;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect()
    {
        uiManager.EnableMatchmakingPanel();
        uiManager.ConnectionStatus.text = "Connecting to the server...";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.NickName = PlayerPrefs.GetString("Nickname");
        uiManager.ConnectionStatus.text = "Finding an oponnent...";
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        int random = Random.Range(100, 999);
        string name = "room" + random;
        RoomOptions roomOptions = new RoomOptions()
        {
            MaxPlayers = 2
        };

        PhotonNetwork.JoinOrCreateRoom(name, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            foreach(Player player in PhotonNetwork.PlayerList)
            {
                if(player.IsMasterClient)
                {
                    ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
                    customProperties.Add("IsReadyToLoad", true);
                    player.SetCustomProperties(customProperties);
                }
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(targetPlayer.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }
}
