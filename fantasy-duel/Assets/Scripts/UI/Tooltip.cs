using UnityEngine.EventSystems;
using UnityEngine;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private PlayerManager playerManager;

    private void Awake()
    {
        playerManager = transform.root.GetComponent<PlayerManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ActiveTooltip(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ActiveTooltip(false);
    }

    private void ActiveTooltip(bool isActivated)
    {
        if (playerManager.PhotonView.IsMine)
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.CompareTag("Tooltip"))
                    child.gameObject.SetActive(isActivated);
            }
        }
    }
}
