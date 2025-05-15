using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TestPlayer : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector3 inputDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal"); //À§ ¾Æ·¡
        float v = Input.GetAxisRaw("Vertical");   //ÁÂ ¿ì

        inputDirection = new Vector3(h, 0f, v).normalized;
    }

    void FixedUpdate()
    {
        Vector3 moveVelocity = inputDirection * moveSpeed;
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
    }
}
