﻿using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour
{
    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    [SerializeField] private Networking networking;
    [SerializeField] private CardStorage deckCardStorage;
    [SerializeField] private InputField playerNameInput;
    [SerializeField] private int minCardsToPlay = 20;

    public void VerifyPlayer()
    {
        if (CanPlayerStartAMatch())
            networking.Connect();
        else
            PanelManager.Instance.ShowNotificationPanel();
    }

    private bool CanPlayerStartAMatch()
    {
        return PlayerPrefs.GetString("Nickname") != "" && deckCardStorage.Collection.Count > minCardsToPlay &&
        PlayerPrefs.GetInt("Character") > 0;
    }

    public void UpdateNickname()
    {
        if (playerNameInput.text != PlayerPrefs.GetString("Nickname"))
        {
            int random = Random.Range(100, 999);
            string nickname = playerNameInput.text + random.ToString();
            PlayerPrefs.SetString("Nickname", nickname);
            ShowNickname();
        }
    }

    public void ShowNickname()
    {
        playerNameInput.text = PlayerPrefs.GetString("Nickname");
    }

    public void SetProperties()
    {
        SetDeck();
        SetCharacter();
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerProperties);
    }

    private void SetDeck()
    {
        string json = Utility.GetJsonFrom(deckCardStorage.GetCustomDeck());
        playerProperties.Add(PhotonNetwork.NickName, json);
    }

    private void SetCharacter()
    {
        playerProperties.Add(PhotonNetwork.NickName + "-Character", PlayerPrefs.GetInt("Character"));
    }
}
