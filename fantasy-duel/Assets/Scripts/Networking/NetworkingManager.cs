using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkingManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private UIManager uiManager;
    public void Connect()
    {
        uiManager.EnableMatchmakingPanel();
        uiManager.ConnectionStatus.text = "Connecting to server...";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.NickName = PlayerPrefs.GetString("Nickname");
    }
}
