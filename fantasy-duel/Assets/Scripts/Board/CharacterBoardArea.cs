
public class CharacterBoardArea : BoardArea
{
    protected override void SetCard()
    {
        if (playedCard.Type.Equals("Magics"))
            SetMagicCard();
    }

    protected override void SetMagicCard()
    {
        CardInteraction cardInteraction = playedCardGameObject.GetComponent<CardInteraction>();
        if (cardInteraction.CanCardBePlayed())
        {
            playedCardGameObject.SetActive(false);
            playerManager.PlayerInfo.ChangeLife(playedCard.Healing);
            playerManager.PlaySoundEffect(Clip.CardPlayed);
            playerManager.PlayerInfo.UpdateCoins(-playedCard.Coins);
            playerManager.PlayerHand.RemoveCard(playedCardGameObject);
        }
    }
}
