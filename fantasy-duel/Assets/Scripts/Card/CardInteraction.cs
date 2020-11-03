using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class CardInteraction : MonoBehaviour, ISelectable
{
    private PlayerManager playerManager;
    private CardInfo cardInfo;
    private CardParticlesManager particlesManager;
    private int sceneIndex;
    private bool isMouseOver = false;
    private Vector3 screenPosition;
    private Vector3 offset;
    private Vector3 initialPosition;
    private Vector3 initialScale;
    public bool IsDragging { get; private set; } = false;
    public bool WasPlayed { get; set; } = false;
    public bool IsReadyToBePlayed { get; set; } = false;
    public bool IsLocked { get; set; } = false;
    public int TurnWhenWasPlayed { get; set; }
    public bool IsSelected { get; set; }

    public BoardArea BoardArea { get; set; }

    private void Awake()
    {
        cardInfo = GetComponent<CardInfo>();
        particlesManager = GetComponent<CardParticlesManager>();

        sceneIndex = SceneManager.GetActiveScene().buildIndex;
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
        IsDragging = false;

        if(CanCardBePlayed())
        {
            BoardArea.SetCard();
            playerManager.PlayerHUD.HighlightCoins();
        }
        else
        {
            ReturnToInitialTransform();
        }
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
            DeselectInMatch();
        }
    }

    private void DeselectInMatch()
    {
        foreach (GameObject obj in playerManager.PlayerBoardArea.Objects)
        {
            obj.GetComponent<ISelectable>().IsSelected = false;

            if (obj.CompareTag("Card"))
            {
                playerManager.PlayerParticlesControl.StopCardParticles(obj, CardParticles.MatchSelection);

                if (!playerManager.PlayerTurn.IsMyTurn && !playerManager.PhotonView.IsMine)
                {
                    particlesManager.Stop(CardParticles.MatchSelection);
                    playerManager.PlayerParticlesControl.StopOpponentCardParticles(obj, CardParticles.OpponentSelection);
                }
            }
            else
                playerManager.PlayerParticlesControl.StopOpponentCharacterParticles();
        }
    }

    public void Select()
    {
        if (sceneIndex == 0)
        {
            IsSelected = true;
            AudioManager.Instance.Play(Audio.SoundEffects, Clip.CardSelect, false);
            particlesManager.Play(CardParticles.MenuSelection);
        }
        else
        {
            SelectCardInMatch();
        }
    }

    private void SelectCardInMatch()
    {
        if (WasPlayed && CanDoAnAction() && playerManager.PlayerTurn.IsMyTurn && cardInfo.Card.Type == "Creatures")
        {
            particlesManager.Stop(CardParticles.Available);
            playerManager.PlayerParticlesControl.PlayCardParticles(gameObject, CardParticles.MatchSelection);
        }
        else if (!playerManager.PlayerTurn.IsMyTurn && !playerManager.PhotonView.IsMine)
        {
            particlesManager.Stop(CardParticles.Target);
            playerManager.PlayerParticlesControl.PlayOpponentCardParticles(gameObject, CardParticles.OpponentSelection);
        }

        playerManager.PlaySoundEffect(Clip.ObjectHit);
        IsSelected = true;
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
        if (sceneIndex == 1 && !IsDragging && !WasPlayed && !IsLocked && playerManager.PhotonView.IsMine)
        {
            IsDragging = true;
            particlesManager.Play(CardParticles.MatchSelection);
            playerManager.PlaySoundEffect(Clip.CardDrag);
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

    public void CheckCardAvailability()
    {
        if (CanDoAnAction() && WasPlayed && cardInfo.Card.Type == "Creatures")
        {
            particlesManager.Play(CardParticles.Available);
        }
    }

    public void ReturnToInitialTransform()
    {
        if (sceneIndex == 1 && !WasPlayed)
        {
            playerManager.PlayerCardMovement.SetInitialTransform(cardInfo.Card);
            particlesManager.Stop(CardParticles.MatchSelection);
        }
    }

    public bool CanDoAnAction()
    {
        int currentTurn = (int)PhotonNetwork.CurrentRoom.CustomProperties["TurnNumber"];
        return (TurnWhenWasPlayed + 1) <= currentTurn;
    }
    public bool CanCardBePlayed()
    {
        return IsReadyToBePlayed && !IsDragging && !WasPlayed &&
        playerManager.PlayerInfo.Coins >= cardInfo.Card.Coins;
    }
    public void PlayCard()
    {
        WasPlayed = true;
        TurnWhenWasPlayed = (int)PhotonNetwork.CurrentRoom.CustomProperties["TurnNumber"];
        IsSelected = false;
    }
}
