using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

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
}
