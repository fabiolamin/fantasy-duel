using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHUD : MonoBehaviour
{
    private PhotonView photonView;
    private PlayerInfo playerInfo;

    [SerializeField] private Text lifePoints;
    [SerializeField] private Text coins;
    [SerializeField] private Text rounds;
    [SerializeField] private Text nickname;
    [SerializeField] private GameObject buttons;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        playerInfo = GetComponent<PlayerInfo>();

        SetHUD();

        if (photonView.IsMine)
        {
            SetButtons();
            photonView.RPC("RotateHUDToOpponentRPC", RpcTarget.OthersBuffered);
        }
    }

    private void SetButtons()
    {
        buttons.SetActive(true);
    }

    public void SetHUD()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("SetHUDRPC", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void SetHUDRPC()
    {
        nickname.text = photonView.Owner.NickName;
        lifePoints.text = playerInfo.LifePoints.ToString();
        coins.text = playerInfo.Coins.ToString();
        rounds.text = playerInfo.WonRounds.ToString();
    }

    [PunRPC]
    private void RotateHUDToOpponentRPC()
    {
        nickname.transform.Rotate(0, 0, 180);
        lifePoints.transform.Rotate(0, 0, 180);
        coins.transform.Rotate(0, 0, 180);
        rounds.transform.Rotate(0, 0, 180);
    }
}
