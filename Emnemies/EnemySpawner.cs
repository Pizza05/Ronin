using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float minimumSpawnTime;

    [SerializeField]
    private float maximumSpawnTime;

    private float timeUntillSpawb;

    void Update()
    {
        timeUntillSpawb -= Time.deltaTime;

        if (timeUntillSpawb <= 0 )
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn()
    {
        timeUntillSpawb = Random.Range(minimumSpawnTime, maximumSpawnTime);
    }
}
