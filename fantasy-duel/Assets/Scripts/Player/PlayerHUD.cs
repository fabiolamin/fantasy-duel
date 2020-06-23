using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;

public class PlayerHUD : MonoBehaviour
{
    private PlayerManager playerManager;
    private float matchMessageDurationAux;
    private bool isReadyToHideMatchMessage = false;

    [SerializeField] private float matchMessageDuration = 2f;
    [SerializeField] private Text lifePoints;
    [SerializeField] private Text coins;
    [SerializeField] private Text rounds;
    [SerializeField] private Text nickname;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] playedCardImages;
    [SerializeField] private Text matchMessage;
    [SerializeField] private Text duration;
    [SerializeField] private GameObject endMatchPanel;
    [SerializeField] private Transform playersRoundsPanel;
    [SerializeField] private GameObject playerRoundsText;
    [SerializeField] private ParticleSystem notificationParticle;

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

        matchMessageDurationAux = matchMessageDuration;
    }

    private void Update()
    {
        if (isReadyToHideMatchMessage)
        {
            CheckMatchMessageCountdown();
        }
    }

    private void CheckMatchMessageCountdown()
    {
        matchMessageDuration -= Time.deltaTime;
        if (matchMessageDuration <= 0)
        {
            HideMatchMessage();
        }
    }

    private void HideMatchMessage()
    {
        ActiveMatchMessage(false);
        matchMessageDuration = matchMessageDurationAux;
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

    public void SetMatchMessage(string message)
    {
        matchMessage.text = message;
    }

    public void ActiveMatchMessage(bool isActivated)
    {
        if (playerManager.PhotonView.IsMine)
        {
            notificationParticle.Play();
            matchMessage.gameObject.SetActive(isActivated);
            isReadyToHideMatchMessage = isActivated;
        }
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

    public void ShowEndMatchPanel()
    {
        playerManager.PlaySoundEffect(Clip.EndMatch);
        endMatchPanel.SetActive(true);

        foreach(var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            GameObject playerRounds = Instantiate(playerRoundsText, playersRoundsPanel);
            int wonRounds = playerManager.PlayerInfo.WonRounds;
            playerRounds.GetComponent<Text>().text = playerManager.PhotonView.Owner.NickName + "  " + wonRounds;
        }
    }
}
