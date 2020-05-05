using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class CardInfo : MonoBehaviour
{
    private PhotonView photonView;
    public Card Card { get; set; }

    public Card GetAvailableCard()
    {
        Card card = new Card();
        card.Id = Card.Id;
        card.Name = Card.Name;
        card.Description = Card.Description;
        card.Type = Card.Type;
        card.Coins = Card.Coins;
        card.AttackPoints = Card.AttackPoints;
        card.LifePoints = Card.LifePoints;
        card.IsAvailable = true;

        return card;
    }
}
