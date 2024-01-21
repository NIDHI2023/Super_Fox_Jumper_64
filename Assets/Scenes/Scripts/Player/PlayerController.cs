//nothing autosaves so if you make a change you HAVE to save for unity to do stuff
//go to edit, input manager to find types of input that is mapped to stuff
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //anything public you can interact with editor
    //want to control speed for player
    //double gets too precise so we use float
    public float moveSpeed;

    //want to know how much jumping we want, could be speed or height
    public float jumpHeight;


    //Transform is an obj position or location in the scene. We wiill use this to create a point in space to check where the ground is
    public Transform groundCheckSpot;
    //The ground (like a circle), how big is the circle (float radius)
    public float groundCheckRadius;
    //want tot know the Layer this circle is touching for now we only want the player to be jumping off of the ground
    //LayerMask provides us adropdown in the inspector to choose a Layer to check for, can have multiple Layers selected
    public LayerMask whatLayerIsGrounded;
    //we want to do a single time check and know if we're on the ground or not
    public bool isGrounded;
    public bool isCrouched;

    //want to know if on the ladder and if we should be showing the climbing animation
    public bool isClimbing;
    public bool onLadder;

    public float knockbackForce;
    public float knockbackFrames;
    public float invincibleFrames;

    public GameObject bounceBox;

    private float knockbackCounter;
    private float invincibleCounter;

    public AudioSource jumpSound;
    public AudioSource hurtSound;

    public Vector3 respawnPos;


    //want access to our own rigidbody
    public Rigidbody2D myRB;
    public bool canMove;

    //need a reference to LevelManager so we can access it
    private LevelManager levelManager;

    //we want to know what our initial gravity scale is bc we will be changing it
    private float initialGravity;



    //we want to access our own Animator to be able to modify our animations and trigger the transitions
    private Animator myAnimator;

    //we should know what to go back to once attachment to platform is over
    private Transform prevParent;

    // Start is called before the first frame update
    //this is like a constructor, we initalize stuff here
    void Start()
    {
        //we want to grab our own rb cause there might be oterh enemies n stuff
        myRB = GetComponent<Rigidbody2D>();
        //the diamonds tell us what component we are looking for
        myAnimator = GetComponent<Animator>();
        respawnPos = transform.position;
        initialGravity = myRB.gravityScale;

        //need to find the object that has LevelManager script attached to it
        levelManager = FindObjectOfType<LevelManager>();

        canMove = true;
        prevParent = transform.parent;
        
    }

    // Update is called once per frame
    //we put stuff that happens over and over and over again here
    //you must be careful in case things freeze
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckSpot.position, groundCheckRadius, whatLayerIsGrounded);

        //test if we're getting knockedback or not
        //if we aren't we want to behave as normal
        if (knockbackCounter <= 0f && canMove)
        {
            //GetKey will return true as long as key is pressed down
            //KeyCode allows us to grab any key without knowing its special code
            if (Input.GetAxisRaw("Horizontal") > 0f)
            {
                //movement is always done through the RB
                //velocity is how quick the player iis moving
                //Vector3 has x, y, and z values. x is our moveSpeed, y is our current y, z we will never change since we are 2D
                myRB.velocity = new Vector3(moveSpeed, myRB.velocity.y, 0f);

                //we want our player to actually face left or right depending on which way they run
                //this is done through scaling: +x points right -x points left
                //we don't need to look for the transform like we did for Rigidbody and Animator because every object has a transform
                transform.localScale = new Vector3(1f, 1f, 1f);


            } //we don't want 2 separate if statements cause if you hold both directions at same time it gets confused.
              //this will do what is registered first
            else if (Input.GetAxisRaw("Horizontal") < 0f)
            {
                myRB.velocity = new Vector3(-moveSpeed, myRB.velocity.y, 0f);
                //now we face left
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                //this eliminates fox slide. but w/o this it's good for ice levels
                myRB.velocity = new Vector3(0f, myRB.velocity.y, 0f);
            }
            //while there are Axes for Jump we want to use GetButtonDown for jump to be instant
            //GetButtonDown will jump right as button is pressed and then stop even if held down
            //GetButtonUp would only jump when we let go of space, which is a good idea for charging up a super jump
            //use Axes nmae for "jump" instead of KeyCode
            if (Input.GetButtonDown("Jump") && isGrounded && !onLadder)
            {
                myRB.velocity = new Vector3(myRB.velocity.x, jumpHeight, 0f);
                jumpSound.Play();
            }

            if (onLadder)
            {
                isClimbing = true;
                if (Input.GetAxisRaw("Vertical") < 0f)
                {
                    myRB.velocity = new Vector3(myRB.velocity.x, -moveSpeed, 0f);
                }
                else if (Input.GetAxisRaw("Vertical") > 0f)
                {
                    myRB.velocity = new Vector3(myRB.velocity.x, moveSpeed, 0f);
                }
                else
                {
                    myRB.velocity = new Vector3(0f, 0f, 0f);
                }
            }

            if (myRB.velocity.y == 0 && isGrounded)
            {
                //we stop climbing animation and not on ladder anymore
                isClimbing = false;
                onLadder = false;
            }

            if (!onLadder)
            {
                isClimbing = false;
                if (Input.GetAxisRaw("Vertical") < 0f)
                {
                    isCrouched = true;
                }
                else if (Input.GetAxisRaw("Vertical") > 0f)
                {
                    isCrouched = false;
                }
            }

        }

        //now we have been knockedback by an enemy, so we count back to 0
        if (knockbackCounter > 0f)
        {
            //tick counter to 0
            knockbackCounter -= Time.deltaTime;
            if (transform.localScale.x > 0f)
            {
                myRB.velocity = new Vector3(-knockbackForce, knockbackForce, 0f);
            } else
            {
                myRB.velocity = new Vector3(-knockbackForce, knockbackForce, 0f);
            }
        }

        if (levelManager.invincibilityFrames)
        {
            if (invincibleCounter <= 0f)
            {
                levelManager.invincibilityFrames = false;
            }
            else if (invincibleCounter > 0f)
            {
                invincibleCounter -= Time.deltaTime;
            }

        }

        //we want to pass some data back to the Animator so it can change our animations
        //Set methods on the animator object let us do this
        //the Set methods have two args: String name of the param they're setting, value to set that param to
        //we have these vals alreayd (speed and grounded) -> (x velocity ONLY POSITIVE, isGrounded value)
        myAnimator.SetFloat("speed", Mathf.Abs(myRB.velocity.x));
        myAnimator.SetFloat("height", myRB.velocity.y);
        myAnimator.SetBool("grounded", isGrounded);
        myAnimator.SetBool("crouched", isCrouched);
        myAnimator.SetBool("climbing", isClimbing);

        if (myRB.velocity.y < 0f)
        {
            //activate only when falling down
            bounceBox.SetActive(true);
        } else
        {
            bounceBox.SetActive(false);
        }

    }

    //tells us when something is triggered, so it can fall through
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "KillPlane")
        {
            transform.position = respawnPos;
            levelManager.Respawn();
        }

        if(other.tag == "Checkpoint")
        {
            respawnPos = other.transform.position;
        }

        if (other.tag == "Finish")
        {
            Destroy(GameObject.FindWithTag("BossBridge"));
        }
    }

    //we must stay in this trigger to actually interact with the ladder
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Ladder" && Input.GetButton("Vertical"))
        {
            //changing our gravityScale means that we won't fall down the ladder if we let go of the key. instead we will stay in place on the ladder
            myRB.gravityScale = 0;
            onLadder = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ladder")
        {
            //set gravity back to initial
            myRB.gravityScale = initialGravity;
            onLadder = false;
        }
    }


    //this tells us when our object collides with another object
    void OnCollisionEnter2D(Collision2D other)
    {
        //want to check what we're collidign against but no Collision object - that's just an instance in time
        if (other.gameObject.tag == "MovingPlatform" || other.gameObject.tag == "MovingPlatformV")
        {
            //capturing what we are currently part of
            //prevParent = transform.parent;
            //if we know the thing we just collided with is the moving platform we make ourselves a child of this plaform so we move the same speed
            transform.parent = other.transform;
        }
    }

    //we need to know when we have left the platform to regain control
    void OnCollisionExit2D(Collision2D other)
    {
        Debug.Log(other.gameObject.tag);

        if (other.gameObject.tag == "MovingPlatform" || other.gameObject.tag == "MovingPlatformV")
        {
            transform.parent = prevParent;

        }
    }

    public void Knockback ()
    {
        //want counter to be set to however long we want knockbackFrames
        knockbackCounter = knockbackFrames;
        invincibleCounter = invincibleFrames;
        levelManager.invincibilityFrames = true;
    }

}
