using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    GameObject player;
    public float health;
    public float maxHealth;
    public GameObject meleeAtk;
    public GameObject rangedAtk;
    public bool ranged;
    public bool melee;
    public bool flexible;
    float dis;
    public float detectDis;
    public float flexDis;
    Rigidbody2D rb;
    public float speed;
    [Header("Ranged")]
    public float fireRange;
    public float projSpeed;
    bool fired;
    public float fireSpeed;
    RaycastHit2D fireRay1;
    RaycastHit2D fireRay2;
    [Header("Melee")]
    public float range;
    RaycastHit2D meleeRay1;
    RaycastHit2D meleeRay2;
    bool cool;
    public float atkSpeed;
    public float dmg;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        dis = Vector3.Distance(player.transform.position, transform.position);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        if (dis <= detectDis)
        {
            //Atk code
            if (ranged)
            {
                fireRay2 = Physics2D.Raycast(transform.position + (Vector3.right/2), transform.right, fireRange);
                fireRay1 = Physics2D.Raycast(transform.position - (Vector3.right/2), -transform.right, fireRange);
                if (fireRay1.rigidbody && !fired)
                {
                    StartCoroutine("FireCool");
                    GameObject p = Instantiate(rangedAtk, transform.position, transform.rotation);
                    Rigidbody2D pr = p.GetComponent<Rigidbody2D>();
                    pr.linearVelocityX = -projSpeed;
                    Destroy(p, fireSpeed);
                }
                else if (fireRay2.rigidbody && !fired)
                {
                    StartCoroutine("FireCool");
                    GameObject p = Instantiate(rangedAtk, transform.position, transform.rotation);
                    Rigidbody2D pr = p.GetComponent<Rigidbody2D>();
                    pr.linearVelocityX = projSpeed;
                    Destroy(p, fireSpeed);
                }
            }
            else if (melee)
            {
                if (player.transform.position.x < transform.position.x && !cool && dis - 1.5f > range)
                {
                    rb.linearVelocityX = -speed;
                }
                else if (player.transform.position.x  > transform.position.x && !cool && dis + 1.5f > range)
                {
                    rb.linearVelocityX = speed;
                }
                meleeRay2 = Physics2D.Raycast(transform.position + Vector3.right, transform.right, range);
                meleeRay1 = Physics2D.Raycast(transform.position - Vector3.right, -transform.right, range);
                if (meleeRay1.rigidbody && !cool)
                {
                    StartCoroutine("AtkCool");
                    GameObject p = Instantiate(meleeAtk, transform.position - Vector3.right, transform.rotation, transform);
                    Destroy(p, atkSpeed);
                }
                else if (meleeRay2.rigidbody && !cool)
                {
                    StartCoroutine("AtkCool");
                    GameObject p = Instantiate(meleeAtk, transform.position + Vector3.right, transform.rotation, transform);
                    Destroy(p, atkSpeed);
                }
            }
        }
        if (flexible)
        {
            if (dis < flexDis)
            {
                melee = true;
                ranged = false;
            }
            else
            {
                ranged = true;
                melee = false;
            }
        }
    }
    IEnumerator FireCool()
    {
        fired = true;
        yield return new WaitForSeconds(fireSpeed + 0.1f);
        fired = false;
    }
    IEnumerator AtkCool()
    {
        cool = true;
        yield return new WaitForSeconds(atkSpeed);
        cool = false;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Attack")
        {
            health -= player.GetComponent<Player>().damage;
        }
    }
}
