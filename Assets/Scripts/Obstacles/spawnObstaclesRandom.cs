using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnObstaclesRandom : MonoBehaviour
{

    public List<GameObject> obstacle = new List<GameObject>();
    public float delay;
    public float repeating;
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
        InvokeRepeating("Spawn_Obstacle", delay, repeating);
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
            int randObject = Random.Range(0, obstacle.Count);
            if (canSpawn)
            {
                GameObject tempObstacle = Instantiate(obstacle[randObject], transform.position, Quaternion.identity);
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
        yield return new WaitForSeconds(endSpawn + 10);
        endspawn = true;
    }
}
