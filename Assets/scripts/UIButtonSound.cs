using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIButtonSound : MonoBehaviour, IPointerDownHandler
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (button != null &&
            button.IsActive() &&
            button.IsInteractable() &&
            MusicManager.Instance != null)
        {
            MusicManager.Instance.PlayClick();
        }
    }
}
