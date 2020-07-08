using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    [SerializeField] private GameObject canvasLogo;
    [SerializeField] private ParticleSystem logoParticles;

    [SerializeField] private GameObject playButton;
    [SerializeField] private InputField playerName;
    [SerializeField] private Text nickname;

    [SerializeField] private Text connectionStatus;

    [SerializeField] private CardPagination cardPagination;
    [SerializeField] private CardPagination cardPaginationDeck;

    [SerializeField] private Text coins;

    [SerializeField] private Text wins, losses;

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

    public Panel Panel { get; private set; }
    public InputField PlayerName { get { return playerName; } private set { playerName = value; } }
    public Text ConnectionStatus { get { return connectionStatus; } set { connectionStatus = value; } }
    public Text Coins { get { return coins; } set { coins = value; } }

    private void Awake()
    {
        instance = this;

        Panel = GetComponent<Panel>();
        Panel.ShowMainMenuPanel();

        wins.text = PlayerPrefs.GetInt("Wins").ToString();
        losses.text = PlayerPrefs.GetInt("Losses").ToString();
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
