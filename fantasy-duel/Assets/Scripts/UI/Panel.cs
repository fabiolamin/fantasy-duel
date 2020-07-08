using System.Linq;
using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject nicknameCreationPanel;
    [SerializeField] private GameObject matchmakingPanel;
    [SerializeField] private GameObject deckBuildingPanel;
    [SerializeField] private GameObject canvasCardCollection;

    [SerializeField] private GameObject[] cardPlaceholderStore;
    [SerializeField] private GameObject[] cardPlaceholderDeck;

    public void ShowMainMenuPanel()
    {
        DisableAllPanels();
        DisablePlaceholders();
        mainMenuPanel.SetActive(true);
    }

    private void DisableAllPanels()
    {
        mainMenuPanel.SetActive(false);
        nicknameCreationPanel.SetActive(false);
        matchmakingPanel.SetActive(false);
        deckBuildingPanel.SetActive(false);
        canvasCardCollection.SetActive(false);
    }

    private void DisablePlaceholders()
    {
        cardPlaceholderStore.ToList().ForEach(card => card.SetActive(false));
        cardPlaceholderDeck.ToList().ForEach(card => card.SetActive(false));
    }

    public void ShowNicknameCreationPanel()
    {
        DisableAllPanels();
        nicknameCreationPanel.SetActive(true);
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
        UIManager.Instance.ShowCards();
    }
}
