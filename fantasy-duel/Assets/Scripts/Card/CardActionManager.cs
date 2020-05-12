using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CardActionManager : MonoBehaviour
{
    private PhotonView photonView;
    private GameObject player;
    private GameObject opponent;
    private GameObject playerCard;
    private GameObject opponentObject;

    private int playerCardIndex;
    private int opponentCardIndex;
    private ExitGames.Client.Photon.Hashtable property = new ExitGames.Client.Photon.Hashtable();

    public bool CanPlayerDoAnAction { get; set; }

    private void Awake()
    {
        CanPlayerDoAnAction = false;
        photonView = transform.root.gameObject.GetComponent<PhotonView>();
    }

    public void StartAction()
    {
        if (CanPlayerDoAnAction)
        {
            GetPlayers();
            GetPlayersCard();
            UpdatePlayersCard();
            CanPlayerDoAnAction = false;
        }
    }

    private void GetPlayers()
    {
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
        {
            opponent = p;

            if (p.GetComponent<PhotonView>().ViewID == photonView.ViewID)
            {
                player = p;
            }
        }
    }

    private void GetPlayersCard()
    {
        playerCard = GetPlayerSelectedCard();
        opponentObject = GetOpponentSelectedObject();
    }

    private GameObject GetPlayerSelectedCard()
    {
        foreach (GameObject card in player.GetComponent<PlayerDeck>().PlayedCards)
        {
            if (card.GetComponent<CardInteraction>().IsSelected)
            {
                if (card.GetComponent<CardInfo>().Card.Type == "Creatures")
                {
                    if (card.GetComponent<CardInteraction>().CanDoAnAction())
                    {
                        return card.gameObject;
                    }
                }
            }
        }

        return null;
    }

    private GameObject GetOpponentSelectedObject()
    {
        foreach (GameObject playerObject in opponent.GetComponent<PlayerDeck>().SelectableObjects)
        {
            if (playerObject.GetComponent<ISelectable>().IsSelected)
            {
                if (playerObject.GetComponent<IProtectable>().IsProteged)
                {
                    if (!HasOpponentPlayedABaseCard())
                    {
                        return playerObject.gameObject;
                    }
                }
                else
                {
                    return playerObject.gameObject;
                }

            }
        }
        return null;
    }

    private bool HasOpponentPlayedABaseCard()
    {
        foreach (GameObject card in opponent.GetComponent<PlayerDeck>().PlayedCards)
        {
            if (card.GetComponent<CardInfo>().Card.Type == "Bases")
            {
                return true;
            }
        }

        return false;
    }

    private void UpdatePlayersCard()
    {
        if (playerCard != null &&  opponentObject != null)
        {
            if(opponentObject.CompareTag("Card"))
            {
                player.GetComponent<PlayerDeck>().UpdateLifePoints(playerCard.tag, playerCard.GetComponent<CardInfo>().Card.Id, playerCard.GetComponent<CardInfo>().Card.Type, opponentObject.GetComponent<CardInfo>().Card.AttackPoints);
                property.Add("CardID", opponentObject.GetComponent<CardInfo>().Card.Id);
                property.Add("CardType", opponentObject.GetComponent<CardInfo>().Card.Type);
            }
            else
            {
                player.GetComponent<PlayerDeck>().UpdateLifePoints(playerCard.tag, playerCard.GetComponent<CardInfo>().Card.Id, playerCard.GetComponent<CardInfo>().Card.Type, 0);
                property.Add("CardID", 0);
                property.Add("CardType", "");
            }
            
            property.Add("Tag", opponentObject.tag);
            property.Add("EnemyCardDamage", playerCard.GetComponent<CardInfo>().Card.AttackPoints);
            PhotonNetwork.PlayerListOthers[0].SetCustomProperties(property);
            property.Clear();
        }
    }
}