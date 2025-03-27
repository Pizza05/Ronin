using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackDamage = 20;
    public float attackRange = 1.5f;
    public float attackCooldown = 0.5f;
    private float lastAttackTime;

    public Transform attackPoint;
    public LayerMask enemyLayer;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > lastAttackTime + attackCooldown)
        {
            Attack();
        }
        
    }

    private IEnumerator AttackAnim () 
    {
        animator.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.4f);
        animator.SetBool("isAttack", false);
    }
    void Attack()
    {
        lastAttackTime = Time.time;
        StartCoroutine(AttackAnim());

        // ตรวจสอบว่ามี Enemy อยู่ในระยะโจมตีหรือไม่
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EmnemyHealth>().TakeDamage(attackDamage);
        }
    }

    // วาด Gizmos เพื่อดูระยะโจมตี
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
