using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;

public class PlayerHUD : MonoBehaviour
{
    private PlayerManager playerManager;
    private float turnMessageDurationAux;
    private bool isReadyToHideTurnMessage = false;

    [SerializeField] private float turnMessageDuration = 2f;
    [SerializeField] private Text lifePoints;
    [SerializeField] private Text coins;
    [SerializeField] private Text rounds;
    [SerializeField] private Text nickname;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] playedCardImages;
    [SerializeField] private Text turnMessage;
    [SerializeField] private Text duration;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();

        SetHUD();

        if (playerManager.PhotonView.IsMine)
        {
            buttons.ToList().ForEach(button => button.SetActive(true));
            playedCardImages.ToList().ForEach(image => image.SetActive(true));
            playerManager.PhotonView.RPC("RotateHUDToOpponentRPC", RpcTarget.OthersBuffered);
        }

        turnMessageDurationAux = turnMessageDuration;
    }

    private void Update()
    {
        if (isReadyToHideTurnMessage)
        {
            StartTurnMessageCountdown();
            VerifyTurnMessageCountdown();
        }
    }

    private void StartTurnMessageCountdown()
    {
        turnMessageDuration -= Time.deltaTime;
    }

    private void VerifyTurnMessageCountdown()
    {
        if (turnMessageDuration <= 0)
        {
            HideTurnMessage();
        }
    }

    private void HideTurnMessage()
    {
        ActiveTurnMessage(false);
        turnMessageDuration = turnMessageDurationAux;
    }

    public void SetHUD()
    {
        if (playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("SetHUDRPC", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void SetHUDRPC()
    {
        nickname.text = playerManager.PhotonView.Owner.NickName;
        lifePoints.text = playerManager.PlayerInfo.LifePoints.ToString();
        coins.text = playerManager.PlayerInfo.Coins.ToString();
        rounds.text = playerManager.PlayerInfo.WonRounds.ToString();
    }

    [PunRPC]
    private void RotateHUDToOpponentRPC()
    {
        nickname.transform.Rotate(0, 0, 180);
        lifePoints.transform.Rotate(0, 0, 180);
        coins.transform.Rotate(0, 0, 180);
        rounds.transform.Rotate(0, 0, 180);
    }

    public void ActiveTurnMessage(bool isActivated)
    {
        turnMessage.gameObject.SetActive(isActivated);
        isReadyToHideTurnMessage = isActivated;
    }

    public void ActiveButtons(bool isActivated)
    {
        buttons.ToList().ForEach(button => button.GetComponent<Button>().enabled = isActivated);
    }

    public void SetTurnDurationText(int time)
    {
        ActiveTurnDurationText(true);
        duration.text = "00 : " + time.ToString("00");
    }

    public void ActiveTurnDurationText(bool isActivated)
    {
        duration.gameObject.SetActive(isActivated);
    }
}
