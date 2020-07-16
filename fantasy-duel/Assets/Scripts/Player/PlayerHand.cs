using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHand : MonoBehaviour
{
    private int index = 0;
    private PlayerManager playerManager;
    private List<Card> convertedCards = new List<Card>();

    [SerializeField] private Transform cardsParent;
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
            while (index < 5)
            {
                playerManager.PhotonView.RPC("InstantiateCard", RpcTarget.AllBuffered);
            }

            playerManager.PhotonView.RPC("OrganizeCards", RpcTarget.AllBuffered);
            TurnHandCardsUp();
        }
    }

    private void GetAllCards()
    {
        string json = playerManager.PhotonView.Owner.CustomProperties[playerManager.PhotonView.Owner.NickName].ToString();
        convertedCards = Utility.GetCardsFrom(json);
    }

    [PunRPC]
    private void InstantiateCard()
    {
        if(index < convertedCards.Count)
        {
            Card card = convertedCards[index];
            GameObject instantiatedCard = Instantiate(cardPrefab, Vector3.zero, Quaternion.Euler(-90, Utility.GetYRotation(), 0), cardsParent);
            instantiatedCard.GetComponent<CardInfo>().Card = card;
            instantiatedCard.GetComponent<CardUI>().Set(card);

            if (card.Type == "Creatures")
            {
                instantiatedCard.GetComponent<CardInfo>().IsProteged = true;
            }

            Cards.Add(instantiatedCard);

            index++;
        }
    }

    public void UpdateHand()
    {
        if (playerManager.PhotonView.IsMine)
        {
            playerManager.PhotonView.RPC("InstantiateCard", RpcTarget.AllBuffered);
            playerManager.PhotonView.RPC("OrganizeCards", RpcTarget.AllBuffered);
            TurnHandCardsUp();
        }
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
            playerManager.PhotonView.RPC("RemoveCard", RpcTarget.AllBuffered, Utility.GetCardIndexFromList(card, Cards));
        }
    }

    [PunRPC]
    private void RemoveCard(int index)
    {
        Cards.RemoveAt(index);
    }

    public void Lock(bool isLocked)
    {
        Cards.ForEach(card => card.GetComponent<CardInteraction>().IsLocked = isLocked);
    }
}
