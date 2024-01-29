using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class obstacleMovement : MonoBehaviour
{
    public float obstacleSpeed;
    public bool wobble;

    private float counter;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        counter += 0.02f;
        transform.position -= new Vector3(obstacleSpeed*Time.deltaTime, 0f, 0f);
        if(wobble) transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z+Mathf.Sin(counter));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
