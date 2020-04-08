using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHUD : MonoBehaviour
{
    private PhotonView photonView;
    private PlayerManager playerManager;

    [SerializeField] private Text lifePoints;
    [SerializeField] private Text coins;
    [SerializeField] private Text rounds;
    [SerializeField] private Text nickname;
    [SerializeField] private GameObject buttons;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        playerManager = GetComponent<PlayerManager>();

        if (photonView.IsMine)
        {
            SetButtons();
            Set();
            photonView.RPC("RotateHUDToOpponent", RpcTarget.OthersBuffered);
        }
    }

    private void SetButtons()
    {
        buttons.SetActive(true);
    }

    public void Set()
    {
        photonView.RPC("SetHUD", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void SetHUD()
    {
        nickname.text = photonView.Owner.NickName;
        lifePoints.text = playerManager.LifePoints.ToString();
        coins.text = playerManager.Coins.ToString();
        rounds.text = playerManager.WonRounds.ToString();
    }

    [PunRPC]
    private void RotateHUDToOpponent()
    {
        nickname.transform.Rotate(0, 0, 180);
        lifePoints.transform.Rotate(0, 0, 180);
        coins.transform.Rotate(0, 0, 180);
        rounds.transform.Rotate(0, 0, 180);
    }
}
