using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class CardInteraction : MonoBehaviour, ISelectable
{
    private PlayerManager playerManager;
    private CardInfo cardInfo;
    private int sceneIndex;
    private bool isMouseOver = false;
    private Vector3 screenPosition;
    private Vector3 offset;
    private Vector3 initialPosition;
    private Vector3 initialScale;
    public bool IsDragging { get; private set; }
    public bool WasPlayed { get; set; }
    public bool IsLocked { get; set; }
    public int TurnWhenWasPlayed { get; set; }
    public bool IsSelected { get; set; }

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
            playerManager = transform.root.GetComponent<PlayerManager>();
        }
    }

    private void Update()
    {
        DragAndDrop();
    }

    private void OnMouseDown()
    {
        Deselect();
        Select();
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

    public void Deselect()
    {
        if (sceneIndex == 0)
        {
            foreach (Transform card in transform.parent)
            {
                card.GetComponent<ISelectable>().IsSelected = false;
            }
        }
        else
        {
            foreach (GameObject obj in playerManager.PlayerBoardArea.Objects)
            {
                obj.GetComponent<ISelectable>().IsSelected = false;
            }
        }
    }

    public void Select()
    {
        if (sceneIndex == 0)
        {
            if (cardInfo.Card.IsAvailable)
            {
                IsSelected = true;
                AudioManager.Instance.Play(Audio.SoundEffects, Clip.CardSelect, false);
            }
        }
        else
        {
            playerManager.PlaySoundEffect(Clip.ObjectHit);
            IsSelected = true;
        }
    }

    private void EnhanceCard()
    {
        if (sceneIndex == 1 && !isMouseOver && !WasPlayed)
        {
            playerManager.PlaySoundEffect(Clip.CardSelect);
            playerManager.PlayerCardMovement.RaiseCard(cardInfo.Card);
            playerManager.PlayerCardMovement.IncreaseCardScale(cardInfo.Card);
            isMouseOver = true;
        }
    }

    private void SetDragAndDrop()
    {
        if (sceneIndex == 1 && !IsDragging && !WasPlayed && !IsLocked)
        {
            playerManager.PlaySoundEffect(Clip.CardDrag);
            IsDragging = true;
            screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));
        }
    }

    private void DragAndDrop()
    {
        if (IsDragging && !IsLocked && playerManager.PhotonView.IsMine)
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
            playerManager.PlayerCardMovement.SetInitialTransform(cardInfo.Card);
        }
    }

    public bool CanDoAnAction()
    {
        int currentTurn = (int)PhotonNetwork.CurrentRoom.CustomProperties["TurnNumber"];
        return (TurnWhenWasPlayed + 2) <= currentTurn;
    }
}
