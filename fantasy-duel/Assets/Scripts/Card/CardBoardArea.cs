using UnityEngine;
using Photon.Pun;

public class CardBoardArea : MonoBehaviour
{
    private PlayerManager playerManager;
    private Ray ray;
    private RaycastHit raycastHit;
    private GameObject playedCard;

    private void Awake()
    {
        playerManager = transform.root.GetComponent<PlayerManager>();
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
            playerManager.PlayerBoardArea.SetCard(playedCard.GetComponent<CardInfo>().Card, position);
            playerManager.PlayerInfo.UpdateCoins(-playedCard.GetComponent<CardInfo>().Card.Coins);
            playerManager.PlayerBoardArea.Add(playedCard);
            playerManager.PlayerHand.RemoveCard(playedCard);
            playerManager.PlayerHand.UpdateHand();
            playerManager.PlayerHand.Lock(true);
        }
    }

    private bool CanCardBePlayed()
    {
        return !playedCard.GetComponent<CardInteraction>().IsDragging && !playedCard.GetComponent<CardInteraction>().WasPlayed && playerManager.PlayerInfo.Coins >= playedCard.GetComponent<CardInfo>().Card.Coins;
    }
}
