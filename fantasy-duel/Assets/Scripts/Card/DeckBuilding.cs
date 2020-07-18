using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class DeckBuilding : MonoBehaviour
{
    private int playerCoins;

    [SerializeField] private int maximumPlayerCoins = 80;

    [SerializeField] private Text coins;

    [Header("Store")]
    [SerializeField] private CardStorage storeCardStorage;
    [SerializeField] private CardPagination storeCardPagination;

    [Header("Deck")]
    [SerializeField] private CardStorage deckCardStorage;
    [SerializeField] private CardPagination deckCardPagination;

    private void Awake()
    {
        playerCoins = maximumPlayerCoins - deckCardStorage.Collection.Sum(card => card.Coins);
        coins.text = playerCoins + "/80";
    }
    public void StoreCard()
    {
        Card card = GetSelectedCard(storeCardPagination.CardPrefabs);

        if (card != null)
        {
            if (playerCoins >= card.Coins)
            {
                AudioManager.Instance.Play(Audio.SoundEffects, Clip.Coins, false);
                storeCardStorage.Collection.Remove(card);
                deckCardStorage.Collection.Add(card);
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
            deckCardStorage.Collection.Remove(card);
            storeCardStorage.Collection.Add(card);
            RefreshCardPageManager(card.Type);
            UpdateCoins(card.Coins);
        }
    }

    private Card GetSelectedCard(GameObject[] cards)
    {
        foreach (GameObject card in cards)
        {
            if (card.GetComponent<CardInteraction>().IsSelected)
            {
                card.GetComponent<CardInteraction>().IsSelected = false;
                return card.GetComponent<CardInfo>().Card;
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
        coins.text = playerCoins + "/80";
    }

    public void Save()
    {
        deckCardStorage.SaveCardsAsFiles();
        storeCardStorage.SaveCardsAsFiles();
        PanelManager.Instance.ShowMainMenuPanel();
    }
}
