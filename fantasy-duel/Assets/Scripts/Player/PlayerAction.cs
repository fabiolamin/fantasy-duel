using UnityEngine;
using Photon.Pun;

public class PlayerAction : MonoBehaviour
{
    private PlayerManager playerManager;
    private GameObject[] players;
    private GameObject player;
    private GameObject opponent;
    private GameObject playerObject;
    private GameObject opponentObject;

    public bool CanPlayerDoAnAction { get; set; }

    private void Awake()
    {
        CanPlayerDoAnAction = false;
        playerManager = GetComponent<PlayerManager>();
    }

    public void PrepareForAction()
    {
        GetPlayers();
        GetSelections();

        if (CanPlayerDoAnAction && playerObject != null && opponentObject != null)
        {
            StartAction();
            ResetVariables();
        }
    }

    private void StartAction()
    {
        UpdatePlayersObject();
        playerManager.PlayerInfo.PlayCharacterAnimation(CharacterAnimations.Attack);
    }

    private void ResetVariables()
    {
        CanPlayerDoAnAction = false;
        playerObject.GetComponent<ISelectable>().IsSelected = false;
        opponentObject.GetComponent<ISelectable>().IsSelected = false;
        playerObject = null;
    }

    private void GetPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length == 2)
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
    }

    private void GetSelections()
    {
        playerObject = GetPlayerSelectedObject();
        opponentObject = GetOpponentSelectedObject();
    }

    private GameObject GetPlayerSelectedObject()
    {
        foreach (GameObject card in player.GetComponent<PlayerManager>().PlayerBoardArea.Cards)
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
        foreach (GameObject card in opponent.GetComponent<PlayerManager>().PlayerBoardArea.Cards)
        {
            if (card.GetComponent<CardInfo>().Card.Type == "Bases")
            {
                return true;
            }
        }

        return false;
    }

    private void UpdatePlayersObject()
    {
        if (playerObject != null && opponentObject != null)
        {
            ExitGames.Client.Photon.Hashtable property = new ExitGames.Client.Photon.Hashtable();

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
            property.Add("IsReadyToUpdateObject", true);
            PhotonNetwork.PlayerListOthers[0].SetCustomProperties(property);
        }
    }

    public void ShowAvailableOpponentCardsToAttack()
    {
        GetPlayers();
        bool hasABaseCard = false;
        var opponentPlayerManager = opponent.GetComponent<PlayerManager>();
        if (opponentPlayerManager.PlayerBoardArea.Objects.Count > 0)
        {
            foreach (GameObject card in opponentPlayerManager.PlayerBoardArea.Cards)
            {
                if (card.GetComponent<CardInfo>().Card.Type == "Bases")
                {
                    hasABaseCard = true;
                    card.GetComponent<CardParticlesManager>().Play(CardParticles.Target);
                }
            }

            if (!hasABaseCard)
            {
                foreach (GameObject card in opponentPlayerManager.PlayerBoardArea.Cards)
                {
                    card.GetComponent<CardParticlesManager>().Play(CardParticles.Target);
                }

                opponentPlayerManager.PlayerInfo.Character.PlayParticles(CharacterParticles.Target);
            }
        }
    }

    public void HideAvailableOpponentCardsToAttack()
    {
        GetPlayers();

        if (players.Length == 2)
        {
            var opponentPlayerManager = opponent.GetComponent<PlayerManager>();
            opponentPlayerManager.PlayerBoardArea.Cards.ForEach(card => card.GetComponent<CardParticlesManager>().Stop(CardParticles.Target));
            opponentPlayerManager.PlayerInfo.Character.StopParticles(CharacterParticles.Target);
        }
            
    }
}