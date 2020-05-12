using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Photon.Pun;

public class CardBoardArea : MonoBehaviour
{
    private PlayerDeck playerDeck;
    private PlayerInfo playerInfo;
    private Ray ray;
    private RaycastHit raycastHit;
    private GameObject playedCard;

    private void Awake()
    {
        playerDeck = transform.root.gameObject.GetComponent<PlayerDeck>();
        playerInfo = transform.root.gameObject.GetComponent<PlayerInfo>();
        ray = new Ray(transform.position, Vector3.up);
    }

    private void Update()
    {
        VerifyRaycast();
    }

    private void VerifyRaycast()
    {
        if (Physics.Raycast(ray.origin, ray.direction * 25f, out raycastHit))
        {
            if (raycastHit.collider.gameObject.CompareTag("Card"))
            {
                playedCard = raycastHit.collider.gameObject;
                SetCardInBoardArea();
            }
        }
    }

    private void SetCardInBoardArea()
    {
        if (CanCardBePlayed())
        {
            playedCard.GetComponent<CardInteraction>().WasPlayed = true;
            playedCard.GetComponent<CardInteraction>().TurnWhenWasPlayed = (int)PhotonNetwork.CurrentRoom.CustomProperties["TurnNumber"];
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            playerDeck.SetCardInBoardArea(playedCard.GetComponent<CardInfo>().Card, position);
            playerInfo.UpdateCoins(-playedCard.GetComponent<CardInfo>().Card.Coins);
            playerDeck.AddCardToPlayedCards(playedCard);
            playerDeck.RemoveCardFromHandCards(playedCard);
            playerDeck.UpdateHandCards();
            playerDeck.UpdateCardsLock(true);
        }
    }

    private bool CanCardBePlayed()
    {
        return !playedCard.GetComponent<CardInteraction>().IsDragging && !playedCard.GetComponent<CardInteraction>().WasPlayed && playerInfo.Coins >= playedCard.GetComponent<CardInfo>().Card.Coins;
    }
}
