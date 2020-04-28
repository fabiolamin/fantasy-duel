using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class CardBoardArea : MonoBehaviour
{
    private PlayerDeck playerDeck;
    private bool HasACard = false;
    private Ray ray;
    private RaycastHit raycastHit;
    private GameObject card;

    private void Awake()
    {
        playerDeck = transform.root.gameObject.GetComponent<PlayerDeck>();
        ray = new Ray(transform.position, Vector3.up);
    }

    private void Update()
    {
        if(!HasACard)
        {
            VerifyRaycast();
        }
    }

    private void VerifyRaycast()
    {
        if (Physics.Raycast(ray.origin, ray.direction * 25f, out raycastHit))
        {
            if (raycastHit.collider.gameObject.CompareTag("Card"))
            {
                card = raycastHit.collider.gameObject;
                SetCardInBoardArea();
            }
        }
    }

    private void SetCardInBoardArea()
    {
        if (!card.GetComponent<CardInteraction>().IsDragging && !card.GetComponent<CardInteraction>().WasPlayed)
        {
            card.GetComponent<CardInteraction>().WasPlayed = true;
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            playerDeck.SetCardInBoardArea(card.GetComponent<CardInfo>().GetCard(), position);
            HasACard = true;
        }
    }
}
