using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    [SerializeField] float speed = 2.0f;
    Vector2 motionVector;
    Animator animator;
    public Vector2 lastMotionVector;
    public bool moving;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // If dialogue playing, freeze the player's movement
        if (DialogueManager.instance.DialogueIsPlaying)
        {
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        motionVector = new Vector2(horizontal, vertical);

        animator.SetFloat("horizontal", horizontal);
        animator.SetFloat("vertical", vertical);

        moving = horizontal != 0f || vertical != 0f;
        animator.SetBool("moving", moving);

        if (moving)
        {
            lastMotionVector = new Vector2(horizontal, vertical).normalized;

            animator.SetFloat("lastHorizontal", horizontal);
            animator.SetFloat("lastVertical", vertical);
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rigidbody2d.velocity = motionVector * speed;
    }
}
