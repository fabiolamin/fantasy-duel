using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    public PhotonView PhotonView { get; private set; }
    public PlayerInfo PlayerInfo { get; private set; }
    public PlayerHUD PlayerHUD { get; private set; }
    public PlayerHand PlayerHand { get; private set; }
    public PlayerCardMovement PlayerCardMovement { get; private set; }
    public PlayerBoardArea PlayerBoardArea { get; private set; }
    public PlayerAction PlayerAction { get; private set; }

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        PlayerInfo = GetComponent<PlayerInfo>();
        PlayerHUD = GetComponent<PlayerHUD>();
        PlayerHand = GetComponent<PlayerHand>();
        PlayerCardMovement = GetComponent<PlayerCardMovement>();
        PlayerBoardArea = GetComponent<PlayerBoardArea>();
        PlayerAction = GetComponent<PlayerAction>();
    }
}
