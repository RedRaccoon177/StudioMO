using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class ScaleAdjustor : MonoBehaviour
{
    [SerializeField]
    private Vector2 panelSize = new Vector2(1920, 1080);

    private RectTransform targetRect;
    private RectTransform parentCanvasRect;

    void Update()
    {
        if (targetRect == null || parentCanvasRect == null)
            return;

        Vector2 canvasSize = parentCanvasRect.rect.size;

        float scaleX = canvasSize.x / panelSize.x;
        float scaleY = canvasSize.y / panelSize.y;

        targetRect.localScale = new Vector3(scaleX, scaleY, 1f);
    }

    [ContextMenu("화면 비율 재조정")]
    private void Resize()
    {
        //Transform current = transform;
        //while (current != null)
        //{
        //    Canvas canvas = current.GetComponent<Canvas>();
        //    if (canvas != null)
        //        return canvas;
        //    current = current.parent;
        //}
    }
}
