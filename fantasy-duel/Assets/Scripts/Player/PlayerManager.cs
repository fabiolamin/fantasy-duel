using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float timeToDisconnect = 3f;
    public PhotonView PhotonView { get; private set; }
    public PlayerInfo PlayerInfo { get; private set; }
    public PlayerHUD PlayerHUD { get; private set; }
    public PlayerHand PlayerHand { get; private set; }
    public PlayerCardMovement PlayerCardMovement { get; private set; }
    public PlayerBoardArea PlayerBoardArea { get; private set; }
    public PlayerAction PlayerAction { get; private set; }
    public PlayerTurn PlayerTurn { get; private set; }
    public bool IsReadyToDisconnect { get; set; } = false;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        PlayerInfo = GetComponent<PlayerInfo>();
        PlayerHUD = GetComponent<PlayerHUD>();
        PlayerHand = GetComponent<PlayerHand>();
        PlayerCardMovement = GetComponent<PlayerCardMovement>();
        PlayerBoardArea = GetComponent<PlayerBoardArea>();
        PlayerAction = GetComponent<PlayerAction>();
        PlayerTurn = GetComponent<PlayerTurn>();
    }

    private void Update()
    {
        if (IsReadyToDisconnect)
        {
            CheckDisconnectCountdown();
        }
    }

    private void CheckDisconnectCountdown()
    {
        timeToDisconnect -= Time.deltaTime;

        if (timeToDisconnect <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
            if (PhotonView.Owner.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(0);
            }
        }
    }

    public void PlaySoundEffect(Clip clip)
    {
        PhotonView.RPC("PlaySoundEffectRPC", RpcTarget.AllBuffered, (int)clip);
    }

    [PunRPC]
    private void PlaySoundEffectRPC(int index)
    {
        AudioManager.Instance.Play(Audio.SoundEffects, (Clip)index, false);
    }
}
