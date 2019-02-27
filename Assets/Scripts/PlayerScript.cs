using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
    float speed = 5;
    float h;
    Rigidbody2D rb2d;
    bool facingLeft;
    public bool canJump = false;
   
    public int coinsCollected;
    Animator anim;
    public int health;
    [SerializeField]
    Text healthText;
    [SerializeField]
    Text coinText;


	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        facingLeft = true;
        coinsCollected = 0;
        anim = GetComponent<Animator>();
        health = 100;
        coinText.text = coinsCollected.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        h = Input.GetAxis("Horizontal");
        MoveCharacter(h);
        FlipCharacter(h);

        if (h != 0) {
            anim.SetBool("isRunning", true);
        }
        else {
            anim.SetBool("isRunning", false);
        }

        if (Input.GetAxisRaw("Jump") == 1 && canJump && rb2d.velocity.y == 0)
        {
            anim.SetBool("isJumping", true);
            rb2d.AddForce(Vector3.up * 10, ForceMode2D.Impulse);
            canJump = false;
        }
	}

    void MoveCharacter(float h)
    {
        transform.Translate(new Vector3(h,0,0) * Time.deltaTime * speed);
    }

    void FlipCharacter(float h)
    {
        if (h>0 && facingLeft || h<0 && !facingLeft )
        {
            facingLeft = !facingLeft;
            Vector3 localScaleX = transform.localScale;
            localScaleX.x *= -1;
            transform.localScale = localScaleX;
        }

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "platform")
        {
            canJump = true;
            anim.SetBool("isJumping", false);
        }

        if (col.gameObject.tag == "coin")
        {
            coinsCollected++;
            coinText.text = coinsCollected.ToString();
            Destroy(col.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "EnemyAttack")
        {
            if (health > 0)
            {
                health -= 10;
                healthText.text = "Health=" + health.ToString();
            }

            rb2d.AddForce(new Vector2(transform.localScale.x,0)*6, ForceMode2D.Impulse);
        }
    }
}
