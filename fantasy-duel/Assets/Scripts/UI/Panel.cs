using System.Linq;
using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField] private PlayerSettings playerSettings;

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject matchmakingPanel;
    [SerializeField] private GameObject deckBuildingPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject canvasCardCollection;

    [SerializeField] private GameObject[] cardPlaceholderStore;
    [SerializeField] private GameObject[] cardPlaceholderDeck;

    public void ShowMainMenuPanel()
    {
        DisableAllPanels();
        HideCards();
        mainMenuPanel.SetActive(true);
    }

    private void DisableAllPanels()
    {
        mainMenuPanel.SetActive(false);
        matchmakingPanel.SetActive(false);
        deckBuildingPanel.SetActive(false);
        settingsPanel.SetActive(false);
        canvasCardCollection.SetActive(false);
    }

    private void HideCards()
    {
        cardPlaceholderStore.ToList().ForEach(card => card.SetActive(false));
        cardPlaceholderDeck.ToList().ForEach(card => card.SetActive(false));
    }

    public void ShowMatchmakingPanel()
    {
        DisableAllPanels();
        matchmakingPanel.SetActive(true);
    }

    public void ShowDeckBuildingPanel()
    {
        UIManager.Instance.ShowDecks();
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
}
