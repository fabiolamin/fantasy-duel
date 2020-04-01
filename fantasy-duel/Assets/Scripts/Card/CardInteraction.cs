using UnityEngine;

public class CardInteraction : MonoBehaviour
{
    private CardInfo cardInfo;
    public bool IsSelected { get; set; }

    private void Awake()
    {
        cardInfo = GetComponent<CardInfo>();
    }

    private void OnMouseDown()
    {
        if (cardInfo.IsAvailable)
        {
            IsSelected = true;
        }
    }
}
