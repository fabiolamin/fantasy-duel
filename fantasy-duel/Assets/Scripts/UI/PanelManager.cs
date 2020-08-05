using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    private static PanelManager instance;

    [SerializeField] private Text wins, losses;

    [SerializeField] private PlayerSettings playerSettings;

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject matchmakingPanel;
    [SerializeField] private GameObject deckBuildingPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject characterPanel;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private GameObject canvasCardCollection;

    public static PanelManager Instance
    {
        get
        {
            if (instance == null)
                Debug.LogError("PanelManager is NULL.");

            return instance;
        }
    }

    private void Awake()
    {
        instance = this;

        ShowMainMenuPanel();

        wins.text = PlayerPrefs.GetInt("Wins").ToString();
        losses.text = PlayerPrefs.GetInt("Losses").ToString();
    }

    public void ShowMainMenuPanel()
    {
        DisableAllPanels();
        mainMenuPanel.SetActive(true);
    }

    private void DisableAllPanels()
    {
        mainMenuPanel.SetActive(false);
        matchmakingPanel.SetActive(false);
        deckBuildingPanel.SetActive(false);
        settingsPanel.SetActive(false);
        canvasCardCollection.SetActive(false);
        characterPanel.SetActive(false);
        notificationPanel.SetActive(false);
        tutorialPanel.SetActive(false);
    }

    public void ShowMatchmakingPanel()
    {
        DisableAllPanels();
        matchmakingPanel.SetActive(true);
    }

    public void ShowDeckBuildingPanel()
    {
        DisableAllPanels();
        deckBuildingPanel.SetActive(true);
        canvasCardCollection.SetActive(true);
    }

    public void ShowSettingsPanel()
    {
        DisableAllPanels();
        settingsPanel.SetActive(true);
        playerSettings.ShowNickname();
    }

    public void ShowCharacterPanel()
    {
        DisableAllPanels();
        characterPanel.SetActive(true);
    }

    public void ShowNotificationPanel()
    {
        notificationPanel.SetActive(true);
    }

    public void ShowTutorialPanel()
    {
        tutorialPanel.SetActive(true);
    }
}
