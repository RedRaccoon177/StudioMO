using UnityEngine;

public class WaveMovement : MonoBehaviour
{
    [Header("�ĵ� ������ ����")]
    public float amplitude = 20f;  // ���Ʒ� �̵� �Ÿ� (����)
    public float speed = 1f;       // �̵� �ӵ� (�ֱ�)

    private Vector3 startPosition; // �ʱ� ��ġ

    void Start()
    {
        // ���� ��ġ ����
        startPosition = transform.position;
    }

    void Update()
    {
        // �ĵ� ȿ��: Sin �Լ��� �̿��� ���Ʒ� ������
        float yOffset = Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }
}