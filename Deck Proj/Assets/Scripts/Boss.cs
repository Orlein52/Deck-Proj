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
    public GameObject chargeHit;
    public int chargeDmg;
    bool c;
    GameObject a;
    public GameObject test;
    //float u;
    public GameObject m;
    Vector3 pos;
    Vector3 pos2;
    bool maybe;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        m = Instantiate(test, transform.position + (5 * Vector3.right), transform.rotation);
        pos = transform.position;
        pos2 = player.transform.position;
        charging = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!maybe)
        {
            float u = (-pos.x + m.transform.position.x) * (pos2.x - m.transform.position.x);
            m.transform.position += Vector3.up * u;
            m.transform.position -= Vector3.right;
            Debug.Log(m.transform.position.x + " , " + m.transform.position.y);
            //StartCoroutine("Perhaps");
        }
        if (m.transform.position.y  < -1000)
        {
            Debug.Log(m.transform.position.x);
            Destroy(m);
        }
        if (dis <= detectDis)
        {
            if (!charging)
            {
                if (!c)
                {
                    StartCoroutine("Charging");
                }
                if (player.transform.position.x < transform.position.x && !cool && dis - 1.5f > range)
                {
                    rb.linearVelocityX = -speed;
                }
                else if (player.transform.position.x > transform.position.x && !cool && dis + 1.5f > range)
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
        if (-5 < rb.linearVelocityX && rb.linearVelocityX < 5)
        {
            //charging = false;
            Destroy(a);
        }
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void Charge()
    {
        if (player.transform.position.x < transform.position.x)
        {
            a = Instantiate(chargeHit, transform.position - Vector3.right / 1.9f, transform.rotation, transform);
            rb.AddForceX(-chargeForce, ForceMode2D.Impulse);
        }
        else if (player.transform.position.x > transform.position.x)
        {
            a = Instantiate(chargeHit, transform.position + Vector3.right / 1.9f, transform.rotation, transform);
            rb.AddForceX(chargeForce, ForceMode2D.Impulse);
        }
        //charging = false;
    }
    IEnumerator AtkCool()
    {
        cool = true;
        yield return new WaitForSeconds(atkSpeed);
        cool = false;
    }
    IEnumerator Charging()
    {
        c = true;
        float a = Random.Range(1, 5);
        yield return new WaitForSeconds(a);
        charging = true;
        Charge();
        yield return new WaitForSeconds(1);
        charging = false;
        c = false;
    }
    IEnumerator Perhaps()
    {
        maybe = true;
        yield return new WaitForSeconds(1);
        maybe = false;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Attack")
        {
            health -= player.GetComponent<Player>().damage;
        }
    }
}
