using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpForce;
    public float crouchTimer;
    private bool canCrouch;
    private bool canJump;
    public LayerMask lm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
    }

    private void Jump()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, lm))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            Debug.Log("Hit ground");
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rb.AddForceY(jumpForce);
            }
        }
    }
}
