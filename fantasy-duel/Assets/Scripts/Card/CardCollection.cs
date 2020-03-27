using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCollection : MonoBehaviour
{
    public List<Card> Bases { get; set; } = new List<Card>();
    public List<Card> Creatures { get; set; } = new List<Card>();
    public List<Card> Magics { get; set; } = new List<Card>();

    public void Add(Card card)
    {
        string type = card.Type;

        switch (type)
        {
            case "Bases":
                Bases.Add(card);
                break;

            case "Creatures":
                Creatures.Add(card);
                break;

            case "Magics":
                Magics.Add(card);
                break;
        }
    }
}
