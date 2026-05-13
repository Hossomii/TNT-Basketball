using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonAudio : MonoBehaviour, IPointerClickHandler, ISubmitHandler, IPointerEnterHandler
{
    [Header("Settings")]
    [SerializeField] private bool playHoverSound = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance?.PlayUIClick();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        AudioManager.Instance?.PlayUIClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!playHoverSound)
            return;

        AudioManager.Instance?.PlayUIHover();
    }
}