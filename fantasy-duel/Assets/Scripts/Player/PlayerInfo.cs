using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerInfo : MonoBehaviour
{
    private PhotonView photonView;
    private PlayerHUD playerHUD;

    [SerializeField] private int maxLifePoints;
    [SerializeField] private int maxCoins;
    [SerializeField] private GameObject cameraPrefab;

    public int LifePoints { get; private set; }
    public int Coins { get; private set; }
    public int WonRounds { get; set; }

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        playerHUD = GetComponent<PlayerHUD>();

        SetAttributes();

        if (photonView.IsMine)
        {
            SetCamera();
        }
    }

    private void SetAttributes()
    {
        WonRounds = 0;
        LifePoints = maxLifePoints;
        Coins = maxCoins;
    }

    private void SetCamera()
    {
        int y = 180;
        if (PhotonNetwork.IsMasterClient)
        {
            y = 0;
        }
        Instantiate(cameraPrefab, new Vector3(0.2f, 32, 0.3f), Quaternion.Euler(90, y, 0), transform);
    }

    public void TransferSomeLifePointsToCoins()
    {
        photonView.RPC("UpdateLifePointsRPC", RpcTarget.AllBuffered, -3);
        photonView.RPC("UpdateCoinsRPC", RpcTarget.AllBuffered, 5);
        playerHUD.SetHUD();
    }

    [PunRPC]
    private void UpdateLifePointsRPC(int amount)
    {
        LifePoints = Mathf.Clamp(LifePoints + amount, 0, maxLifePoints);
    }

    [PunRPC]
    private void UpdateCoinsRPC(int amount)
    {
        Coins = Mathf.Clamp(Coins + amount, 0, maxCoins);
    }
}
