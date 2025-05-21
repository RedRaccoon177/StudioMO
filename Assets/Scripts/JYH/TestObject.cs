using UnityEngine;

[DisallowMultipleComponent]
public class TestObject : MonoBehaviour
{
    private void OnValidate()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if(rectTransform != null)
        {
            Debug.Log(rectTransform.sizeDelta);
        }
    }
}
