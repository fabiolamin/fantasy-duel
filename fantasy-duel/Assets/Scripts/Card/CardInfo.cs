using UnityEngine;

public class CardInfo : MonoBehaviour
{
    public int Id { get; set;}
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public int Coins { get; set; }
    public int AttackPoints { get; set; }
    public int LifePoints { get; set; }
    public bool IsAvailable { get; set; }

    public void Set(Card card)
    {
        Id = card.Id;
        Name = card.Name;
        Description = card.Description;
        Type = card.Type;
        Coins = card.Coins;
        AttackPoints = card.AttackPoints;
        LifePoints = card.LifePoints;
        IsAvailable = card.IsAvailable;
    }

    public Card GetCard()
    {
        Card card = new Card();
        card.Id = Id;
        card.Name = Name;
        card.Description = Description;
        card.Type = Type;
        card.Coins = Coins;
        card.AttackPoints = AttackPoints;
        card.LifePoints = LifePoints;
        card.IsAvailable = IsAvailable;

        return card;
    }
}
