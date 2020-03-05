using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject nicknameCreationPanel;
    [SerializeField] private GameObject matchmakingPanel;
    [SerializeField] private GameObject playButton;
    [SerializeField] private InputField playerName;
    [SerializeField] private Text nickname;
    [SerializeField] private Text connectionStatus;
    public InputField PlayerName { get { return playerName; } private set { playerName = value; } }
    public Text ConnectionStatus { get { return connectionStatus; } set { connectionStatus = value; } }

    private void Awake()
    {
        mainMenuPanel.SetActive(true);
        nicknameCreationPanel.SetActive(false);
        matchmakingPanel.SetActive(false);
        playButton.SetActive(false);
    }

    public void EnableNicknameCreationPanel()
    {
        mainMenuPanel.SetActive(false);
        nicknameCreationPanel.SetActive(true);

    }

    public void EnableMatchmakingPanel()
    {
        mainMenuPanel.SetActive(false);
        nicknameCreationPanel.SetActive(false);
        matchmakingPanel.SetActive(true);
    }

    public void SetNicknameText()
    {
        nickname.text = "Nickname: " + PlayerPrefs.GetString("Nickname");
        playButton.SetActive(true);
    }
}
