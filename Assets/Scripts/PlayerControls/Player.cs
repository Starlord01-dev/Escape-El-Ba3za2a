using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D playerRigidBody;
    private Vector2 vectorGravity;
    private bool isJumping;
    private float jumpCounter;
    private bool crouch;
    private bool gameStart;
    private bool loseMoneyAnimationPlaying;

    public float jumpSpeed;
    public float gravityPower;
    public float jumpTime;
    public float playerSpeed;
    public float playerMoneyBalance;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public GameObject moneyBalance;

    public Sprite jumpSprite;
    public Sprite crouchSprite;
    public Sprite walkSprite;
    public Sprite loseMoneySprite;
    public Sprite playerBack;
    public Sprite playerFront;
    public Image winImage;
    public Image loseImage;
    public Image balanceImage;

    public static Player instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameStart = false;
        crouch = false;
        loseMoneyAnimationPlaying = false;
        vectorGravity = new Vector2(0, -Physics.gravity.y);
        playerRigidBody = GetComponent<Rigidbody2D>();
        moneyBalance.GetComponent<TextMeshProUGUI>().SetText(playerMoneyBalance.ToString() + "$");
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = playerBack;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStart)
        {
            //Jumping 
            if (Input.GetButtonDown("Jump") && isGrounded())
            {

                playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpSpeed);
                if(!loseMoneyAnimationPlaying) transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = jumpSprite;
                isJumping = true;
                jumpCounter = 0;
            }

            if (playerRigidBody.velocity.y > 0 && isJumping)
            {
                jumpCounter += Time.deltaTime;
                if (jumpCounter > jumpTime) { isJumping = false; }

                float smoothnessCorrection = jumpCounter / jumpTime;
                float currentGravityPower = gravityPower;
                if (smoothnessCorrection > 0.5f)
                {
                    currentGravityPower = gravityPower * (1 - smoothnessCorrection);
                }
                playerRigidBody.velocity += vectorGravity * gravityPower * Time.deltaTime;
            }

            if (Input.GetButtonUp("Jump"))
            {
                isJumping = false;
                if (!loseMoneyAnimationPlaying) transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = walkSprite;
                jumpCounter = 0;

                if (playerRigidBody.velocity.y > 0)
                {
                    playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, playerRigidBody.velocity.y * 0.6f);
                }
            }

            if (playerRigidBody.velocity.y < 0)
            {
                playerRigidBody.velocity -= vectorGravity * gravityPower * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.LeftControl) && !crouch)
            {
                gameObject.GetComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x, gameObject.GetComponent<BoxCollider2D>().size.y / 2);
                crouch = true;
                if (!loseMoneyAnimationPlaying) transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = crouchSprite;
            }
            if ((Input.GetKeyUp(KeyCode.LeftControl)) && crouch)
            {
                gameObject.GetComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x, gameObject.GetComponent<BoxCollider2D>().size.y * 2);
                crouch = false;
                if (!loseMoneyAnimationPlaying) transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = walkSprite;
            }
        }
        else 
        { 
            Time.timeScale = 0;
            if (Input.GetButtonDown("Jump"))
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = playerFront;
                gameStart = true;
                Time.timeScale = 1;
            }
        }

    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.25f, 0.01f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "lose")
        {
            playerMoneyBalance -= 10;
            moneyBalance.GetComponent<TextMeshProUGUI>().SetText(playerMoneyBalance.ToString()+"$");
            StartCoroutine("loseMoneyAnimation");
            if (playerMoneyBalance <= 0)
            {
                Debug.Log("Game Over you broke");
            }
        }
    }

    public void OnPlayerStopped()
    {
        if(playerMoneyBalance <= 0)
        {
            loseImage.gameObject.SetActive(true);
        }
        else if (playerMoneyBalance <= 50)
        {
            winImage.gameObject.SetActive(true);
        }
        else 
        { 
            balanceImage.gameObject.SetActive(true);
        }
    }

    IEnumerator loseMoneyAnimation()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = loseMoneySprite;
        loseMoneyAnimationPlaying = true;
        yield return new WaitForSeconds(0.5f);
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = walkSprite;
        loseMoneyAnimationPlaying = false;
    }

}
