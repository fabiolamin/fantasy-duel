using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Utility
{
    public static int GetYRotation()
    {
        int y = 180;
        if (PhotonNetwork.IsMasterClient)
        {
            y = 0;
        }

        return y;
    }

    public static int GetCardIndexFromList(Card card, List<GameObject> cardCollection)
    {
        foreach (var c in cardCollection)
        {
            if (c.CompareTag("Card"))
            {
                var cardInfo = c.GetComponent<CardInfo>().Card;

                if (card.Id == cardInfo.Id && card.Type == cardInfo.Type)
                {
                    return cardCollection.IndexOf(c);
                }
            }
        }

        return -1;
    }

    public static List<Card> GetCardsFrom(string json)
    {
        CardArray array = JsonUtility.FromJson<CardArray>(json);
        return array.Card.ToList();
    }

    public static string GetJsonFrom(Card[] cards)
    {
        CardArray array = new CardArray();
        array.Card = cards;
        string json = JsonUtility.ToJson(array);
        return json;
    }
}
