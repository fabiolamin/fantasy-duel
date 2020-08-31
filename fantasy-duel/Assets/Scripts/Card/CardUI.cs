using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    private Card card;

    [SerializeField] private Text cardName;
    [SerializeField] private Text description;
    [SerializeField] private Image art;
    [SerializeField] private Image icon;
    [SerializeField] private Text coins;
    [SerializeField] private Text attackPoints;
    [SerializeField] private Text lifePoints;

    [Header("Magics Cards")]
    [SerializeField] private Color fortificationColor;
    [SerializeField] private Color healingColor;

    [Header("Sprites")]
    [SerializeField] private Sprite[] bases;
    [SerializeField] private Sprite[] creatures;
    [SerializeField] private Sprite[] magics;
    [SerializeField] private Sprite[] icons;

    public void Set(Card newCard)
    {
        card = newCard;
        cardName.text = card.Name;
        description.text = card.Description;
        SetSprites();
        SetDescription();
        coins.text = card.Coins.ToString();
        attackPoints.text = card.AttackPoints.ToString();
        lifePoints.text = card.LifePoints.ToString();
    }

    private void SetSprites()
    {
        switch (card.Type)
        {
            case "Bases":
                art.sprite = bases[card.Id - 1];
                icon.sprite = icons[0];
                break;
            case "Creatures":
                art.sprite = creatures[card.Id - 1];
                icon.sprite = icons[1];
                break;
            case "Magics":
                art.sprite = magics[card.Id - 1];
                icon.sprite = icons[2];
                break;
        }
    }

    private void SetDescription()
    {
        if(card.Fortification > 0)
        {
            description.color = fortificationColor;
            description.text = "+" + card.Fortification;
        }
        else if (card.Healing > 0)
        {
            description.color = healingColor;
            description.text = "+" + card.Healing;
        }
    }
}
