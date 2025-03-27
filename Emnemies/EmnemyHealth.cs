using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;
    private EnemyAI enemyAI;

    //public EnemySpawner spawner;

    void Start()
    {
        currentHealth = maxHealth;
        enemyAI = GetComponent<EnemyAI>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            enemyAI.Die();
        }
    }
}
