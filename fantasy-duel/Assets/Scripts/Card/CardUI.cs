using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CardUI : MonoBehaviour
{
    private PhotonView photonView;

    [SerializeField] private Text name;
    [SerializeField] private Text description;
    [SerializeField] private Image art;
    [SerializeField] private Image icon;
    [SerializeField] private Text coins;
    [SerializeField] private Text attackPoints;
    [SerializeField] private Text lifePoints;

    [Header("Sprites")]
    [SerializeField] private Sprite[] bases;
    [SerializeField] private Sprite[] creatures;
    [SerializeField] private Sprite[] magics;
    [SerializeField] private Sprite[] icons;

    public void Set(Card card)
    {
        name.text = card.Name;
        description.text = card.Description;
        SetSprites(card);
        coins.text = card.Coins.ToString();
        attackPoints.text = card.AttackPoints.ToString();
        lifePoints.text = card.LifePoints.ToString();
    }

    private void SetSprites(Card card)
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
}
