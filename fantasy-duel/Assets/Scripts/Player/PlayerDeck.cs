using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class PlayerDeck : MonoBehaviour
{
    private PhotonView photonView;
    private List<Card> allCards = new List<Card>();
    private List<GameObject> deck = new List<GameObject>();
    private List<GameObject> handCards = new List<GameObject>();

    [SerializeField] private Transform deckPosition;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject[] handCardsPositions;

    [Header("When mouse is over a card")]
    [SerializeField] private float scale = 1f;
    [SerializeField] private float height = 1.4f;
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
            GameObject instantiatedCard = Instantiate(cardPrefab, deckPosition.position, Quaternion.Euler(-90, GetYRotation(), 0), transform);
            instantiatedCard.GetComponent<CardUI>().Set(card);
            instantiatedCard.GetComponent<CardInfo>().Set(card);
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

    private int GetIndex(Card card)
    {
        int index = handCards.FindIndex(c => c.GetComponent<CardInfo>().Id == card.Id && c.GetComponent<CardInfo>().Type == card.Type);
        return index;
    }

    public void RaiseCard(Card card)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RaiseCardRPC", RpcTarget.AllBuffered, GetIndex(card));
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
            photonView.RPC("IncreaseCardScaleRPC", RpcTarget.AllBuffered, GetIndex(card));
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
            photonView.RPC("SetInitialTransformRPC", RpcTarget.AllBuffered, GetIndex(card));
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
            photonView.RPC("SetCardInBoardAreaRPC", RpcTarget.AllBuffered, GetIndex(card), cardPosition);
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
        Card card = handCard.GetComponent<CardInfo>().GetCard();
        if(photonView.IsMine)
        {
            photonView.RPC("RemoveCardFromHandCardsRPC", RpcTarget.AllBuffered, GetIndex(card));
        }
    }

    [PunRPC]
    private void RemoveCardFromHandCardsRPC(int index)
    {
        handCards.RemoveAt(index);
    }
}
