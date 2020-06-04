using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CardInfo : MonoBehaviour, IDamageable, IProtectable
{
    private PlayerManager playerManager;
    private PhotonView photonView;
    private CardUI cardUI;

    public Card Card { get; set; }
    public bool IsProteged { get; set; }

    private void Awake()
    {
        cardUI = GetComponent<CardUI>();

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            playerManager = transform.root.GetComponent<PlayerManager>();
        }
    }

    public void Damage(int amount)
    {
        playerManager.PlaySoundEffect(Clip.CardDamage);
        Card.LifePoints -= amount;
        CheckLife();
        cardUI.Set(Card);
    }

    private void CheckLife()
    {
        if(Card.LifePoints <= 0)
        {
            playerManager.PlaySoundEffect(Clip.CardDestruction);
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
