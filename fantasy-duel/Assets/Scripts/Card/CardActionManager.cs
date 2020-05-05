using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CardActionManager : MonoBehaviour
{
    private PhotonView photonView;
    private GameObject player;
    private GameObject opponent;
    private GameObject playerCard;
    private GameObject opponentCard;

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
        if(CanPlayerDoAnAction)
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
        playerCard = GetSelectedCard(player);
        opponentCard = GetSelectedCard(opponent);
    }

    private GameObject GetSelectedCard(GameObject player)
    {
        foreach (Transform card in player.GetComponent<PlayerDeck>().CardsParent)
        {
            if (card.gameObject.GetComponent<CardInteraction>().IsSelected)
            {
                return card.gameObject;
            }
        }

        return null;
    }

    private void UpdatePlayersCard()
    {
        player.GetComponent<PlayerDeck>().UpdateLifePoints(playerCard.GetComponent<CardInfo>().Card.Id, playerCard.GetComponent<CardInfo>().Card.Type, opponentCard.GetComponent<CardInfo>().Card.AttackPoints);
        property.Add("EnemyCardID", opponentCard.GetComponent<CardInfo>().Card.Id);
        property.Add("EnemyCardType", opponentCard.GetComponent<CardInfo>().Card.Type);
        property.Add("EnemyCardDamage", playerCard.GetComponent<CardInfo>().Card.AttackPoints);
        PhotonNetwork.PlayerListOthers[0].SetCustomProperties(property);
    }
}

