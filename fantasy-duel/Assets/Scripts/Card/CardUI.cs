using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    private Card card;
    private bool isReadyToShowNewPoints = false;
    private float timeAux = 0;

    [SerializeField] private Text cardName;
    [SerializeField] private Text description;
    [SerializeField] private Image art;
    [SerializeField] private Image icon;
    [SerializeField] private Text coins;
    [SerializeField] private Text attackPoints;
    [SerializeField] private Text lifePoints;

    [SerializeField] private Text newPoints;
    [SerializeField] private float newPointsDuration = 2f;
    [SerializeField] private GameObject life;
    [SerializeField] private GameObject attack;

    [Header("Magics Cards")]
    [SerializeField] private Color fortificationColor;
    [SerializeField] private Color healingColor;

    [Header("Sprites")]
    [SerializeField] private Sprite[] bases;
    [SerializeField] private Sprite[] creatures;
    [SerializeField] private Sprite[] magics;
    [SerializeField] private Sprite[] icons;

    private void Awake()
    {
        timeAux = newPointsDuration;
    }

    private void Update()
    {
        if (isReadyToShowNewPoints)
        {
            CheckNewPoints();
        }
    }

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
        life.SetActive(true);
        attack.SetActive(true);

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
                life.SetActive(false);
                attack.SetActive(false);
                break;
        }
    }

    private void SetDescription()
    {
        if (card.Fortification > 0)
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

    public void ShowNewPoints(int points, Color color)
    {
        newPoints.gameObject.SetActive(true);
        isReadyToShowNewPoints = true;
        newPoints.color = color;
        if (points > 0)
            newPoints.text = "+" + points;
        else
            newPoints.text = points.ToString();
    }

    private void CheckNewPoints()
    {
        newPointsDuration -= Time.deltaTime;

        if (newPointsDuration <= 0)
        {
            newPoints.gameObject.SetActive(false);
            newPointsDuration = timeAux;
            isReadyToShowNewPoints = false;
        }
    }
}
