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

    [SerializeField] private Transform cardsParent;
    [SerializeField] private Transform deckPosition;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject[] handCardsPositions;
    [SerializeField] private GameObject playerLifeObject;

    [Header("When mouse is over a card")]
    [SerializeField] private float scale = 1f;
    [SerializeField] private float height = 1.4f;

    public List<GameObject> PlayedCards { get; private set; }
    public List<GameObject> SelectableObjects { get; private set; }
    public Transform CardsParent { get { return cardsParent; } }
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        GetAllCards();

        if (photonView.IsMine)
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
        SelectableObjects = new List<GameObject>();
        SelectableObjects.Add(playerLifeObject);
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
            if (card.Type == "Creatures")
            {
                instantiatedCard.GetComponent<CardInfo>().IsProteged = true;
            }
            instantiatedCard.SetActive(false);
            deck.Add(instantiatedCard);
        }
    }
    public void UpdateHandCards()
    {
        if (photonView.IsMine)
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

    public void UpdateCardsLock(bool isLocked)
    {
        handCards.ForEach(card => card.GetComponent<CardInteraction>().IsLocked = isLocked);
        PlayedCards.ForEach(card => card.GetComponent<CardInteraction>().IsLocked = isLocked);
    }

    public void AddCardToPlayedCards(GameObject card)
    {
        Card playedCard = card.GetComponent<CardInfo>().Card;

        if (photonView.IsMine)
        {
            photonView.RPC("AddCardToPlayedCardsRPC", RpcTarget.AllBuffered, GetIndexFromHandCards(playedCard));
        }
    }

    [PunRPC]
    private void AddCardToPlayedCardsRPC(int index)
    {
        PlayedCards.Add(handCards[index]);
        SelectableObjects.Add(handCards[index]);
    }

    public void RemoveCardFromHandCards(GameObject handCard)
    {
        Card card = handCard.GetComponent<CardInfo>().Card;
        if (photonView.IsMine)
        {
            photonView.RPC("RemoveCardFromHandCardsRPC", RpcTarget.AllBuffered, GetIndexFromHandCards(card));
        }
    }

    [PunRPC]
    private void RemoveCardFromHandCardsRPC(int index)
    {
        handCards.RemoveAt(index);
    }

    public void UpdateLifePoints(string tag, int id, string type, int amountToDecrease)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("UpdateLifePointsRPC", RpcTarget.AllBuffered, tag, id, type, amountToDecrease);
        }
    }

    [PunRPC]
    private void UpdateLifePointsRPC(string tag, int id, string type, int amount)
    {
        if (tag == "PlayerLife")
        {
            GetComponent<IDamageable>().Damage(amount);
        }
        else
        {
            foreach (GameObject selectableObject in SelectableObjects)
            {
                if (selectableObject.CompareTag("Card"))
                {
                    if (selectableObject.GetComponent<CardInfo>().Card.Id == id && selectableObject.GetComponent<CardInfo>().Card.Type == type)
                    {
                        selectableObject.GetComponent<IDamageable>().Damage(amount);
                        return;
                    }
                }

            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == photonView.Owner && changedProps.ContainsKey("CardID"))
        {
            string tag = changedProps["Tag"].ToString();
            int id = (int)changedProps["CardID"];
            string type = changedProps["CardType"].ToString();
            int damage = (int)changedProps["EnemyCardDamage"];

            UpdateLifePoints(tag, id, type, damage);
        }
    }
}
