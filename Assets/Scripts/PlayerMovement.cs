// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// public class PlayerMovement : MonoBehaviour
// {
//     [SerializeField] private float speed;
//     [SerializeField] private float jumpPower;

//     [SerializeField] private LayerMask wallLayer;
//     [SerializeField] private LayerMask groundLayer;
//     [SerializeField] private AudioClip jumpSound;
//     [Header("Wall Jumping")]
//     [SerializeField] private float wallJumpX; //Horizontal wall jump force
//     [SerializeField] private float wallJumpY; //Vertical wall jump force
//     private float wallJumpCooldown;
//     private float xAxis;

//     private BoxCollider2D boxCollider;
//     private Animator anim;
//     private Rigidbody2D body;



//     private Vector2 direction;
//     // Update is called once per frame

//     void Awake()
//     {
//         body = GetComponent<Rigidbody2D>();
//         anim = GetComponent<Animator>();
//         boxCollider = GetComponent<BoxCollider2D>();
//     }
//     public void Update()
//     {


//         xAxis = direction.x;
//         if (xAxis > 0) transform.localScale = new Vector3(1, 1, 1);
//         else if (xAxis < 0) transform.localScale = new Vector3(-1, 1, 1);


//         if (xAxis != 0)
//         {

//             anim.SetBool("isRunning", true);

//         }
//         else
//         {

//             anim.SetBool("isRunning", false);


//         }
//         anim.SetBool("grounded", isGrounded());
//         if (wallJumpCooldown < 0.2f)
//         {

//             if (Input.GetKey(KeyCode.Space))
//             {
//                 Jump();
//                 if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
//                     SoundManager.instance.PlaySound(jumpSound);

//             }


//             body.velocity = new Vector2(xAxis * speed, body.velocity.y);

//         }
//         else
//         {
//             wallJumpCooldown += Time.deltaTime;
//         }
//     }
//     public void Jump()
//     {
//         SoundManager.instance.PlaySound(jumpSound);
//         if (isGrounded())
//         {
//             body.velocity = new Vector2(body.velocity.x, jumpPower);
//             anim.SetTrigger("isJump");
//         }
//         else if (onWall() && !isGrounded())
//         {
//             if (xAxis == 0)
//             {
//                 body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
//                 transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);

//             }
//             else
//             {
//                 body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

//                 wallJumpCooldown = 0;
//             }
//         }

//     }
//     private void WallJump()
//     {
//         body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
//         wallJumpCooldown = 0;
//     }

//     public void OnCollisionEnter2D(Collision2D collision)
//     {

//     }
//     public bool isGrounded()
//     {
//         RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
//         return raycastHit.collider != null;
//     }
//     public bool onWall()
//     {

//         RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
//         return raycastHit.collider != null;
//     }


//     public void UpMovementButtonEnter()
//     {
//         if (onWall() && !isGrounded())
//         {

//             body.gravityScale = 0;
//             body.velocity = Vector2.zero;
//         }
//         else
//             body.gravityScale = 3;


//         direction.y = 1;
//     }
//     public void UpMovementButtonExit()
//     {
//         direction.y = 0;
//     }


//     public void RightMovementButtonEnter()
//     {
//         direction.x = 1;
//     }
//     public void LeftMovementButtonEnter()
//     {
//         direction.x = -1;
//     }
//     public void HorizontalMovementExit()
//     {
//         direction.x = 0;
//     }
//     public bool canAttack()
//     {
//         return xAxis == 0 && isGrounded() && !onWall();
//     }
// }
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime; //How much time the player can hang in the air before jumping
    private float coyoteCounter; //How much time passed since the player ran off the edge

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX; //Horizontal wall jump force
    [SerializeField] private float wallJumpY; //Vertical wall jump force

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    // private float horizontalInput;
    private float xAxis;
    private Vector2 direction;

    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        xAxis = direction.x;
        if (xAxis > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (xAxis < 0) transform.localScale = new Vector3(-1, 1, 1);


        if (xAxis != 0)
        {

            anim.SetBool("isRunning", true);

        }
        else
        {

            anim.SetBool("isRunning", false);


        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        //Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        if (onWall())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 3;
            body.velocity = new Vector2(xAxis * speed, body.velocity.y);

            if (isGrounded())
            {
                coyoteCounter = coyoteTime; //Reset coyote counter when on the ground
                jumpCounter = extraJumps; //Reset jump counter to extra jump value
            }
            else
                coyoteCounter -= Time.deltaTime; //Start decreasing coyote counter when not on the ground
        }
    }

    private void Jump()
    {
        if (coyoteCounter <= 0 && !onWall() && jumpCounter <= 0) return;
        //If coyote counter is 0 or less and not on the wall and don't have any extra jumps don't do anything

        SoundManager.instance.PlaySound(jumpSound);

        if (onWall())
            WallJump();
        else
        {
            if (isGrounded())
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            else
            {
                //If not on the ground and coyote counter bigger than 0 do a normal jump
                if (coyoteCounter > 0)
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                else
                {
                    if (jumpCounter > 0) //If we have extra jumps then jump and decrease the jump counter
                    {
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }

            //Reset coyote counter to 0 to avoid double jumps
            coyoteCounter = 0;
        }
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }


    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return xAxis == 0 && isGrounded() && !onWall();
    }
    public void UpMovementButtonEnter()
    {
        if (onWall() && !isGrounded())
        {

            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
            body.gravityScale = 3;


        direction.y = 1;
    }
    public void UpMovementButtonExit()
    {
        direction.y = 0;
    }


    public void RightMovementButtonEnter()
    {
        direction.x = 1;
    }
    public void LeftMovementButtonEnter()
    {
        direction.x = -1;
    }
    public void HorizontalMovementExit()
    {
        direction.x = 0;
    }
    // public bool canAttack()
    // {
    //     return xAxis == 0 && isGrounded() && !onWall();
    // }
}