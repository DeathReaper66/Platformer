using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    [Header("Bullet Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    private float lifetimeRecovery;
    private float direction;
    private bool hit;
    private float movementSpeed;

    private BoxCollider2D boxCollider;
    private Animator anim;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (hit) return;

        movementSpeed = speed * (direction/5);
        transform.Translate(movementSpeed, 0, 0);

        lifetimeRecovery += Time.deltaTime;
        if (lifetimeRecovery >= lifetime)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        anim.SetTrigger("explode");
        boxCollider.enabled = false;
        movementSpeed = 0;
    }
    public void SetDirection(float _direction)
    {
        lifetimeRecovery = 0;

        direction = Mathf.Sign(_direction);
        hit = false;
        gameObject.SetActive(true);
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;
        }

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}