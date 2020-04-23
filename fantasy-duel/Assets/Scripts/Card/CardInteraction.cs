using UnityEngine;
using UnityEngine.SceneManagement;

public class CardInteraction : MonoBehaviour
{
    private CardInfo cardInfo;
    private int sceneIndex;
    private Vector3 initialPosition;
    private Vector3 initialScale;
    private bool isMouseOver = false;

    [Header("When mouse is over")]
    [SerializeField] private float scale = 1f;
    [SerializeField] private float height = 1.4f;

    public bool IsSelected { get; set; }

    private void Awake()
    {
        initialPosition = transform.position;
        initialScale = transform.localScale;
        cardInfo = GetComponent<CardInfo>();
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnMouseDown()
    {
        switch (sceneIndex)
        {
            case 0:
                SelectCardInDeckBuilding();
                break;

            case 1:
                SelectCardInMatch();
                break;
        }
    }

    private void OnMouseOver()
    {
        if(!isMouseOver)
        {
            RaiseCard();
            IncreaseCardScale();
            isMouseOver = true;
        }
    }

    private void OnMouseExit()
    {
        SetInitialTransform();
        isMouseOver = false;
    }

    private void SelectCardInDeckBuilding()
    {
        if (cardInfo.IsAvailable)
        {
            IsSelected = true;
        }
    }

    private void SelectCardInMatch()
    {
        //ToDo
    }

    private void RaiseCard()
    {
        if (sceneIndex == 1)
        {
            transform.position += Vector3.up * height;
        }
    }

    private void IncreaseCardScale()
    {
        if (sceneIndex == 1)
        {
            transform.localScale += new Vector3(scale, scale, 0);
        }
    }

    private void SetInitialTransform()
    {
        if (sceneIndex == 1)
        {
            transform.position = initialPosition;
            transform.localScale = initialScale;
        }
    }
}
