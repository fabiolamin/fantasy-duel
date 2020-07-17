using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour
{
    [SerializeField] private Networking networking;
    [SerializeField] private CardStorage deckCardStorage;
    [SerializeField] private InputField playerNameInput;

    public void VerifyNickname()
    {
        if (PlayerPrefs.GetString("Nickname") == "")
            PlayerPrefs.SetString("Nickname", "user" + Random.Range(100, 999));
        else
            networking.Connect();
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

    public void SetDeckAsProperty()
    {
        string json = Utility.GetJsonFrom(deckCardStorage.GetCustomDeck());
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties.Add(PhotonNetwork.NickName, json);
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
    }
}
