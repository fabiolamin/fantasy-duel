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
            GameObject firstCardPlayedGameObject = playerManager.PlayerBoardArea.Cards[playedCardIndex];
            Card firstCardPlayed = firstCardPlayedGameObject.GetComponent<CardInfo>().Card;
            playerManager.PlayerBoardArea.ChangeLifeObject(firstCardPlayedGameObject.tag, firstCardPlayed.Id, firstCardPlayed.Type, newCard.Healing);
            playerManager.PlayerBoardArea.FortifyCard(firstCardPlayedGameObject, newCard.Fortification);
            playerManager.PlayerParticlesControl.StopCardParticles(firstCardPlayedGameObject, CardParticles.Available);
            playerManager.PlayerParticlesControl.PlayCardParticles(firstCardPlayedGameObject, CardParticles.Played);
            newCardGameObject.SetActive(false);
            playerManager.PlaySoundEffect(Clip.CardPlayed);
            playerManager.PlayerInfo.UpdateCoins(-newCard.Coins);
            playerManager.PlayerHand.RemoveCard(newCardGameObject);
        }
        else
        {
            cardInteraction.ReturnToInitialTransform();
        }
    }
}
