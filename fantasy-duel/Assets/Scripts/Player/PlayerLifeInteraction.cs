using UnityEngine;

public class PlayerLifeInteraction : MonoBehaviour, ISelectable, IProtectable
{
    private PlayerManager playerManager;
    public bool IsSelected { get; set; }
    public bool IsProteged { get; set; }
    private void Awake()
    {
        playerManager = transform.root.GetComponent<PlayerManager>();
        IsProteged = true;
    }

    private void OnMouseDown()
    {
        Deselect();
        Select();
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
