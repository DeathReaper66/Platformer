using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersMovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float speed;
    private float horizontalInput;

    [Header("Components")]
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private Animator anim;
    [SerializeField] private BoxCollider2D playerBox;

    [Header("JumpParameters")]
    [SerializeField] private float jumpPower;
    [SerializeField] private float coyoteJumpCooldown;
    private float coyoteJumpRecovery;

    [Header("Dash Parameters")]
    [SerializeField] private float dashPower;
    [SerializeField] private float dashCooldown;
    private float dashRecovery;

    [Header("Other Parameters")]
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerBox = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        Setdirection();

        if (!isGrounded())
        {
            coyoteJumpRecovery += Time.deltaTime;
            if (Input.GetKey(KeyCode.Space) && coyoteJumpRecovery < coyoteJumpCooldown)
                Jump();
        }
        else if (isGrounded())
        {
            coyoteJumpRecovery = 0;
            if (Input.GetKey(KeyCode.Space))
                Jump();
        }

        dashRecovery += Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftShift))
            Dash();

        anim.SetBool("running", horizontalInput != 0);
        anim.SetBool("isGrounded", isGrounded());

    }
    private void Setdirection()
    {

        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(4, 4, 1);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-4, 4, 1);
    }

    private void Jump()
    {
        coyoteJumpRecovery = coyoteJumpCooldown;
        body.velocity = new Vector2(body.velocity.x, jumpPower);
        anim.SetTrigger("jump");
    }
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerBox.bounds.center, playerBox.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return isGrounded();
    }

    private void Dash()
    {
        if (dashRecovery >= dashCooldown)
        {
            StartCoroutine(InvunerabilityForDash());

            anim.SetTrigger("dash");
            if (transform.localScale.x < 0)
            {
                if (isGrounded())
                {
                    body.AddForce(Vector2.up * 120);
                    body.AddForce(Vector2.left * dashPower);
                }
                else
                    body.AddForce(Vector2.left * dashPower);
            }
            else if (transform.localScale.x > 0)
            {
                if (isGrounded())
                {
                    body.AddForce(Vector2.up * 120);
                    body.AddForce(Vector2.right * dashPower);
                }
                else
                    body.AddForce(Vector2.right * dashPower);
            }
            dashRecovery = 0;
        }
    }
    private IEnumerator InvunerabilityForDash()
    {
        Physics2D.IgnoreLayerCollision(8,9,true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(8,9,false);
    }
}
