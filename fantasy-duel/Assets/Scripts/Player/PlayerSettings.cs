using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    private string nickname;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private NetworkingManager networkingManager;
    
    public void VerifyNickname()
    {
        if (PlayerPrefs.GetString("Nickname") == "")
        {
            uiManager.EnableNicknameCreationPanel();
        }
        else
        {
            networkingManager.Connect();
        }
    }

    public void CreateNickname()
    {
        int random = Random.Range(100, 999);
        nickname = uiManager.PlayerName.text + random.ToString();
        PlayerPrefs.SetString("Nickname", nickname);
        uiManager.SetNicknameText();
    }
}
