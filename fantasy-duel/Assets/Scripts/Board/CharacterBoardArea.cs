
public class CharacterBoardArea : BoardArea
{
    protected override void SetCard()
    {
        if (newCard.Type.Equals("Magics"))
            SetMagicCard();
    }

    protected override void SetMagicCard()
    {
        CardInteraction cardInteraction = newCardGameObject.GetComponent<CardInteraction>();
        if (cardInteraction.CanCardBePlayed())
        {
            newCardGameObject.SetActive(false);
            playerManager.PlayerInfo.ChangeLife(newCard.Healing);
            playerManager.PlaySoundEffect(Clip.CardPlayed);
            playerManager.PlayerInfo.UpdateCoins(-newCard.Coins);
            playerManager.PlayerHand.RemoveCard(newCardGameObject);
        }
    }
}
