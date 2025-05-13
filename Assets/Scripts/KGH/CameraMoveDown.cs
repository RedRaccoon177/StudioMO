using UnityEngine;
using System.Collections;

public class CameraMoveDown : MonoBehaviour
{
    [Header("이동 속도 및 거리 설정")]
    public float moveSpeed = 5f;       // 이동 속도
    public float moveDistance = 5f;    // 이동 거리

    [Header("대기 시간 설정")]
    public float activationDelay = 5f; // 이동 활성화 대기 시간

    private Vector3 originalPosition;  // 시작 위치
    private Vector3 targetPosition;    // 목표 위치
    private bool isMovingDown = false; // 이동 상태
    private bool canMove = false;      // 이동 가능 여부

    void Start()
    {
        originalPosition = transform.position;
        targetPosition = originalPosition;

        // 이동 활성화 타이머 시작
        StartCoroutine(EnableMovementAfterDelay());
    }

    IEnumerator EnableMovementAfterDelay()
    {
        // 대기 시간 동안 이동 불가능
        yield return new WaitForSeconds(activationDelay);
        canMove = true;
        Debug.Log("이동 가능!");
    }

    void Update()
    {
        // 이동 가능 여부 확인
        if (!canMove) return;

        // 스페이스바를 누르면 이동 시작
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isMovingDown)
            {
                // 목표 위치를 Y값 기준으로 아래로 설정
                targetPosition = new Vector3(originalPosition.x, originalPosition.y - moveDistance, originalPosition.z);
                isMovingDown = true;
            }
        }

        // 이동 중일 때
        if (isMovingDown)
        {
            // 부드럽게 아래로 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 목표 위치에 도달하면 고정
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMovingDown = false;
                originalPosition = transform.position;  // 새로운 고정 위치
            }
        }
    }
}
