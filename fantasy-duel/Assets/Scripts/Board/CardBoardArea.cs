using UnityEngine;
public class CardBoardArea : BoardArea
{
    protected override void SetCard()
    {
        if (playedCard.Type.Equals("Magics"))
            SetMagicCard();
        else
            SetDefaultCard();
    }

    protected override void SetMagicCard()
    {
        CardInteraction cardInteraction = playedCardGameObject.GetComponent<CardInteraction>();

        if (cardInteraction.CanCardBePlayed())
        {
            GameObject firstCardPlayedGameObject = playerManager.PlayerBoardArea.Cards[firstCardPlayedIndex];
            Card firstCardPlayed = firstCardPlayedGameObject.GetComponent<CardInfo>().Card;
            playerManager.PlayerBoardArea.ChangeLifeObject(firstCardPlayedGameObject.tag, firstCardPlayed.Id, firstCardPlayed.Type, playedCard.Healing);
            playerManager.PlayerBoardArea.FortifyCard(firstCardPlayedGameObject, playedCard.Fortification);
            playerManager.PlayerParticlesControl.StopCardParticles(firstCardPlayedGameObject, CardParticles.Available);
            playerManager.PlayerParticlesControl.PlayCardParticles(firstCardPlayedGameObject, CardParticles.Played);
            playedCardGameObject.SetActive(false);
            playerManager.PlaySoundEffect(Clip.CardPlayed);
            playerManager.PlayerInfo.UpdateCoins(-playedCard.Coins);
            playerManager.PlayerHand.RemoveCard(playedCardGameObject);
        }
    }
}
