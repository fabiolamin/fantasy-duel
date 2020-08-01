using UnityEngine;

public abstract class BoardArea : MonoBehaviour
{
    protected PlayerManager playerManager;
    protected GameObject playedCardGameObject;
    protected Card playedCard;
    protected int firstCardPlayedIndex;

    protected abstract void SetCard();
    protected abstract void SetMagicCard();
    protected void Awake()
    {
        playerManager = transform.root.GetComponent<PlayerManager>();
    }
    protected void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Card"))
        {
            playedCardGameObject = other.gameObject;
            playedCardGameObject.GetComponent<CardInteraction>().IsReadyToBePlayed = true;
            playedCard = playedCardGameObject.GetComponent<CardInfo>().Card;

            SetCard();
        }
    }
    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Card"))
        {
            playedCardGameObject.GetComponent<CardInteraction>().IsReadyToBePlayed = false;
        }
    }
    protected void SetDefaultCard()
    {
        CardInteraction cardInteraction = playedCardGameObject.GetComponent<CardInteraction>();

        if (cardInteraction.CanCardBePlayed())
        {
            cardInteraction.PlayCard();
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            playerManager.PlayerBoardArea.SetCard(playedCard, position);
            playerManager.PlayerBoardArea.Add(playedCardGameObject);
            playerManager.PlayerParticlesControl.PlayCardParticles(playedCardGameObject, CardParticles.Played);
            firstCardPlayedIndex = playerManager.PlayerBoardArea.Cards.FindIndex(c => c == playedCardGameObject);
            playerManager.PlaySoundEffect(Clip.CardPlayed);
            playerManager.PlayerInfo.UpdateCoins(-playedCard.Coins);
            playerManager.PlayerHand.RemoveCard(playedCardGameObject);
        }
    }
}
