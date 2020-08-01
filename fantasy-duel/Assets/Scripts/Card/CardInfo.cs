using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CardInfo : MonoBehaviour, IProtectable
{
    private PlayerManager playerManager;
    private PhotonView photonView;
    private CardUI cardUI;
    private CardInteraction cardInteraction;
    private CardParticlesManager particlesManager;

    public Card Card { get; set; }
    public bool IsProteged { get; set; }

    private void Awake()
    {
        cardUI = GetComponent<CardUI>();

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            playerManager = transform.root.GetComponent<PlayerManager>();
            cardInteraction = GetComponent<CardInteraction>();
            particlesManager = GetComponent<CardParticlesManager>();
        }
    }

    private void CheckLife()
    {
        if (Card.LifePoints <= 0)
        {
            particlesManager.Play(CardParticles.Destruction);
            playerManager.PlaySoundEffect(Clip.CardDestruction);
            gameObject.SetActive(false);
            playerManager.PlayerBoardArea.Remove(gameObject);
        }
    }

    public void ChangeLife(int value)
    {
        Card.LifePoints += value;
        CheckLife();
        cardUI.Set(Card);

        if (value < 0)
        {
            particlesManager.Play(CardParticles.Damage);
            playerManager.PlaySoundEffect(Clip.CardDamage);
        }
    }

    public void Fortify(int value)
    {
        Card.AttackPoints += value;
        cardUI.Set(Card);
    }
}
