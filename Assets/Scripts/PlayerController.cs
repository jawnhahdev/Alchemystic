using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 3.0f;
    public float jumpForce = 10.0f;
    private bool grounded;
    private Rigidbody2D rb2d;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        if (moveHorizontal > 0)
        {
            animator.SetBool("isWalkingForward", true);
        }
        else if (moveHorizontal < 0)
        {
            animator.SetBool("isWalkingBackward", true);

        }else if (moveHorizontal == 0)
        {
            animator.SetBool("isWalkingForward", false);
            animator.SetBool("isWalkingBackward", false);
        }

        
        Move(moveHorizontal);
        

    }

    void FixedUpdate()
    {
        if (rb2d.velocity.y > 0)
        {
            animator.SetBool("isJumping", true);
        }
        else if (rb2d.velocity.y < 0)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
            
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb2d.AddForce(Vector3.up * jumpForce * 30);
           

        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            animator.SetBool("isFire", true);
            animator.SetBool("isIce", false);
            animator.SetBool("isSpace", false);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetBool("isFire", false);
            animator.SetBool("isIce", true);
            animator.SetBool("isSpace", false);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.SetBool("isFire", false);
            animator.SetBool("isIce", false);
            animator.SetBool("isSpace", true);
        }


    }


    private void Move(float moveHorizontal)
    {
        transform.Translate(Vector3.right * moveHorizontal * moveSpeed * Time.deltaTime);
        
    }
    

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            grounded = true;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
            
    }

    private void OnCollisionStay2D(Collision2D col)
    {

    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {

            grounded = false;
        }

    }
}
