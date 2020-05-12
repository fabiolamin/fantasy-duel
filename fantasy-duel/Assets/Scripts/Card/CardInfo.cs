using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class CardInfo : MonoBehaviour, IDamageable, IProtectable
{
    private PhotonView photonView;
    private CardUI cardUI;

    public Card Card { get; set; }
    public bool IsProteged { get; set; }

    private void Awake()
    {
        cardUI = GetComponent<CardUI>();
    }

    public void Damage(int amount)
    {
        Card.LifePoints -= amount;
        CheckLife();
        cardUI.Set(Card);
    }

    private void CheckLife()
    {
        if(Card.LifePoints <= 0)
        {
            gameObject.SetActive(false);
        }
    }

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
