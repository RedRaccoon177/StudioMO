using System;
using UnityEngine;

[ExecuteAlways]
[DisallowMultipleComponent]
[RequireComponent(typeof(Canvas))]
public class ScaleAdjustor : MonoBehaviour
{
    private bool hasRectTransform = false;

    private RectTransform rectTransform;

    private RectTransform getRectTransform
    {
        get
        {
            if(hasRectTransform == false)
            {
                rectTransform = GetComponent<RectTransform>();
                hasRectTransform = true;
            }
            return rectTransform;
        }
    }

    private bool hasCanvas = false;

    private Canvas canvas = null;

    private Canvas getCanvas {
        get
        {
            if (hasCanvas == false)
            {
                canvas = GetComponent<Canvas>();
                hasCanvas = true;
            }
            return canvas;
        }
    }

    /// <summary>
    /// ���� �г��� �����ϴ� ������
    /// </summary>
    [Serializable]
    private struct Frame
    {
        public Vector2 size;
        public Vector2 position;
        public RectTransform rectTransform;

        public void Rescale(Vector2 size, Vector2 position)
        {
            if (rectTransform != null && this.size.x != 0 && this.size.y != 0)
            {
                float x = size.x / this.size.x;
                float y = size.y / this.size.y;
                Vector3 localScale = rectTransform.localScale;
                localScale.x = x;
                localScale.y = y;
                rectTransform.localScale = localScale;
                float z = rectTransform.position.z;
                rectTransform.position = new Vector3(position.x + (this.position.x * x), position.y + (this.position.y * y), z);
            }
        }
    }

    [SerializeField]
    private Frame[] _frames = new Frame[0];

#if UNITY_EDITOR
    private void OnValidate()
    {
        for (int i = 0; i < _frames.Length - 1; i++)
        {
            for (int j = i + 1; j < _frames.Length; j++)
            {
                if (_frames[j].rectTransform != null && _frames[j].rectTransform == _frames[i].rectTransform)
                {
                    _frames[j].rectTransform = null;
                    _frames[j].size = Vector2.zero;
                    _frames[j].position = Vector2.zero;
                }
            }
        }
        Rescale();
    }
#endif

    //ȭ�� ũ�Ⱑ ����Ǹ� ȣ��Ǵ� �ݹ� �޼���(ĵ������ ������ �־�� ���� �۵���)
    private void OnRectTransformDimensionsChange()
    {
        Rescale();
    }

#if UNITY_EDITOR
    [SerializeField]
    private RectTransform selectedRectTransform = null;

    [ContextMenu("������ ��Ʈ Ʈ�������� ���� ��ġ Ȯ��")]
    private void Log()
    {
        if(selectedRectTransform != null)
        {
            Debug.Log(selectedRectTransform.position);
        }
        else
        {
            Debug.LogWarning("������ ��Ʈ Ʈ�������� �����ϴ�.");
        }
    }
#endif

    //ĵ���� ũ�⿡ ���缭 �г��� �����ϴ� �޼���
    private void Rescale()
    {
        Vector2 size = getRectTransform.rect.size;
        Vector2 pivot = getRectTransform.pivot;
        Vector3 position = getRectTransform.position - new Vector3(size.x * pivot.x, size.y * pivot.y);
        for (int i = 0; i < _frames.Length; i++)
        {
            _frames[i].Rescale(size, position);
        }
    }
}