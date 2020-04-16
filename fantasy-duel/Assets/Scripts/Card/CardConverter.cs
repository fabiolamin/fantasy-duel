using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardConverter : MonoBehaviour
{
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
