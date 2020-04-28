using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class CardInteraction : MonoBehaviour
{
    private PlayerDeck playerDeck;
    private CardInfo cardInfo;
    private int sceneIndex;
    private bool isMouseOver = false;
    private Vector3 screenPosition;
    private Vector3 offset;
    private Vector3 initialPosition;
    private Vector3 initialScale;
    public bool IsSelected { get; set; }
    public bool IsDragging { get; private set; }
    public bool WasPlayed { get; set; }

    private void Awake()
    {
        cardInfo = GetComponent<CardInfo>();
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        IsDragging = false;
        WasPlayed = false;
        initialPosition = transform.position;
        initialScale = transform.localScale;

        if (sceneIndex == 1)
        {
            playerDeck = transform.parent.gameObject.GetComponent<PlayerDeck>();
        }
    }

    private void Update()
    {
        if (IsDragging)
        {
            DragAndDrop();
        }
    }

    private void OnMouseDown()
    {
        if (sceneIndex == 0)
        {
            SelectCardInDeckBuilding();
        }
    }

    private void OnMouseOver()
    {
        if (sceneIndex == 1 && !isMouseOver && !WasPlayed)
        {
            playerDeck.RaiseCard(cardInfo.GetCard());
            playerDeck.IncreaseCardScale(cardInfo.GetCard());
            isMouseOver = true;
        }
    }

    private void OnMouseDrag()
    {
        if (sceneIndex == 1 && !IsDragging && !WasPlayed)
        {
            SetDragAndDrop();
        }
    }

    private void OnMouseExit()
    {
        if (sceneIndex == 1 && !WasPlayed)
        {
            playerDeck.SetInitialTransform(cardInfo.GetCard(), initialPosition, initialScale);
        }

        isMouseOver = false;
    }

    private void OnMouseUp()
    {
        if(sceneIndex == 1 && !WasPlayed)
        {
            IsDragging = false;
            playerDeck.SetInitialTransform(cardInfo.GetCard(), initialPosition, initialScale);
        }
    }

    private void SelectCardInDeckBuilding()
    {
        if (cardInfo.IsAvailable)
        {
            IsSelected = true;
        }
    }

    private void SetDragAndDrop()
    {
        IsDragging = true;
        screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
    }

    private void DragAndDrop()
    {
        Vector3 currentScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
        Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPosition) + offset;
        transform.position = currentPosition;
    }
}
