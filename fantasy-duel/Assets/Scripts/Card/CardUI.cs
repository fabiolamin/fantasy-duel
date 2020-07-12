using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CardUI : MonoBehaviour
{
    private PhotonView photonView;
    private CardInfo cardInfo;

    [SerializeField] private Text name;
    [SerializeField] private Text description;
    [SerializeField] private Image art;
    [SerializeField] private Image icon;
    [SerializeField] private Text coins;
    [SerializeField] private Text attackPoints;
    [SerializeField] private Text lifePoints;
    [SerializeField] private GameObject unavailableImage;

    [Header("Sprites")]
    [SerializeField] private Sprite[] bases;
    [SerializeField] private Sprite[] creatures;
    [SerializeField] private Sprite[] magics;
    [SerializeField] private Sprite[] icons;

    private void Awake()
    {
        cardInfo = GetComponent<CardInfo>();
    }

    public void Set()
    {
        name.text = cardInfo.Card.Name;
        description.text = cardInfo.Card.Description;
        SetSprites();
        coins.text = cardInfo.Card.Coins.ToString();
        attackPoints.text = cardInfo.Card.AttackPoints.ToString();
        lifePoints.text = cardInfo.Card.LifePoints.ToString();
        unavailableImage.SetActive(!cardInfo.Card.IsAvailable);
    }

    private void SetSprites()
    {
        switch (cardInfo.Card.Type)
        {
            case "Bases":
                art.sprite = bases[cardInfo.Card.Id - 1];
                icon.sprite = icons[0];
                break;
            case "Creatures":
                art.sprite = creatures[cardInfo.Card.Id - 1];
                icon.sprite = icons[1];
                break;
            case "Magics":
                art.sprite = magics[cardInfo.Card.Id - 1];
                icon.sprite = icons[2];
                break;
        }
    }
}
