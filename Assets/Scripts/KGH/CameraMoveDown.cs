using UnityEngine;
using System.Collections;

public class CameraMoveDown : MonoBehaviour
{
    [Header("�̵� �ӵ� �� �Ÿ� ����")]
    public float moveSpeed = 5f;       // �̵� �ӵ�
    public float moveDistance = 5f;    // �̵� �Ÿ�

    [Header("��� �ð� ����")]
    public float activationDelay = 5f; // �̵� Ȱ��ȭ ��� �ð�

    private Vector3 originalPosition;  // ���� ��ġ
    private Vector3 targetPosition;    // ��ǥ ��ġ
    private bool isMovingDown = false; // �̵� ����
    private bool canMove = false;      // �̵� ���� ����

    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition;

        // �̵� Ȱ��ȭ Ÿ�̸� ����
        StartCoroutine(EnableMovementAfterDelay());
    }

    IEnumerator EnableMovementAfterDelay()
    {
        // ��� �ð� ���� �̵� �Ұ���
        yield return new WaitForSeconds(activationDelay);
        canMove = true;
        Debug.Log("�̵� ����!");
    }

    void Update()
    {
        // �̵� ���� ���� Ȯ��
        if (!canMove) return;

        // �����̽��ٸ� ������ �̵� ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isMovingDown)
            {
                // ��ǥ ��ġ�� Y�� �������� �Ʒ��� ����
                targetPosition = new Vector3(originalPosition.x, originalPosition.y - moveDistance, originalPosition.z);
                isMovingDown = true;
            }
        }

        // �̵� ���� ��
        if (isMovingDown)
        {
            // �ε巴�� �Ʒ��� �̵�
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // ��ǥ ��ġ�� �����ϸ� ����
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMovingDown = false;
                originalPosition = transform.position;  // ���ο� ���� ��ġ
            }
        }
    }
}
