using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviourPunCallbacks
{
    private static MatchManager instance;
    private bool isMatchOver = false;

    public static MatchManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("MatchManager is NULL.");
            }

            return instance;
        }
    }

    public bool isThereAPlayerLeavingMatch { get; set; } = false;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (PhotonNetwork.PlayerList.Length == 1 && !isMatchOver)
        {
            EndMatch();
            isThereAPlayerLeavingMatch = true;
        }
    }

    public void EndMatch()
    {
        isMatchOver = true;

        foreach (var player in PhotonNetwork.PlayerList)
        {
            ExitGames.Client.Photon.Hashtable property = new ExitGames.Client.Photon.Hashtable();
            property.Add("IsMatchOver", true);
            player.SetCustomProperties(property);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene(0);
    }
}
