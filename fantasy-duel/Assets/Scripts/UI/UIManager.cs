using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject nicknameCreationPanel;
    [SerializeField] private GameObject matchmakingPanel;
    [SerializeField] private GameObject deckBuildingPanel;
    [SerializeField] private GameObject canvasCardCollection;
    [SerializeField] private GameObject canvasLogo;
    [SerializeField] private ParticleSystem logoParticles;

    [SerializeField] private GameObject playButton;
    [SerializeField] private InputField playerName;
    [SerializeField] private Text nickname;

    [SerializeField] private Text connectionStatus;

    [SerializeField] private GameObject[] cardPlaceholder;
    [SerializeField] private GameObject[] cardPlaceholderDeck;

    [SerializeField] private CardPagination cardPagination;
    [SerializeField] private CardPagination cardPaginationDeck;

    [SerializeField] private Text coins;

    public static UIManager Instance
    {
        get
        {
            if(instance == null)
            {
                Debug.LogError("UI Manager is NULL.");
            }

            return instance;
        }
    }
    public InputField PlayerName { get { return playerName; } private set { playerName = value; } }
    public Text ConnectionStatus { get { return connectionStatus; } set { connectionStatus = value; } }
    public Text Coins { get { return coins; } set { coins = value; } }

    private void Awake()
    {
        instance = this;
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
        cardPagination.Load("Creatures");
        cardPaginationDeck.Load("Creatures");
    }

    public void SetNicknameText()
    {
        nickname.text = "Nickname: " + PlayerPrefs.GetString("Nickname");
        playButton.SetActive(true);
    }

    public void HideLogo()
    {
        canvasLogo.SetActive(false);
        logoParticles.Play();
    }
}
