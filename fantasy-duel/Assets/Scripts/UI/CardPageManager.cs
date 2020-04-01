﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardPageManager : MonoBehaviour
{
    private int start = 0;
    private int end = 6;
    private int pages;
    private int cardsPerPage;

    [SerializeField] private CardConversion cardConversion;
    [SerializeField] private CardCollection cardCollection;
    [SerializeField] private GameObject[] cardPrefabs;

    public GameObject[] CardPrefabs { get { return cardPrefabs; }}
    public List<Card> Cards { get; set; }

    private void Awake()
    {
        cardConversion.Deserialize();
        cardsPerPage = cardPrefabs.Length;
    }

    public void SetList(string type)
    {
        Cards = cardCollection.Cards.FindAll(card => card.Type == type).OrderBy(card => card.Id).ToList();
        SetPagesNumber();
        SetInterval(0, cardsPerPage);
        SetPage();
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
            if (index < Cards.Count)
            {
                Card card = Cards[index];
                int position = index % cardsPerPage;
                cardPrefabs[position].SetActive(true);
                cardPrefabs[position].GetComponent<CardUI>().Set(card);
                cardPrefabs[position].GetComponent<CardInfo>().Set(card);
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
