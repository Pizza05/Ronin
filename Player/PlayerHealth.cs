using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public bool isDead = false;

    public Animator animator; // อ้างอิงไปยัง Animator
    public PlayerHealthBar healthBar; // UI แสดงเลือด (ต้องมี HealthBar.cs)

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        StartCoroutine(TakeHitAnim());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator TakeHitAnim()
    {
        animator.SetBool("isTakeHit", true);
        yield return new WaitForSeconds(0.4f);
        animator.SetBool("isTakeHit", false);
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.SetHealth(currentHealth);
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Death"); // เล่นอนิเมชันตาย

        //StartCoroutine(Respawn()); // รีเซ็ตหลังจากตาย (ถ้าต้องการ)

    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f); // รอ 3 วิ
        isDead = false;
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
        animator.SetTrigger("Idle");
        GetComponent<PlayerController>().enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(20); // กด T แล้วเลือดลด 20
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(10); // กด H แล้วเลือดเพิ่ม 10
        }
    }
}

