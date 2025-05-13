using UnityEngine;

public class VRCameraController : MonoBehaviour
{
    [Header("회전 속도 설정")]
    public float rotationSpeed = 100f;  // 마우스 감도

    private float pitch = 0f;  // 상하 회전 각도
    private float yaw = 0f;    // 좌우 회전 각도

    void Update()
    {
        // 마우스 입력 받기
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        // 상하 회전 (피치)
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);  // 상하 각도 제한

        // 좌우 회전 (요)
        yaw += mouseX;

        // 회전 적용
        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
