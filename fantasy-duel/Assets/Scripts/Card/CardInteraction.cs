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
    public bool IsLocked { get; set; }

    private void Awake()
    {
        cardInfo = GetComponent<CardInfo>();
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        IsDragging = false;
        WasPlayed = false;
        IsLocked = false;
        initialPosition = transform.position;
        initialScale = transform.localScale;

        if (sceneIndex == 1)
        {
            playerDeck = transform.parent.gameObject.GetComponent<PlayerDeck>();
        }
    }

    private void Update()
    {
        DragAndDrop();
    }

    private void OnMouseDown()
    {
        SelectCardInDeckBuilding();
    }

    private void OnMouseOver()
    {
        EnhanceCard();
    }

    private void OnMouseDrag()
    {
        SetDragAndDrop();
    }

    private void OnMouseExit()
    {
        ReturnToInitialTransform();
        isMouseOver = false;
    }

    private void OnMouseUp()
    {
        ReturnToInitialTransform();
        IsDragging = false;
    }

    private void SelectCardInDeckBuilding()
    {
        if (sceneIndex == 0 && cardInfo.IsAvailable)
        {
            IsSelected = true;
        }
    }

    private void EnhanceCard()
    {
        if (sceneIndex == 1 && !isMouseOver && !WasPlayed)
        {
            playerDeck.RaiseCard(cardInfo.GetCard());
            playerDeck.IncreaseCardScale(cardInfo.GetCard());
            isMouseOver = true;
        }
    }

    private void SetDragAndDrop()
    {
        if (sceneIndex == 1 && !IsDragging && !WasPlayed && !IsLocked)
        {
            IsDragging = true;
            screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
        }
    }

    private void DragAndDrop()
    {
        if (IsDragging && !IsLocked)
        {
            Vector3 currentScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPosition) + offset;
            transform.position = currentPosition;
        }
    }

    private void ReturnToInitialTransform()
    {
        if (sceneIndex == 1 && !WasPlayed)
        {
            playerDeck.SetInitialTransform(cardInfo.GetCard(), initialPosition, initialScale);
        }
    }
}
