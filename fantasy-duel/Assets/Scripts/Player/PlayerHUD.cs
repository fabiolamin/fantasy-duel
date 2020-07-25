﻿using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;

public class PlayerHUD : MonoBehaviour
{
    private PlayerManager playerManager;
    private float notificationDurationAux;
    private bool isReadyToHideNotification = false;

    [SerializeField] private float notificationDuration = 2f;
    [SerializeField] private Text lifePoints;
    [SerializeField] private Text coins;
    [SerializeField] private Text rounds;
    [SerializeField] private Text nickname;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] playedCardImages;
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private Text notification;
    [SerializeField] private Text duration;
    [SerializeField] private GameObject endMatchPanel;
    [SerializeField] private Transform playersRoundsPanel;
    [SerializeField] private GameObject playerRoundsText;
    [SerializeField] private ParticleSystem notificationParticle;
    [SerializeField] private GameObject leavingPanel;

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

        notificationDurationAux = notificationDuration;
    }

    private void Update()
    {
        if (isReadyToHideNotification)
        {
            CheckNotificationCountdown();
        }
    }

    private void CheckNotificationCountdown()
    {
        notificationDuration -= Time.deltaTime;
        if (notificationDuration <= 0)
        {
            HideNotification();
        }
    }

    private void HideNotification()
    {
        ActiveNotification(false);
        notificationDuration = notificationDurationAux;
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

    public void SetNotification(string message)
    {
        notification.text = message;
    }

    public void ActiveNotification(bool isActivated)
    {
        if (playerManager.PhotonView.IsMine)
        {
            notificationParticle.Play();
            notificationPanel.SetActive(isActivated);
            isReadyToHideNotification = isActivated;
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
        notificationParticle.Play();
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

    public void ActiveLeavingPanel(bool isActivated)
    {
        leavingPanel.SetActive(isActivated);
    }
}
