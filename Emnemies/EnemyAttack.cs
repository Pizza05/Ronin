using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage = 10;
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
            animator.SetTrigger("Attack");
        }
    }
}
