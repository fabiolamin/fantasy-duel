using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerDeck : MonoBehaviour
{
    private PhotonView photonView;
    private List<Card> cards = new List<Card>();
    private List<GameObject> instantiatedCards = new List<GameObject>();

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject[] handCards;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        GetCards();

        if (photonView.IsMine)
        {
            photonView.RPC("InstantiateCards", RpcTarget.AllBuffered);
            TurnCardsUp();
        }
    }

    private void GetCards()
    {
        string json = photonView.Owner.CustomProperties[photonView.Owner.NickName].ToString();
        cards = CardConverter.GetCardsFrom(json);
    }

    [PunRPC]
    private void InstantiateCards()
    {
        for (int index = 0; index < handCards.Length; index++)
        {
            Card card = cards[index];
            GameObject instantiatedCard = Instantiate(cardPrefab, handCards[index].transform.position, Quaternion.Euler(-90, GetYRotation(), 0));
            instantiatedCard.GetComponent<CardUI>().Set(card);
            instantiatedCard.GetComponent<CardInfo>().Set(card);
            instantiatedCards.Add(instantiatedCard);
        }
    }

    private void TurnCardsUp()
    {
        foreach (GameObject card in instantiatedCards)
        {
            card.transform.rotation = Quaternion.Euler(90, GetYRotation(), 0);
        }
    }

    private int GetYRotation()
    {
        int y = 180;
        if (PhotonNetwork.IsMasterClient)
        {
            y = 0;
        }

        return y;
    }
}
