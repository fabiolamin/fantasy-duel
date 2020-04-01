using UnityEngine;
using System.Linq;

public class DeckBuilding : MonoBehaviour
{
    private int playerCoins;
    [SerializeField] private int maximumPlayerCoins = 80;

    [SerializeField] private UIManager uiManager;

    [Header("Store")]
    [SerializeField] private CardCollection storeCardCollection;
    [SerializeField] private CardConversion storeCardConversion;
    [SerializeField] private CardPageManager storeCardPageManager;

    [Header("Deck")]
    [SerializeField] private CardCollection deckCardCollection;
    [SerializeField] private CardConversion deckCardConversion;
    [SerializeField] private CardPageManager deckCardPageManager;

    private void Awake()
    {
        playerCoins = maximumPlayerCoins - deckCardCollection.Cards.Sum(card => card.Coins);
        uiManager.Coins.text = "Coins: " + playerCoins;
    }
    public void StoreCard()
    {
        Card card = GetSelectedCard(storeCardPageManager.CardPrefabs);

        if (card != null)
        {
            if (playerCoins >= card.Coins)
            {
                deckCardCollection.Cards.Add(card);
                storeCardCollection.Cards.Find(defaultCard => defaultCard.Id == card.Id && defaultCard.Type == card.Type).IsAvailable = false;
                RefreshCardPageManager(card.Type);
                UpdateCoins(-card.Coins);
            }
        }
    }

    public void DeleteCard()
    {
        Card card = GetSelectedCard(deckCardPageManager.CardPrefabs);
        int index = deckCardCollection.Cards.FindIndex(defaultCard => defaultCard.Id == card.Id && defaultCard.Type == card.Type);
        deckCardCollection.Cards.RemoveAt(index);
        storeCardCollection.Cards.Find(defaultCard => defaultCard.Id == card.Id && defaultCard.Type == card.Type).IsAvailable = true;
        RefreshCardPageManager(card.Type);
        UpdateCoins(card.Coins);
    }

    private Card GetSelectedCard(GameObject[] cardPrefabs)
    {
        foreach (GameObject cardPrefab in cardPrefabs)
        {
            if (cardPrefab.GetComponent<CardInteraction>().IsSelected)
            {
                Card card = cardPrefab.GetComponent<CardInfo>().GetCard();
                cardPrefab.GetComponent<CardInteraction>().IsSelected = false;
                return card;
            }
        }

        return null;
    }

    private void RefreshCardPageManager(string type)
    {
        storeCardPageManager.SetList(type);
        deckCardPageManager.SetList(type);
    }

    private void UpdateCoins(int value)
    {
        playerCoins = Mathf.Clamp(playerCoins + value, 0, maximumPlayerCoins);
        uiManager.Coins.text = "Coins: " + playerCoins;
    }

    public void Save()
    {
        deckCardConversion.Serialize();
        storeCardConversion.Serialize();
        uiManager.ShowMainMenuPanel();
    }
}
