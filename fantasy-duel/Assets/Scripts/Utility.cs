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
        int index = cardCollection.FindIndex(c => c.GetComponent<CardInfo>().Card.Id == card.Id && c.GetComponent<CardInfo>().Card.Type == card.Type);
        return index;
    }
}
