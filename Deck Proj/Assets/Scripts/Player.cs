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
    public float jumpDis;
    public float jumpForce;
    public bool grounded;
    public bool jumped;
    public float cTime;
    public Transform aim;
    float angleDeg;
    float angleRad;
    Vector3 mousePos;
    Camera cam;
    bool a;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Time.timeScale = 1;
        jumpRay = new Ray2D(transform.position - (Vector3.down/2), Vector2.down);
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        tempmove = rb.linearVelocity;
        tempmove.x = inputX * speed;
        rb.linearVelocityX = (tempmove.x);
        jumpRay.origin = transform.position + ((Vector3.down / 2) + (Vector3.down / 10));
        mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        angleRad = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);
        angleDeg = (180 / Mathf.PI) * angleRad - 0;
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
    }
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 InputAxis = context.ReadValue<Vector2>();
        inputX = InputAxis.x;
        inputY = InputAxis.y;
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
    IEnumerator JumpFix()
    {
        a = true;
        yield return new WaitForSeconds(0.5f);
        a = false;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "KillBox")
        {
            SceneManager.LoadScene(0);
        }
    }
}
