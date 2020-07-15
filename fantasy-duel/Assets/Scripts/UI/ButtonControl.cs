using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonControl : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 initialScale;
    [SerializeField] private float scale = 0.1f;

    private void Awake()
    {
        initialScale = transform.localScale;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.Play(Audio.SoundEffects, Clip.ButtonHit, false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale += new Vector3(scale, scale, scale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = initialScale;
    }
}
