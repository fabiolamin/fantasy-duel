using UnityEngine;
using Photon.Pun;

public class PlayerAction : MonoBehaviour
{
    private PlayerManager playerManager;
    private GameObject player;
    private GameObject opponent;
    private GameObject playerObject;
    private GameObject opponentObject;
    private ExitGames.Client.Photon.Hashtable property = new ExitGames.Client.Photon.Hashtable();

    public bool CanPlayerDoAnAction { get; set; }

    private void Awake()
    {
        CanPlayerDoAnAction = false;
        playerManager = GetComponent<PlayerManager>();
    }

    public void StartAction()
    {
        if (CanPlayerDoAnAction)
        {
            GetPlayers();
            GetSelections();
            UpdatePlayersObject();
            CanPlayerDoAnAction = false;
        }
    }

    private void GetPlayers()
    {
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
        {
            opponent = p;

            if (p.GetComponent<PhotonView>().ViewID == playerManager.PhotonView.ViewID)
            {
                player = p;
            }
        }
    }

    private void GetSelections()
    {
        playerObject = GetPlayerSelectedObject();
        opponentObject = GetOpponentSelectedObject();
    }

    private GameObject GetPlayerSelectedObject()
    {
        foreach (GameObject card in player.GetComponent<PlayerManager>().PlayerBoardArea.Objects)
        {
            if (card.GetComponent<ISelectable>().IsSelected)
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
        foreach (GameObject playerObject in opponent.GetComponent<PlayerManager>().PlayerBoardArea.Objects)
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
        foreach (GameObject card in opponent.GetComponent<PlayerManager>().PlayerBoardArea.Objects)
        {
            if (card.CompareTag("Card"))
            {
                if (card.GetComponent<CardInfo>().Card.Type == "Bases")
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void UpdatePlayersObject()
    {
        if (playerObject != null && opponentObject != null)
        {
            PlayerBoardArea playerBoardArea = player.GetComponent<PlayerManager>().PlayerBoardArea;
            Card playerCard = playerObject.GetComponent<CardInfo>().Card;

            if (opponentObject.CompareTag("Card"))
            {
                Card opponentCard = opponentObject.GetComponent<CardInfo>().Card;
                playerBoardArea.DamageObject(playerObject.tag, playerCard.Id, playerCard.Type, opponentCard.AttackPoints);
                property.Add("TargetCardID", opponentCard.Id);
                property.Add("TargetCardType", opponentCard.Type);
            }

            property.Add("TargetTag", opponentObject.tag);
            property.Add("CardAttack", playerCard.AttackPoints);
            PhotonNetwork.PlayerListOthers[0].SetCustomProperties(property);
            property.Clear();
        }
    }
}