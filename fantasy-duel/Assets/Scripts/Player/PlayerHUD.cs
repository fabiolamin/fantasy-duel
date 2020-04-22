using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class PlayerHUD : MonoBehaviour
{
    private PhotonView photonView;
    private PlayerInfo playerInfo;
    private float turnMessageDurationAux;
    private bool isReadyToHideTurnMessage = false;

    [SerializeField] private float turnMessageDuration = 2f;
    [SerializeField] private Text lifePoints;
    [SerializeField] private Text coins;
    [SerializeField] private Text rounds;
    [SerializeField] private Text nickname;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private Text turnMessage;
    [SerializeField] private Text duration;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        playerInfo = GetComponent<PlayerInfo>();

        SetHUD();

        if (photonView.IsMine)
        {
            buttons.ToList().ForEach(button => button.SetActive(true));
            photonView.RPC("RotateHUDToOpponentRPC", RpcTarget.OthersBuffered);
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
        UpdateTurnMessage(false);
        turnMessageDuration = turnMessageDurationAux;
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

    public void UpdateTurnMessage(bool status)
    {
        turnMessage.gameObject.SetActive(status);
        isReadyToHideTurnMessage = status;
    }

    public void UpdateButtons(bool status)
    {
        buttons.ToList().ForEach(button => button.GetComponent<Button>().enabled = status);
    }

    public void SetTurnDurationText(int time)
    {
        UpdateTurnDurationText(true);
        duration.text = "00 : " + time.ToString("00");
    }

    public void UpdateTurnDurationText(bool status)
    {
        duration.gameObject.SetActive(status);
    }
}
