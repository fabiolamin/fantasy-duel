
public class CharacterBoardArea : BoardArea
{
    public override void SetCard()
    {
        if (newCard.Type.Equals("Magics"))
            SetMagicCard();
    }

    protected override void SetMagicCard()
    {
        if (newCard.Healing > 0)
        {
            newCardGameObject.SetActive(false);
            playerManager.PlayerInfo.ChangeLife(newCard.Healing);
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
