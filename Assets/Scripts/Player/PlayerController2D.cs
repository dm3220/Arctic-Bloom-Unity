using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    public float moveSpeed = 4f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 move;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");
        move.Normalize();
        if (move != Vector2.zero)
        {
            animator.SetBool("Walk", true);
            if (move.x > 0)
            {
                transform.localScale = new Vector3(3, 3, 3);
            }
            else if (move.x < 0)
            {
                transform.localScale = new Vector3(-3, 3, 3);

            }
        }

        else

        {
            animator.SetBool("Walk", false);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }
}
