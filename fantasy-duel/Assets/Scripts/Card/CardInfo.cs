using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CardInfo : MonoBehaviour, IProtectable
{
    private PlayerManager playerManager;
    private PhotonView photonView;
    private CardUI cardUI;
    private CardParticlesManager particlesManager;

    public Card Card { get; set; }
    public bool IsProteged { get; set; }

    private void Awake()
    {
        cardUI = GetComponent<CardUI>();

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            playerManager = transform.root.GetComponent<PlayerManager>();
            particlesManager = GetComponent<CardParticlesManager>();
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

    private void CheckLife()
    {
        if (Card.LifePoints <= 0)
        {
            particlesManager.Play(CardParticles.Destruction);
            playerManager.PlaySoundEffect(Clip.CardDestruction);
            playerManager.PlayerBoardArea.Remove(gameObject);
            Invoke("Disable", 0.7f);
        }
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Fortify(int value)
    {
        Card.AttackPoints += value;
        cardUI.Set(Card);
    }
}
