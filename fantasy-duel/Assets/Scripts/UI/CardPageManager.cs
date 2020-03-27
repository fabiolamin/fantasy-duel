using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardPageManager : MonoBehaviour
{
    private int start = 0;
    private int end = 6;
    private int pages;
    private int cardsPerPage;

    [SerializeField] private CardCollection cardCollection;
    [SerializeField] private GameObject[] cardPlaceholders;
    public List<Card> Cards { get; set; }

    private void Awake()
    {
        cardsPerPage = cardPlaceholders.Length;
    }

    public void SetList(string type)
    {
        switch (type)
        {
            case "Bases":
                Cards = cardCollection.Bases;
                SetPagesNumber();
                SetInterval(0, cardsPerPage);
                SetPage();
                break;

            case "Creatures":
                Cards = cardCollection.Creatures;
                SetPagesNumber();
                SetInterval(0, cardsPerPage);
                SetPage();
                break;

            case "Magics":
                Cards = cardCollection.Magics;
                SetPagesNumber();
                SetInterval(0, cardsPerPage);
                SetPage();
                break;
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
        cardPlaceholders.ToList().ForEach(cardPlaceholder => cardPlaceholder.SetActive(false));

        for (int index = start; index < end; index++)
        {
            if (index < Cards.Count)
            {
                Card card = Cards[index];
                int position = index % cardsPerPage;
                cardPlaceholders[position].SetActive(true);
                cardPlaceholders[position].GetComponent<CardUI>().Set(card);
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
