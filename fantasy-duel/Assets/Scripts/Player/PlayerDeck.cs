using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class PlayerDeck : MonoBehaviour
{
    private PhotonView photonView;
    private List<Card> cards = new List<Card>();
    private List<GameObject> instantiatedCards = new List<GameObject>();

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject[] handCards;

    [Header("When mouse is over a card")]
    [SerializeField] private float scale = 1f;
    [SerializeField] private float height = 1.4f;

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
            GameObject instantiatedCard = Instantiate(cardPrefab, handCards[index].transform.position, Quaternion.Euler(-90, GetYRotation(), 0), transform);
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

    private int GetIndex(Card card)
    {
        int index = instantiatedCards.FindIndex(c => c.GetComponent<CardInfo>().Id == card.Id && c.GetComponent<CardInfo>().Type == card.Type);
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
        instantiatedCards[index].transform.position += Vector3.up * height;
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
        instantiatedCards[index].transform.localScale += new Vector3(scale, scale, 0);
    }

    public void SetInitialTransform(Card card, Vector3 position, Vector3 scale)
    {
        if (photonView.IsMine)
        {
            float[] cardPosition = new float[] { position.x, position.y, position.z };
            float[] cardScale = new float[] { scale.x, scale.y, scale.z };
            photonView.RPC("SetInitialTransformRPC", RpcTarget.AllBuffered, GetIndex(card), cardPosition, cardScale);
        }
    }

    [PunRPC]
    private void SetInitialTransformRPC(int index, float[] position, float[] scale)
    {
        instantiatedCards[index].transform.position = new Vector3(position[0], position[1], position[2]);
        instantiatedCards[index].transform.localScale = new Vector3(scale[0], scale[1], scale[2]);
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
        instantiatedCards[index].transform.position = new Vector3(position[0], position[1], position[2]);
        instantiatedCards[index].transform.rotation = Quaternion.Euler(90, GetYRotation(), 0);
    }
}
