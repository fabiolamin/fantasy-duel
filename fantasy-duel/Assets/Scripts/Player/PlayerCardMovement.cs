using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCardMovement : MonoBehaviour
{
    private PlayerManager playerManager;

    [Header("When mouse is over a card")]
    [SerializeField] private float scale = 1f;
    [SerializeField] private float height = 1.4f;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    public void RaiseCard(Card card)
    {
        if (playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("RaiseCardRPC", RpcTarget.AllBuffered, playerManager.PlayerHand.GetIndex(card));
        }
    }

    [PunRPC]
    private void RaiseCardRPC(int index)
    {
        playerManager.PlayerHand.Cards[index].transform.position += Vector3.up * height;
    }

    public void IncreaseCardScale(Card card)
    {
        if (playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("IncreaseCardScaleRPC", RpcTarget.AllBuffered, playerManager.PlayerHand.GetIndex(card));
        }
    }

    [PunRPC]
    private void IncreaseCardScaleRPC(int index)
    {
        playerManager.PlayerHand.Cards[index].transform.localScale += new Vector3(scale, scale, 0);
    }

    public void SetInitialTransform(Card card)
    {
        if (playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("SetInitialTransformRPC", RpcTarget.AllBuffered, playerManager.PlayerHand.GetIndex(card));
        }
    }

    [PunRPC]
    private void SetInitialTransformRPC(int index)
    {
        Vector3 scale = playerManager.PlayerHand.CardsTransform[index].transform.localScale;
        playerManager.PlayerHand.Cards[index].transform.position = playerManager.PlayerHand.CardsTransform[index].transform.position;
        playerManager.PlayerHand.Cards[index].transform.localScale = new Vector3(scale.x, scale.z, 0.00001f);
    }
}
