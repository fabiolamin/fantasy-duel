using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField] private Text name;
    [SerializeField] private Text description;
    [SerializeField] private Image art;
    [SerializeField] private Image icon;
    [SerializeField] private Text coins;
    [SerializeField] private Text attackPoints;
    [SerializeField] private Text lifePoints;

    public void Set(Card card)
    {
        name.text = card.Name;
        description.text = card.Description;
        Sprite artSprite = Resources.Load<Sprite>("Sprites/Cards/" + card.Type + "/" + card.Id);
        art.sprite = artSprite;
        Sprite iconSprite = Resources.Load<Sprite>("Sprites/Cards/Icons/" + card.Type);
        icon.sprite = iconSprite;
        coins.text = card.Coins.ToString();
        attackPoints.text = card.AttackPoints.ToString();
        lifePoints.text = card.LifePoints.ToString();
    }
}
