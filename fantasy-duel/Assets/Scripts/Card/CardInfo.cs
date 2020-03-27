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

    public void Set(Card cards)
    {
        Id = cards.Id;
        Name = cards.Name;
        Description = cards.Description;
        Type = cards.Type;
        Coins = cards.Coins;
        AttackPoints = cards.AttackPoints;
        LifePoints = cards.LifePoints;
    }
}
