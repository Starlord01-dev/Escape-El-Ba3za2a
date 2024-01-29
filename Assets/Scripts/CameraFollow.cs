using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CameraFollow : MonoBehaviour
{
    public GameObject playerPosition;

    private UnityEngine.Transform cameraPosition;
    // Start is called before the first frame update
    void Start()
    {
        cameraPosition = GetComponent<UnityEngine.Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        cameraPosition.transform.position = new Vector3(playerPosition.transform.position.x, cameraPosition.transform.position.y, cameraPosition.transform.position.z);
    }
}
