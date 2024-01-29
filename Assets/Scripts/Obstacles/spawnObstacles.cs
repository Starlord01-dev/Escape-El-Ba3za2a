using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnObstacles : MonoBehaviour
{

    public GameObject obstacle;
    public float delay;
    public float minTime, maxTime;
    public float deathTimer;
    public float spawnCooldown;
    public float endSpawn;

    private bool canSpawn;
    private bool endspawn;
    // Start is called before the first frame update
    void Start()
    {
        canSpawn = true;
        endspawn = false;
        InvokeRepeating("Spawn_Obstacle", delay, 7f);
        StartCoroutine("EndSpawn");
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    void Spawn_Obstacle()
    {
        if (!endspawn)
        {
            float randomTime = Random.Range(minTime, maxTime);
            Invoke("Spawn_Obstacle", randomTime);
            if (canSpawn)
            {
                GameObject tempObstacle = Instantiate(obstacle, transform.position, Quaternion.identity);
                StartCoroutine(killObject(tempObstacle));
            }
            StartCoroutine("SpawnCooldown");
        }
    }

    IEnumerator killObject(GameObject obstacle)
    {
        yield return new WaitForSeconds(deathTimer);
        Destroy(obstacle);
    }

    IEnumerator SpawnCooldown()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnCooldown);
        canSpawn = true;
    }
    IEnumerator EndSpawn()
    {
        yield return new WaitForSeconds(endSpawn);
        endspawn = true;
    }
}
