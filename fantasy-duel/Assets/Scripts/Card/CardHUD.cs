using UnityEngine;
using UnityEngine.UI;

public class CardHUD : MonoBehaviour
{
    [SerializeField] private Text name;
    [SerializeField] private Text description;
    [SerializeField] private Image art;
    [SerializeField] private Image icon;
    [SerializeField] private Text coins;
    [SerializeField] private Text attackPoints;
    [SerializeField] private Text lifePoints;

    public void Set(Cards cards)
    {
        name.text = cards.Name;
        description.text = cards.Description;
        Sprite artSprite = Resources.Load<Sprite>("Sprites/Cards/" + cards.Type + "/" + cards.Id);
        art.sprite = artSprite;
        Sprite iconSprite = Resources.Load<Sprite>("Sprites/Cards/Icons/" + cards.Type);
        icon.sprite = iconSprite;
        coins.text = cards.Coins.ToString();
        attackPoints.text = cards.AttackPoints.ToString();
        lifePoints.text = cards.LifePoints.ToString();
    }
}
