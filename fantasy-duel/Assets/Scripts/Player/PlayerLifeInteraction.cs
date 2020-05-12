using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifeInteraction : MonoBehaviour, ISelectable, IProtectable
{
    public bool IsSelected { get; set; }
    public bool IsProteged { get; set; }

    [SerializeField] private PlayerDeck playerDeck;

    private void Awake()
    {
        IsProteged = true;
    }

    private void OnMouseDown()
    {
        Deselect();
        Select();
    }

    public void Select()
    {
        IsSelected = true;
    }

    public void Deselect()
    {
        foreach (GameObject obj in playerDeck.SelectableObjects)
        {
            obj.GetComponent<ISelectable>().IsSelected = false;
        }
    }
}
