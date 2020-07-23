using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterInteraction : MonoBehaviour, ISelectable, IProtectable
{
    private PlayerManager playerManager;
    public bool IsSelected { get; set; }
    public bool IsProteged { get; set; }
    private void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            playerManager = transform.root.GetComponent<PlayerManager>();
            IsProteged = true;
        }
    }

    private void OnMouseDown()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Deselect();
            Select();
        }
    }

    public void Select()
    {
        playerManager.PlaySoundEffect(Clip.ObjectHit);
        IsSelected = true;
    }

    public void Deselect()
    {
        foreach (GameObject obj in playerManager.PlayerBoardArea.Objects)
        {
            obj.GetComponent<ISelectable>().IsSelected = false;
        }
    }
}
