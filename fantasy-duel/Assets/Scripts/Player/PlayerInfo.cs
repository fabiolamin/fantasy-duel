using UnityEngine;
using Photon.Pun;
public class PlayerInfo : MonoBehaviourPunCallbacks, IDamageable
{
    private PlayerManager playerManager;

    [SerializeField] private int maxLifePoints;
    [SerializeField] private int maxCoins;
    [SerializeField] private GameObject cameraPrefab;

    public int LifePoints { get; private set; }
    public int Coins { get; private set; }
    public int WonRounds { get; set; }

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();

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
        Instantiate(cameraPrefab, new Vector3(0.2f, 32, 0.3f), Quaternion.Euler(90, Utility.GetYRotation(), 0), transform);
    }

    public void TransferSomeLifePointsToCoins()
    {
        Damage(3);
        UpdateCoins(5);
    }

    [PunRPC]
    private void DamageRPC(int amount)
    {
        LifePoints = Mathf.Clamp(LifePoints - amount, 0, maxLifePoints);
        if (LifePoints <= 0)
        {
            Debug.Log("Round lost!");
        }
    }

    public void UpdateCoins(int amount)
    {
        if (photonView.IsMine)
        {
            playerManager.PhotonView.RPC("UpdateCoinsRPC", RpcTarget.AllBuffered, amount);
            playerManager.PlayerHUD.SetHUD();
        }
    }

    [PunRPC]
    private void UpdateCoinsRPC(int amount)
    {
        Coins = Mathf.Clamp(Coins + amount, 0, maxCoins);
    }

    public void Damage(int amount)
    {
        if (playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("DamageRPC", RpcTarget.AllBuffered, amount);
            playerManager.PlayerHUD.SetHUD();
        }
    }
}
