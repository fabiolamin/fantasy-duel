using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject nicknameCreationPanel;
    [SerializeField] private GameObject matchmakingPanel;
    [SerializeField] private GameObject deckBuildingPanel;
    [SerializeField] private GameObject canvasCardCollection;

    [SerializeField] private GameObject playButton;
    [SerializeField] private InputField playerName;
    [SerializeField] private Text nickname;

    [SerializeField] private Text connectionStatus;

    [SerializeField] private GameObject[] cardPlaceholder;
    [SerializeField] private GameObject[] cardPlaceholderDeck;

    [SerializeField] private CardPageManager cardPageManager;
    [SerializeField] private CardPageManager cardPageManagerDeck;

    [SerializeField] private Text coins;

    public InputField PlayerName { get { return playerName; } private set { playerName = value; } }
    public Text ConnectionStatus { get { return connectionStatus; } set { connectionStatus = value; } }
    public Text Coins { get { return coins; } set { coins = value; } }

    private void Awake()
    {
        ShowMainMenuPanel();
    }

    public void ShowMainMenuPanel()
    {
        mainMenuPanel.SetActive(true);
        nicknameCreationPanel.SetActive(false);
        matchmakingPanel.SetActive(false);
        playButton.SetActive(false);
        deckBuildingPanel.SetActive(false);
        canvasCardCollection.SetActive(false);
        SetPlaceholdersAs(false);
    }

    private void SetPlaceholdersAs(bool status)
    {
        cardPlaceholder.ToList().ForEach(card => card.SetActive(status));
        cardPlaceholderDeck.ToList().ForEach(card => card.SetActive(status));
    }

    public void ShowNicknameCreationPanel()
    {
        nicknameCreationPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void ShowMatchmakingPanel()
    {
        matchmakingPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        nicknameCreationPanel.SetActive(false);
    }

    public void ShowDeckBuildingPanel()
    {
        deckBuildingPanel.SetActive(true);
        canvasCardCollection.SetActive(true);
        mainMenuPanel.SetActive(false);
        ShowCards();
    }

    public void ShowCards()
    {
        cardPageManager.SetList("Creatures");
        cardPageManagerDeck.SetList("Creatures");
    }

    public void SetNicknameText()
    {
        nickname.text = "Nickname: " + PlayerPrefs.GetString("Nickname");
        playButton.SetActive(true);
    }
}
