using UnityEngine;
using System.Linq;

public class DeckBuilding : MonoBehaviour
{
    private int playerCoins;
    [SerializeField] private int maximumPlayerCoins = 80;

    [Header("Store")]
    [SerializeField] private CardStorage storeCardStorage;
    [SerializeField] private CardPagination storeCardPagination;

    [Header("Deck")]
    [SerializeField] private CardStorage deckCardStorage;
    [SerializeField] private CardPagination deckCardPagination;

    private void Awake()
    {
        playerCoins = maximumPlayerCoins - deckCardStorage.Collection.Sum(card => card.Coins);
        UIManager.Instance.Coins.text = "Coins: " + playerCoins;
    }
    public void StoreCard()
    {
        Card card = GetSelectedCard(storeCardPagination.CardPrefabs);

        if (card != null)
        {
            if (playerCoins >= card.Coins)
            {
                AudioManager.Instance.Play(Audio.SoundEffects, Clip.Coins, false);
                deckCardStorage.Collection.Add(card);
                storeCardStorage.Collection.Find(defaultCard => defaultCard.Id == card.Id && defaultCard.Type == card.Type).IsAvailable = false;
                RefreshCardPageManager(card.Type);
                UpdateCoins(-card.Coins);
            }
        }
    }

    public void DeleteCard()
    {
        Card card = GetSelectedCard(deckCardPagination.CardPrefabs);

        if (card != null)
        {
            AudioManager.Instance.Play(Audio.SoundEffects, Clip.Coins, false);
            int index = deckCardStorage.Collection.FindIndex(defaultCard => defaultCard.Id == card.Id && defaultCard.Type == card.Type);
            deckCardStorage.Collection.RemoveAt(index);
            storeCardStorage.Collection.Find(defaultCard => defaultCard.Id == card.Id && defaultCard.Type == card.Type).IsAvailable = true;
            RefreshCardPageManager(card.Type);
            UpdateCoins(card.Coins);
        }
    }

    private Card GetSelectedCard(GameObject[] cardPrefabs)
    {
        foreach (GameObject cardPrefab in cardPrefabs)
        {
            if (cardPrefab.GetComponent<CardInteraction>().IsSelected)
            {
                Card card = cardPrefab.GetComponent<CardInfo>().GetAvailableCard();
                cardPrefab.GetComponent<CardInteraction>().IsSelected = false;
                return card;
            }
        }

        return null;
    }

    private void RefreshCardPageManager(string type)
    {
        storeCardPagination.Load(type);
        deckCardPagination.Load(type);
    }

    private void UpdateCoins(int value)
    {
        playerCoins = Mathf.Clamp(playerCoins + value, 0, maximumPlayerCoins);
        UIManager.Instance.Coins.text = "Coins: " + playerCoins;
    }

    public void Save()
    {
        deckCardStorage.SaveCardsAsFiles();
        storeCardStorage.SaveCardsAsFiles();
        UIManager.Instance.Panel.ShowMainMenuPanel();
    }

    private Card SetCardAsAvailable(Card card)
    {
        Card availableCard = new Card();

        availableCard.Id = card.Id;
        availableCard.Name = card.Name;
        availableCard.Coins = card.Coins;
        availableCard.Description = card.Description;
        availableCard.AttackPoints = card.AttackPoints;
        availableCard.LifePoints = card.LifePoints;
        availableCard.IsAvailable = true;

        return availableCard;
    }
}
