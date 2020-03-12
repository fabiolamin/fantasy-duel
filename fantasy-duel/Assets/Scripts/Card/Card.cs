using System;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private int id;

    [Header("Variables")]
    [SerializeField] private int coins;
    [SerializeField] private int attackPoints;
    [SerializeField] private int lifePoints;

    [Header("HUD")]
    [SerializeField] private Text coinsText;
    [SerializeField] private Text attackPointsText;
    [SerializeField] private Text lifePointsText;

    private void Awake()
    {
        SetVariables();
    }

    private void SetVariables()
    {
        if(attackPoints == 0 && lifePoints == 0)
        {
            attackPointsText.text = "-";
            lifePointsText.text = "-";
        }

        coinsText.text = coins.ToString();
        attackPointsText.text = attackPoints.ToString();
        lifePointsText.text = lifePoints.ToString();
    }
}
