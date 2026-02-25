using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    public PlayerInput input;
    Rigidbody2D rb;
    Vector2 tempmove;
    public float speed;
    public float inputX;
    public float inputY;
    Ray2D jumpRay;
    BoxCollider2D col;
    public float health;
    public float maxHealth;
    GameManager manager;
    [Header("Jump Stats")]
    public float jumpRayDis;
    public float jumpForce;
    public bool grounded;
    public bool jumped;
    public float cTime;
    bool a;
    bool left;
    bool right;
    bool p;
    bool d;
    RaycastHit2D upPlatRay;
    RaycastHit2D downPlatRay;
    [Header("Attacks")]
    public GameObject hitBox;
    public float hitDur;
    bool attacked;
    public float damage;
    Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Time.timeScale = 1;
        jumpRay = new Ray2D(transform.position - (Vector3.down/2), Vector2.down);
        right = true;
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        tempmove = rb.linearVelocity;
        tempmove.x = inputX * speed;
        rb.linearVelocityX = tempmove.x;
        jumpRay.origin = transform.position + (Vector3.down/1.5f);
        if (Physics2D.Raycast(jumpRay.origin, jumpRay.direction, jumpRayDis) && !grounded && !a)
        {
            grounded = true;
            anim.SetBool("Falling", false);
        }
        if (Physics2D.Raycast(jumpRay.origin, jumpRay.direction, jumpRayDis))
        {
            jumped = false;
        }
        if (!Physics2D.Raycast(jumpRay.origin, jumpRay.direction, jumpRayDis) && grounded)
        {
            StartCoroutine("coyote");
        }
        if (!grounded && !a)
        {
            anim.SetBool("Falling", true);
        }
        if (!grounded)
        {
            if (inputY < 0)
            {
                rb.gravityScale = 4;
            }
            else if (inputY > 0)
            {
                rb.gravityScale = 1;
            }
            else
            {
                rb.gravityScale = 2;
            }
        }
        upPlatRay = Physics2D.Raycast(transform.position - (Vector3.down / 1.5f), Vector2.up);
        downPlatRay = Physics2D.Raycast(transform.position + (Vector3.down / 1.5f), Vector2.down);
        if (downPlatRay.collider)
        {
            if (inputY < 0 && downPlatRay.collider.gameObject.tag == "Platform")
            {
                PlatformEffector2D plat = downPlatRay.collider.GetComponent<PlatformEffector2D>();
                plat.rotationalOffset = 180;
            }
            else if (inputY >= 0 && downPlatRay.collider.gameObject.tag == "Platform")
            {
                PlatformEffector2D plat = downPlatRay.collider.GetComponent<PlatformEffector2D>();
                plat.rotationalOffset = 0;
            }
        }
        if (upPlatRay.collider)
        {
            if(upPlatRay.collider.gameObject.tag == "Platform")
            {
                PlatformEffector2D plat = upPlatRay.collider.GetComponent<PlatformEffector2D>();
                plat.rotationalOffset = 0;
            }
        }
        if (health <= 0)
        {
            manager.End();
        }
        if (inputX == 0)
        {
            anim.SetBool("WalkL", false);
            anim.SetBool("WalkR", false);
        }
        
    }
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 InputAxis = context.ReadValue<Vector2>();
        inputX = InputAxis.x;
        inputY = InputAxis.y;
        if (inputX < 0)
        {
            left = true;
            anim.SetBool("WalkL", true);
            right = false;
        }
        else if (inputX > 0)
        {
            right = true;
            anim.SetBool("WalkR", true);
            left = false;
        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (!a)
        {
            if (grounded)
            {          
                jumped = true;
                grounded = false;
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                anim.SetBool("Jump", true);
                StartCoroutine("JumpFix");
            }
        }
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (left && !attacked)
        {
            GameObject a = Instantiate(hitBox, (transform.position + Vector3.left), transform.rotation, transform);
            StartCoroutine("AtkCool");
            Destroy(a, hitDur);
        }
        else if (right && !attacked) 
        {
            GameObject a =  Instantiate(hitBox, (transform.position - Vector3.left), transform.rotation, transform);
            StartCoroutine("AtkCool");
            Destroy(a, hitDur);
        }
    }
    IEnumerator JumpFix()
    {
        a = true;
        yield return new WaitForSeconds(0.3f);
        a = false;
        anim.SetBool("Jump", false);
    }
    IEnumerator coyote()
    {
        if (!jumped)
        {
            yield return new WaitForSeconds(cTime);
            grounded = false;
            anim.SetBool("Falling", true);
        }
        else
        {
            grounded = false;
        }
    }
    IEnumerator AtkCool()
    {
        attacked = true;
        yield return new WaitForSeconds(hitDur);
        attacked = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "KillBox")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (other.tag == "Enem_Ranged_Atki")
        {
            health--;
        }
        if (other.tag == "Enem_Melee_Atk")
        {
            health--;
        }
    }
}
