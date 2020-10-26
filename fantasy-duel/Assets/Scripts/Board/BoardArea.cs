using UnityEngine;

public abstract class BoardArea : MonoBehaviour
{
    protected PlayerManager playerManager;
    protected GameObject newCardGameObject;
    protected Card newCard;
    protected GameObject playedCard;
    protected int playedCardIndex;

    protected abstract void SetCard();
    protected abstract void SetMagicCard();
    protected void Awake()
    {
        playerManager = transform.root.GetComponent<PlayerManager>();
        playedCard = new GameObject();
        playedCard.SetActive(false);
    }
    protected void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Card"))
        {
            newCardGameObject = other.gameObject;
            newCardGameObject.GetComponent<CardInteraction>().IsReadyToBePlayed = true;
            newCard = newCardGameObject.GetComponent<CardInfo>().Card;

            SetCard();
        }
    }
    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Card"))
        {
            newCardGameObject.GetComponent<CardInteraction>().IsReadyToBePlayed = false;
        }
    }
    protected void SetDefaultCard()
    {
        CardInteraction cardInteraction = newCardGameObject.GetComponent<CardInteraction>();

        if (cardInteraction.CanCardBePlayed() && !playedCard.activeSelf)
        {
            playedCard = newCardGameObject;
            cardInteraction.PlayCard();
            Vector3 position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            playerManager.PlayerBoardArea.SetCard(newCard, position);
            playerManager.PlayerBoardArea.Add(newCardGameObject);
            playerManager.PlayerParticlesControl.StopCardParticles(newCardGameObject, CardParticles.MatchSelection);
            playerManager.PlayerParticlesControl.PlayCardParticles(newCardGameObject, CardParticles.Played);
            playedCardIndex = playerManager.PlayerBoardArea.Cards.FindIndex(c => c == newCardGameObject);
            playerManager.PlaySoundEffect(Clip.CardPlayed);
            playerManager.PlayerInfo.UpdateCoins(-newCard.Coins);
            playerManager.PlayerHand.RemoveCard(newCardGameObject);
        }
    }
}
