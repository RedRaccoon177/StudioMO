using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ĵ���� ���� �г��� �������� ��ü ȭ�� �������� �����ִ� Ŭ����
/// </summary>
[ExecuteAlways]
[DisallowMultipleComponent]
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
[RequireComponent(typeof(GraphicRaycaster))]
public class Partition : MonoBehaviour
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

    /// <summary>
    /// ���� �г��� �����ϴ� ������
    /// </summary>
    [Serializable]
    private struct Frame
    {
        public Vector2 division;
        public Vector2 pivot;
        public RectTransform rectTransform;

        private static readonly float OriginalValue = 1.0f;

        public void Resize(Vector2 anchorMin, Vector2 anchorMax, Vector2 sizeDelta)
        {
            if (division.x < OriginalValue)
            {
                division.x = OriginalValue;
            }
            if (division.y < OriginalValue)
            {
                division.y = OriginalValue;
            }
            if (rectTransform != null)
            {
                rectTransform.pivot = pivot;
                rectTransform.anchorMin = anchorMin;
                rectTransform.anchorMax = anchorMax;
                rectTransform.sizeDelta = new Vector2((sizeDelta.x / division.x) - sizeDelta.x, (sizeDelta.y / division.y) - sizeDelta.y);
            }
        }

#if UNITY_EDITOR

        public Color gizmoColor;

        private static readonly int SquarangularEdgeCount = 4;

        public void OnDrawGizmos()
        {
            if (rectTransform != null)
            {
                Vector3[] corners = new Vector3[SquarangularEdgeCount];
                rectTransform.GetWorldCorners(corners);
                Gizmos.color = gizmoColor;
                for (int i = 0; i < SquarangularEdgeCount; i++)
                {
                    Gizmos.DrawLine(corners[i], corners[(i + 1) % SquarangularEdgeCount]);
                }
            }
        }
#endif
    }

    [SerializeField]
    private float _ratio = 0;

    [SerializeField]
    private Frame[] _frames = new Frame[0];

    private static readonly float HalfValue = 0.5f;

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _frames.Length; i++)
        {
            _frames[i].OnDrawGizmos();
        }
    }

    private static readonly Vector2 StandardDivision = new Vector2(1, 1);
    private static readonly Vector2 StandardPivot = new Vector2(0.5f, 0.5f);
    private static readonly Color StandardColor = new Color();

    private void OnValidate()
    {
        for (int i = 0; i < _frames.Length - 1; i++)
        {
            for (int j = i + 1; j < _frames.Length; j++)
            {
                if (_frames[j].rectTransform != null && _frames[j].rectTransform == _frames[i].rectTransform)
                {
                    _frames[j].rectTransform = null;
                    _frames[j].division = StandardDivision;
                    _frames[j].pivot = StandardPivot;
                    _frames[j].gizmoColor = StandardColor;
                }
            }
        }
        Resize();
    }
#endif

    //ȭ�� ũ�Ⱑ ����Ǹ� ȣ��Ǵ� �ݹ� �޼���(ĵ������ ������ �־�� ���� �۵���)
    private void OnRectTransformDimensionsChange()
    {
        Resize();
    }

    //ȭ�� ũ�⿡ ���缭 �г��� �����ϴ� �޼���
    private void Resize()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        if (screenSize.x != 0 && screenSize.y != 0)
        {
            Rect safeArea = Screen.safeArea;
            Vector2 sizeDelta = getRectTransform.sizeDelta * (safeArea.size / screenSize);
            Vector2 anchorMin = new Vector2(safeArea.position.x / screenSize.x, safeArea.position.y / screenSize.y);
            Vector2 anchorMax = new Vector2((safeArea.position.x + safeArea.size.x) / screenSize.x, (safeArea.position.y + safeArea.size.y) / screenSize.y);
            if (_ratio > 0)  //���� ����̸� �������� ������
            {
                float value = (safeArea.size.y / safeArea.size.x) * HalfValue * (1 / (_ratio + 1));
                anchorMin.x = Mathf.Clamp(HalfValue - value, anchorMin.x, HalfValue);
                anchorMax.x = Mathf.Clamp(HalfValue + value, HalfValue, anchorMax.x);
            }
            else if (_ratio < 0)  //���� �����̸� �������� ������
            {
                float value = (safeArea.size.x / safeArea.size.y) * HalfValue * (1 / (-_ratio + 1));
                anchorMin.y = Mathf.Clamp(HalfValue - value, anchorMin.y, HalfValue);
                anchorMax.y = Mathf.Clamp(HalfValue + value, HalfValue, anchorMax.y);
            }
            for (int i = 0; i < _frames.Length; i++)
            {
                _frames[i].Resize(anchorMin, anchorMax, sizeDelta);
            }
        }
    }

#if UNITY_EDITOR
    //ȭ�� ���� Ȯ�� �޼���
    [ContextMenu("ȭ�� ���� Ȯ���ϱ�")]
    private void Log()
    {
        if (_ratio > 0)
        {
            Debug.Log("ȭ�� ���� 1:" + (_ratio + 1));
        }
        else if (_ratio < 0)
        {
            Debug.Log("ȭ�� ���� " + (-_ratio + 1) + ":1");
        }
        else
        {
            Debug.Log("ȭ�� ���� ���� ����");
        }
    }
#endif
}