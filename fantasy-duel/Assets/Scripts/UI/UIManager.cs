using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    [SerializeField] private GameObject canvasLogo;
    [SerializeField] private ParticleSystem logoParticles;

    [SerializeField] private Text connectionStatus;

    [SerializeField] private CardPagination cardPagination;
    [SerializeField] private CardPagination cardPaginationDeck;

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
    public Text ConnectionStatus { get { return connectionStatus; } set { connectionStatus = value; } }

    private void Awake()
    {
        instance = this;

        Panel = GetComponent<Panel>();
        Panel.ShowMainMenuPanel();

        wins.text = PlayerPrefs.GetInt("Wins").ToString();
        losses.text = PlayerPrefs.GetInt("Losses").ToString();
    }

    public void ShowDecks()
    {
        cardPagination.Load("All");
        cardPaginationDeck.Load("All");
    }

    public void HideLogo()
    {
        canvasLogo.SetActive(false);
        logoParticles.Play();
    }
}
