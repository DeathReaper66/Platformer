using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersAttack : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    private float attackRecovery;

    [Header("Bullet Point")]
    [SerializeField] private Transform bulletPoint;

    [Header("Bullets")]
    [SerializeField] private GameObject[] bullets;

    private Animator anim;
    private PlayersMovement playersMovement;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playersMovement = GetComponent<PlayersMovement>();
    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && attackRecovery > attackCooldown && playersMovement.canAttack())
            Attack();

        attackRecovery += Time.deltaTime;
    }
    private void Attack()
    {
        bullets[FindBullet()].transform.position = bulletPoint.position;
        bullets[FindBullet()].GetComponent<Bullets>().SetDirection(Mathf.Sign(transform.localScale.x));
        attackRecovery = 0;
    }
    private int FindBullet()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}