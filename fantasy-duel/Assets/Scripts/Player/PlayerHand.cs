using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class PlayerHand : MonoBehaviour
{
    private PlayerManager playerManager;
    private List<Card> convertedCards = new List<Card>();
    private List<GameObject> deck = new List<GameObject>();

    [SerializeField] private Transform cardsParent;
    [SerializeField] private Transform deckPosition;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform[] cardsTransform;

    public List<GameObject> Cards { get; private set; } = new List<GameObject>();
    public Transform[] CardsTransform { get { return cardsTransform; } }

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();

        GetAllCards();

        if (playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("BuildDeck", RpcTarget.AllBuffered);

            while (Cards.Count < 5)
            {
                playerManager.PhotonView.RPC("AddCard", RpcTarget.AllBuffered);
            }

            playerManager.PhotonView.RPC("OrganizeCards", RpcTarget.AllBuffered);
            TurnHandCardsUp();
        }
    }

    private void GetAllCards()
    {
        string json = playerManager.PhotonView.Owner.CustomProperties[playerManager.PhotonView.Owner.NickName].ToString();
        convertedCards = CardConverter.GetCardsFrom(json);
    }

    [PunRPC]
    private void BuildDeck()
    {
        for (int index = 0; index < convertedCards.Count; index++)
        {
            Card card = convertedCards[index];
            GameObject instantiatedCard = Instantiate(cardPrefab, deckPosition.position, Quaternion.Euler(-90, Utility.GetYRotation(), 0), cardsParent);
            instantiatedCard.GetComponent<CardUI>().Set(card);
            instantiatedCard.GetComponent<CardInfo>().Card = card;
            if (card.Type == "Creatures")
            {
                instantiatedCard.GetComponent<CardInfo>().IsProteged = true;
            }
            instantiatedCard.SetActive(false);
            deck.Add(instantiatedCard);
        }
    }
    public void UpdateHand()
    {
        if (playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("AddCard", RpcTarget.AllBuffered);
            playerManager.PhotonView.RPC("OrganizeCards", RpcTarget.AllBuffered);
            TurnHandCardsUp();
        }
    }

    [PunRPC]
    private void AddCard()
    {
        GameObject card = new GameObject();

        if (deck.Count != 0)
        {
            card = deck.First();
        }

        deck.Remove(card);
        Cards.Add(card);
        card.SetActive(true);
    }

    [PunRPC]
    public void OrganizeCards()
    {
        for (int index = 0; index < CardsTransform.Length; index++)
        {
            Cards[index].transform.position = CardsTransform[index].position;
        }
    }
    private void TurnHandCardsUp()
    {
        foreach (GameObject card in Cards)
        {
            card.transform.rotation = Quaternion.Euler(90, Utility.GetYRotation(), 0);
        }
    }

    public void RemoveCard(GameObject handCard)
    {
        Card card = handCard.GetComponent<CardInfo>().Card;
        if (playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("RemoveCard", RpcTarget.AllBuffered, GetIndex(card));
        }
    }

    [PunRPC]
    private void RemoveCard(int index)
    {
        Cards.RemoveAt(index);
    }

    public int GetIndex(Card card)
    {
        int index = Cards.FindIndex(c => c.GetComponent<CardInfo>().Card.Id == card.Id && c.GetComponent<CardInfo>().Card.Type == card.Type);
        return index;
    }

    public void Lock(bool isLocked)
    {
        Cards.ForEach(card => card.GetComponent<CardInteraction>().IsLocked = isLocked);
    }
}
