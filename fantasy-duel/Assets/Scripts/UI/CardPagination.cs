using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardPagination : MonoBehaviour
{
    private int start = 0;
    private int end = 4;
    private int pages;
    private int cardsPerPage;

    [SerializeField] private CardStorage cardStorage;
    [SerializeField] private GameObject[] cardPrefabs;

    public GameObject[] CardPrefabs { get { return cardPrefabs; } }
    public List<Card> Cards { get; set; }

    private void Awake()
    {
        cardStorage.SaveCardsAsList();
        cardsPerPage = cardPrefabs.Length;
    }

    public void Load(string type)
    {
        GetCards(type);
        SetPagesNumber();
        SetInterval(0, cardsPerPage);
        SetPage();
    }

    private void GetCards(string type)
    {
        if (type.Equals("All"))
        {
            Cards = cardStorage.Collection.OrderBy(card => card.Coins).ToList();
        }
        else
        {
            Cards = cardStorage.Collection.FindAll(card => card.Type == type).OrderBy(card => card.Coins).ToList();
        }
    }

    private void SetPagesNumber()
    {
        pages = Mathf.CeilToInt(Cards.Count / cardsPerPage);
        if ((Cards.Count % cardsPerPage) > 0)
        {
            pages++;
        }
    }

    private void SetInterval(int newStart, int newEnd)
    {
        start = newStart;
        end = newEnd;
    }

    private void SetPage()
    {
        cardPrefabs.ToList().ForEach(cardPrefab => cardPrefab.SetActive(false));

        for (int index = start; index < end; index++)
        {
            if (index < Cards.Count && Cards.Count != 0)
            {
                Card card = Cards[index];
                int position = index % cardsPerPage;
                cardPrefabs[position].SetActive(true);
                cardPrefabs[position].GetComponent<CardInfo>().Card = card;
                cardPrefabs[position].GetComponent<CardUI>().Set(card);
            }
        }
    }

    public void ChangePage(int direction)
    {
        int value = cardsPerPage * direction;
        int newStart = Mathf.Clamp(start + value, 0, (pages * cardsPerPage) - cardsPerPage);
        int newEnd = Mathf.Clamp(end + value, cardsPerPage, (pages * cardsPerPage));
        SetInterval(newStart, newEnd);
        SetPage();
    }
}
