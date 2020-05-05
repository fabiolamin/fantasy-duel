using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using ExitGames.Client.Photon;

public class PlayerDeck : MonoBehaviourPunCallbacks
{
    private PhotonView photonView;
    private List<Card> allCards = new List<Card>();
    private List<GameObject> deck = new List<GameObject>();
    private List<GameObject> handCards = new List<GameObject>();
    private GameObject playedCard;
    private Card selectedPlayedCard;
    private int selectedPlayedCardIndex;

    [SerializeField] private Transform cardsParent;
    [SerializeField] private Transform deckPosition;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject[] handCardsPositions;

    [Header("When mouse is over a card")]
    [SerializeField] private float scale = 1f;
    [SerializeField] private float height = 1.4f;

    public List<GameObject> PlayedCards { get; set; }
    public Transform CardsParent { get { return cardsParent; } }
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        GetAllCards();

        if(photonView.IsMine)
        {
            photonView.RPC("BuildDeckRPC", RpcTarget.AllBuffered);

            while (handCards.Count < 5)
            {
                photonView.RPC("AddCardToHandCardsRPC", RpcTarget.AllBuffered);
            }

            photonView.RPC("OrganizeHandCardsRPC", RpcTarget.AllBuffered);
            TurnHandCardsUp();
        }

        PlayedCards = new List<GameObject>();
    }

    private void GetAllCards()
    {
        string json = photonView.Owner.CustomProperties[photonView.Owner.NickName].ToString();
        allCards = CardConverter.GetCardsFrom(json);
    }

    [PunRPC]
    private void BuildDeckRPC()
    {
        for (int index = 0; index < allCards.Count; index++)
        {
            Card card = allCards[index];
            GameObject instantiatedCard = Instantiate(cardPrefab, deckPosition.position, Quaternion.Euler(-90, GetYRotation(), 0), cardsParent);
            instantiatedCard.GetComponent<CardUI>().Set(card);
            instantiatedCard.GetComponent<CardInfo>().Card = card;
            instantiatedCard.SetActive(false);
            deck.Add(instantiatedCard);
        }
    }
    public void UpdateHandCards()
    {
        if(photonView.IsMine)
        {
            photonView.RPC("AddCardToHandCardsRPC", RpcTarget.AllBuffered);
            photonView.RPC("OrganizeHandCardsRPC", RpcTarget.AllBuffered);
            TurnHandCardsUp();
        }
    }

    [PunRPC]
    private void AddCardToHandCardsRPC()
    {
        GameObject card = new GameObject();

        if (deck.Count != 0)
        {
            card = deck.First();
        }
     
        deck.Remove(card);
        handCards.Add(card);
        card.SetActive(true);
    }

    [PunRPC]
    public void OrganizeHandCardsRPC()
    {
        for (int index = 0; index < handCardsPositions.Length; index++)
        {
            handCards[index].transform.position = handCardsPositions[index].transform.position;
        }
    }
    private void TurnHandCardsUp()
    {
        foreach (GameObject card in handCards)
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

    private int GetIndexFromHandCards(Card card)
    {
        int index = handCards.FindIndex(c => c.GetComponent<CardInfo>().Card.Id == card.Id && c.GetComponent<CardInfo>().Card.Type == card.Type);
        return index;
    }

    private int GetIndexFromPlayedCards(Card card)
    {
        int index = PlayedCards.FindIndex(c => c.GetComponent<CardInfo>().Card.Id == card.Id && c.GetComponent<CardInfo>().Card.Type == card.Type);
        return index;
    }

    public void RaiseCard(Card card)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RaiseCardRPC", RpcTarget.AllBuffered, GetIndexFromHandCards(card));
        }
    }

    [PunRPC]
    private void RaiseCardRPC(int index)
    {
        handCards[index].transform.position += Vector3.up * height;
    }

    public void IncreaseCardScale(Card card)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("IncreaseCardScaleRPC", RpcTarget.AllBuffered, GetIndexFromHandCards(card));
        }
    }

    [PunRPC]
    private void IncreaseCardScaleRPC(int index)
    {
        handCards[index].transform.localScale += new Vector3(scale, scale, 0);
    }

    public void SetInitialTransform(Card card)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("SetInitialTransformRPC", RpcTarget.AllBuffered, GetIndexFromHandCards(card));
        }
    }

    [PunRPC]
    private void SetInitialTransformRPC(int index)
    {
        Vector3 scale = handCardsPositions[index].transform.localScale;
        handCards[index].transform.position = handCardsPositions[index].transform.position;
        handCards[index].transform.localScale = new Vector3(scale.x, scale.z, 0.00001f);
    }

    public void SetCardInBoardArea(Card card, Vector3 position)
    {
        if (photonView.IsMine)
        {
            float[] cardPosition = new float[] { position.x, position.y, position.z };
            photonView.RPC("SetCardInBoardAreaRPC", RpcTarget.AllBuffered, GetIndexFromHandCards(card), cardPosition);
        }
    }

    [PunRPC]
    private void SetCardInBoardAreaRPC(int index, float[] position)
    {
        handCards[index].transform.position = new Vector3(position[0], position[1], position[2]);
        handCards[index].transform.rotation = Quaternion.Euler(90, GetYRotation(), 0);
    }

    public void UpdateDeckLock(bool isLocked)
    {
        handCards.ForEach(card => card.GetComponent<CardInteraction>().IsLocked = isLocked);
    }

    public void RemoveCardFromHandCards(GameObject handCard)
    {
        Card card = handCard.GetComponent<CardInfo>().Card;
        if(photonView.IsMine)
        {
            photonView.RPC("RemoveCardFromHandCardsRPC", RpcTarget.AllBuffered, GetIndexFromHandCards(card));
        }
    }

    [PunRPC]
    private void RemoveCardFromHandCardsRPC(int index)
    {
        handCards.RemoveAt(index);
    }

    public void UpdateLifePoints(int id, string type, int amountToDecrease)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("UpdateLifePointsRPC", RpcTarget.AllBuffered, id, type, amountToDecrease);
        }
    }

    [PunRPC]
    private void UpdateLifePointsRPC(int id, string type, int amount)
    {
        foreach(Transform card in CardsParent)
        {
            if(card.GetComponent<CardInfo>().Card.Id == id && card.GetComponent<CardInfo>().Card.Type == type)
            {
                card.GetComponent<CardInfo>().Card.LifePoints -= amount;
                card.GetComponent<CardUI>().Set(card.GetComponent<CardInfo>().Card);
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == photonView.Owner && photonView.IsMine && changedProps.ContainsKey("EnemyCardID"))
        {
            int id = (int)changedProps["EnemyCardID"];
            string type = changedProps["EnemyCardType"].ToString();
            int damage = (int)changedProps["EnemyCardDamage"];

            UpdateLifePoints(id, type, damage);
        }
    }
}
