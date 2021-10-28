using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragWindow : MonoBehaviour, IDragHandler
{
    RectTransform dragRectTransform;
    RectTransform canvasDragRectTransform;

    Canvas canvas;
    CanvasScaler canvasScaler;

    void Awake()
    {
        dragRectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasDragRectTransform = canvas.GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 anchoredPosition = dragRectTransform.anchoredPosition;
        anchoredPosition += eventData.delta / canvas.scaleFactor;

        float anchoredPosX = Mathf.Clamp(anchoredPosition.x, -canvasDragRectTransform.rect.width / 2f, canvasDragRectTransform.rect.width / 2f);
        float anchoredPosY = Mathf.Clamp(anchoredPosition.y, -canvasDragRectTransform.rect.height / 2f, canvasDragRectTransform.rect.height / 2f);

        anchoredPosition = new Vector2(anchoredPosX, anchoredPosY);

        dragRectTransform.anchoredPosition = anchoredPosition;
    }
}