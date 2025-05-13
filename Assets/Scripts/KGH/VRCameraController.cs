using UnityEngine;

public class VRCameraController : MonoBehaviour
{
    [Header("ȸ�� �ӵ� ����")]
    public float rotationSpeed = 100f;  // ���콺 ����

    private float pitch = 0f;  // ���� ȸ�� ����
    private float yaw = 0f;    // �¿� ȸ�� ����

    void Update()
    {
        // ���콺 �Է� �ޱ�
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        // ���� ȸ�� (��ġ)
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);  // ���� ���� ����

        // �¿� ȸ�� (��)
        yaw += mouseX;

        // ȸ�� ����
        transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}
