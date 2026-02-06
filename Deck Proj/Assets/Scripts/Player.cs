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
    [Header("Jump Stats")]
    public float jumpDis;
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
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Time.timeScale = 1;
        jumpRay = new Ray2D(transform.position - (Vector3.down/2), Vector2.down);
        right = true;
        col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        tempmove = rb.linearVelocity;
        tempmove.x = inputX * speed;
        rb.linearVelocityX = tempmove.x;
        jumpRay.origin = transform.position + ((Vector3.down / 2) + (Vector3.down / 10));
        if (Physics2D.Raycast(jumpRay.origin, jumpRay.direction, jumpDis) && !grounded && !a)
        {
            grounded = true;
        }
        if (Physics2D.Raycast(jumpRay.origin, jumpRay.direction, jumpDis))
        {
            jumped = false;
        }
        if (!Physics2D.Raycast(jumpRay.origin, jumpRay.direction, jumpDis) && grounded)
        {
            StartCoroutine("coyote");
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
        upPlatRay = Physics2D.Raycast(transform.position + (Vector3.up / 1.9999f), Vector2.up);
        downPlatRay = Physics2D.Raycast(transform.position - (Vector3.up / 2), Vector2.down);
        if (upPlatRay.collider)
        {
            if (upPlatRay.collider.gameObject.tag == "Platform")
            {
                Physics2D.IgnoreCollision(col, upPlatRay.collider, true);
            }
        }
        if (downPlatRay.collider)
        {
            if (Physics2D.GetIgnoreCollision(col, downPlatRay.collider) && downPlatRay.collider.gameObject.tag == "Platform" && !d && inputY < 0)
            {
                //StartCoroutine("Plat");
                Physics2D.IgnoreCollision(col, downPlatRay.collider, false);
            }
        }
        if (health <= 0)
        {
            SceneManager.LoadScene(1);
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
            right = false;
        }
        else if (inputX > 0)
        {
            right = true;
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
        yield return new WaitForSeconds(0.5f);
        a = false;
    }
    IEnumerator coyote()
    {
        if (!jumped)
        {
            yield return new WaitForSeconds(cTime);
            grounded = false;
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
    IEnumerator Plat()
    {
        p = true;
        yield return new WaitForSeconds(0.1f);
        p = false;
    }
    IEnumerator Down()
    {
        d = true;
        yield return new WaitForSeconds(0.1f);
        d = false;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "KillBox")
        {
            SceneManager.LoadScene(1);
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
    public void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Collider2D>())
        {
            if (other.gameObject.tag == "Platform")
            {
                if (inputY < 0 && !p)
                {
                    StartCoroutine("Down");
                    Physics2D.IgnoreCollision(col, other.gameObject.GetComponent<Collider2D>(), true);
                }
            }
        }
    }
}
