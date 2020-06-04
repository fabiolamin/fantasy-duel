using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Networking : MonoBehaviourPunCallbacks
{
    private bool isReadyToLoad = false;
    [SerializeField] private PlayerSettings playerSettings;
    [SerializeField] private float timeToLoad = 4f;
    private void Awake()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void CheckLoadCountdown()
    {
        timeToLoad -= Time.deltaTime;
        AudioManager.Instance.UpdateVolume(Audio.Soundtrack, -Time.deltaTime);

        if (timeToLoad <= 0)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

    public void Connect()
    {
        UIManager.Instance.ShowMatchmakingPanel();
        UIManager.Instance.ConnectionStatus.text = "Connecting to the server...";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.NickName = PlayerPrefs.GetString("Nickname");
        UIManager.Instance.ConnectionStatus.text = "Finding an oponnent...";
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
        playerSettings.SetDeckAsProperty();

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Player player = GetMasterClient();
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add("IsReadyToLoad", true);
            player.SetCustomProperties(properties);
        }
    }

    private Player GetMasterClient()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.IsMasterClient)
            {
                return player;
            }
        }

        return null;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer.IsMasterClient)
        {
            LoadRoom(changedProps);
        }
    }

    private void LoadRoom(Hashtable changedProps)
    {
        if (changedProps.ContainsKey("IsReadyToLoad"))
        {
            PhotonNetwork.LoadLevel(1);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(cause);
    }
}
