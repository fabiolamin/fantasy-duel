using UnityEngine;
public class CardBoardArea : BoardArea
{
    public override void SetCard()
    {
        if (newCard.Type.Equals("Magics"))
            SetMagicCard();
        else
            SetDefaultCard();
    }

    protected override void SetMagicCard()
    {
        if (playedCard.activeSelf)
        {
            Card playedCardInfo = playedCard.GetComponent<CardInfo>().Card;
            playerManager.PlayerBoardArea.ChangeLifeObject(playedCard.tag, playedCardInfo.Id, playedCardInfo.Type, newCard.Healing);
            playerManager.PlayerBoardArea.FortifyCard(playedCard, newCard.Fortification);
            playerManager.PlayerParticlesControl.PlayCardParticles(playedCard, CardParticles.Played);
            newCardGameObject.SetActive(false);
            playerManager.PlaySoundEffect(Clip.CardPlayed);
            playerManager.PlayerInfo.UpdateCoins(-newCard.Coins);
            playerManager.PlayerHand.RemoveCard(newCardGameObject);
        }
        else
        {
            cardInteraction.ReturnToInitialTransform();
        }

        playerManager.PlayerHUD.HighlightCoins();
    }
}
