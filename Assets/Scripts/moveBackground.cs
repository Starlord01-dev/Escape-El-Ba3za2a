using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class moveBackground : MonoBehaviour
{

    public float backgroundSpeed;
    //public float boarderLeft;
    //public float boarderRight;
    public float boarderCentre;
    public float totalLevelTime;

    public Transform boarderleftTrans, boarderRightTrans, boarderCentreTrans;

    public List<Sprite> backgroundSprites = new List<Sprite>();
    public Sprite endScreenBackground;

    private bool endFrame;
    private bool stopMoving;
    private bool secondEndFrame;
    private bool wait5seconds;
    // Start is called before the first frame update
    void Start()
    {
        endFrame = false;
        secondEndFrame = false;
        stopMoving = false;
        StartCoroutine("levelTime");
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopMoving)
        {

            if (!endFrame)
            {
                transform.position -= new Vector3(backgroundSpeed * Time.deltaTime, 0, 0);
                if (transform.position.x <= boarderleftTrans.position.x)
                {
                    Debug.Log(boarderRightTrans.position.x);
                    transform.position = new Vector3(boarderRightTrans.position.x, transform.position.y, 0);
                    int randomSprite = Random.Range(0, backgroundSprites.Count);
                    gameObject.GetComponent<SpriteRenderer>().sprite = backgroundSprites[randomSprite];
                }
            }
            else if(endFrame && ! secondEndFrame)
            {
                transform.position -= new Vector3(backgroundSpeed * Time.deltaTime, 0, 0);
                if (transform.position.x <= boarderleftTrans.position.x)
                {
                    transform.position = new Vector3(boarderRightTrans.position.x, transform.position.y, 0);
                    gameObject.GetComponent<SpriteRenderer>().sprite = endScreenBackground;
                    secondEndFrame  = true;
                }
            }
            else
            {
                transform.position -= new Vector3(backgroundSpeed * Time.deltaTime, 0, 0);
                if(transform.position.x <= boarderCentreTrans.position.x)
                {
                    stopMoving = true;
                    Player.instance.OnPlayerStopped();
                }
            }
        }
        if(stopMoving && Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine("wait5Seconds");
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator levelTime()
    {
        yield return new WaitForSeconds(totalLevelTime);
        endFrame = true;
    }
    IEnumerator wait5Seconds()
    {
        yield return new WaitForSeconds(2);
    }
}
