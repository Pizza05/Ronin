using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 2f; // ความเร็วของศัตรู
    public float chaseRange = 30f; // ระยะที่ศัตรูจะเริ่มไล่ตาม
    public float attackRange = 1.5f; // ระยะที่ศัตรูจะโจมตี
    public int attackDamage = 10; // ความเสียหายของการโจมตี
    public LayerMask playerLayer; // เลเยอร์ของผู้เล่น

    private int currentHealth;
    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isDead = false;
    private bool isAttacking = false;
    private Vector3 originalScale;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // หา Player โดยใช้ Tag
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (isDead || isAttacking || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < attackRange) // ถ้าผู้เล่นเข้าใกล้ระยะโจมตี
        {
            Attack();
        }
        else if (distance < chaseRange) // ถ้าผู้เล่นเข้าใกล้ระยะไล่
        {
            MoveTowardsPlayer();
        }
        else
        {
            StopMoving();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;

        animator.SetBool("IsMoving", true);
        Flip(direction.x); // หมุนทิศทางการหันหน้าของศัตรู
    }

    void StopMoving()
    {
        rb.velocity = Vector2.zero;
        animator.SetBool("IsMoving", false);
    }

    void Flip(float moveX)
    {
        if (moveX != 0)
        {
            transform.localScale = new Vector3((moveX > 0) ? Mathf.Abs(originalScale.x) : -Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }

    void Attack()
    {
        if (isAttacking) return;

        isAttacking = true;
        animator.SetTrigger("Attack");
        rb.velocity = Vector2.zero;
        Flip(player.position.x - transform.position.x); // หมุนศัตรูไปทางผู้เล่น

        Invoke(nameof(DealDamage), 0.5f); // รออนิเมชันโจมตี
        Invoke(nameof(StopAttack), 1f); // หยุดโจมตีหลังจากเสร็จ
    }

    void DealDamage()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        if (hitPlayer != null)
        {
            PlayerHealth playerHealth = hitPlayer.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }

    void StopAttack()
    {
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        animator.SetTrigger("TakeHit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetBool("IsDead", true); // เล่นอนิเมชันตาย
        rb.velocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;

        Destroy(gameObject, 2f); // ทำลายตัวเองหลังจาก 2 วินาที
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // แสดงระยะโจมตี
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange); // แสดงระยะไล่ตาม
    }
}
