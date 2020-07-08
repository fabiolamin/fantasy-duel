using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;


public class PlayerSettings : MonoBehaviour
{
    private string nickname;

    [SerializeField] private Networking networking;
    [SerializeField] private CardStorage deckCardStorage;

    public void VerifyNickname()
    {
        if (PlayerPrefs.GetString("Nickname") == "")
        {
            UIManager.Instance.Panel.ShowNicknameCreationPanel();
        }
        else
        {
            networking.Connect();
        }
    }

    public void CreateNickname()
    {
        int random = Random.Range(100, 999);
        nickname = UIManager.Instance.PlayerName.text + random.ToString();
        PlayerPrefs.SetString("Nickname", nickname);
        UIManager.Instance.SetNicknameText();
    }

    public void SetDeckAsProperty()
    {
        string json = CardConverter.GetJsonFrom(GetCustomDeck());
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties.Add(PhotonNetwork.NickName, json);
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }

    private Card[] GetCustomDeck()
    {
        List<Card> customDeck = new List<Card>();
        int cardsLength = deckCardStorage.Collection.Count;

        while (cardsLength > 0)
        {
            int index = Random.Range(0, cardsLength);
            Card card = deckCardStorage.Collection[index];
            customDeck.Add(card);
            deckCardStorage.Collection.RemoveAt(index);
            cardsLength--;
        }

        return customDeck.ToArray();
    }
}
