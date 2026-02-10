using UnityEngine;
using System.Collections;
public class Boss : MonoBehaviour
{
    public float range;
    RaycastHit2D meleeRay1;
    RaycastHit2D meleeRay2;
    bool cool;
    public float atkSpeed;
    float dis;
    public float detectDis;
    Rigidbody2D rb;
    public float speed;
    GameObject player;
    public float health;
    public float maxHealth;
    public GameObject meleeAtk;
    bool charging;
    public float chargeForce;
    public GameObject chargeHitR;
    public GameObject chargeHitL;
    public float chargeDmg;
    bool c;
    GameObject a;
    public float dmg;
    float cd;
    GameObject p;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dis <= detectDis)
        {
            if (!charging)
            {
                if (!c && !cool)
                {
                    StartCoroutine("Charging");
                }
                if (player.transform.position.x < transform.position.x && !cool && !meleeRay1.rigidbody)
                {
                    rb.linearVelocityX = -speed;
                }
                else if (player.transform.position.x > transform.position.x && !cool && !meleeRay2.rigidbody)
                {
                    rb.linearVelocityX = speed;
                }
                meleeRay2 = Physics2D.Raycast(transform.position + Vector3.right, transform.right, range);
                meleeRay1 = Physics2D.Raycast(transform.position - Vector3.right, -transform.right, range);
                if (meleeRay1.rigidbody && !cool)
                {
                    StartCoroutine("AtkCool");
                    p = Instantiate(meleeAtk, transform.position - Vector3.right, transform.rotation, transform);
                    Destroy(p, atkSpeed);
                }
                else if (meleeRay2.rigidbody && !cool)
                {
                    StartCoroutine("AtkCool");
                    p = Instantiate(meleeAtk, transform.position + Vector3.right, transform.rotation, transform);
                    Destroy(p, atkSpeed);
                }
            }
        }
        if (-5 < rb.linearVelocityX && rb.linearVelocityX < 5)
        {
            charging = false;
            Destroy(a);
            dmg = cd;
        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void Charge()
    {
        cd = dmg;
        dmg = chargeDmg;
        Destroy(p);
        if (player.transform.position.x < transform.position.x)
        {
            a = Instantiate(chargeHitL, transform.position - Vector3.right / 1.9f, transform.rotation, transform);
            rb.AddForceX(-chargeForce, ForceMode2D.Impulse);
        }
        else if (player.transform.position.x > transform.position.x)
        {
            a = Instantiate(chargeHitR, transform.position + Vector3.right / 1.9f, transform.rotation, transform);
            rb.AddForceX(chargeForce, ForceMode2D.Impulse);
        }
        //charging = false;
    }
    IEnumerator AtkCool()
    {
        cool = true;
        yield return new WaitForSeconds(atkSpeed + 0.2f);
        cool = false;
    }
    IEnumerator Charging()
    {
        c = true;
        float a = Random.Range(1, 5);
        charging = true;
        Charge();
        yield return new WaitForSeconds(a);
        c = false;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Attack")
        {
            health -= player.GetComponent<Player>().damage;
        }
    }
}
