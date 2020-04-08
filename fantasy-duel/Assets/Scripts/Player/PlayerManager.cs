using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour
{
    private PhotonView photonView;

    [SerializeField] private int lifePoints;
    [SerializeField] private int coins;
    [SerializeField] private GameObject cameraPrefab;

    public int LifePoints { get { return lifePoints; } private set { lifePoints = value; } }
    public int Coins { get { return coins; } private set { coins = value; } }
    public int WonRounds { get; set; }

    private void Awake()
    {
        WonRounds = 0;

        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            SetCamera();
        }
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
}
