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
    public GameObject lobProj;
    public GameObject m;
    Vector3 pos;
    Vector3 pos2;
    bool maybe;
    float t;
    float attackNum;
    bool lob;
    public float lobRangeFar;
    public float lobRangeClose;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lob)
        {
            m.transform.position = SampleParabola(pos, pos2, 5, t, Vector3.up);
            t += 0.005f;
        }
        if (lob && t > 1)
        {
            Destroy(m);
            lob = false;
            t = 0;
        }
        if (dis <= detectDis)
        {
            if (attackNum >= 2 && !c)
            {
                if (player.transform.position.x < transform.position.x && !cool && dis >= lobRangeFar)
                {
                    rb.linearVelocityX = -speed;
                }
                else if (player.transform.position.x > transform.position.x && !cool && dis >= lobRangeFar)
                {
                    rb.linearVelocityX = speed;
                }
                else if (player.transform.position.x < transform.position.x && !cool && dis <= lobRangeClose)
                {
                    rb.linearVelocityX = speed;
                }
                else if (player.transform.position.x > transform.position.x && !cool && dis <= lobRangeClose)
                {
                    rb.linearVelocityX = -speed;
                }
                else
                {
                    StartCoroutine("Lobbing");
                }
            }
            if (!charging)
            {
                if (!c && attackNum < 2)
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
    public void Lob(Vector3 target)
    {
        pos = transform.position;
        pos2 = target;
        rb.linearVelocityX = 0;
        if (lob)
        {
            Destroy(m);
            t = 0;
        }
        m = Instantiate(lobProj, transform.position, transform.rotation);
        lob = true;
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
        attackNum = Random.Range(1, 3);        
        c = false;
    }
    IEnumerator Lobbing()
    {
        c = true;
        Lob(player.transform.position);
        float a = Random.Range(1, 5);
        yield return new WaitForSeconds(a);
        attackNum = Random.Range(1, 3);
        c = false;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Attack")
        {
            health -= player.GetComponent<Player>().damage;
        }
    }
    Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t, Vector3 outDirection)
    {
        float parabolicT = t * 2 - 1;
        //start and end are not level, gets more complicated
        Vector3 travelDirection = end - start;
        Vector3 levelDirection = end - new Vector3(start.x, end.y, start.z);
        Vector3 right = Vector3.Cross(travelDirection, levelDirection);
        Vector3 up = outDirection;
        Vector3 result = start + t * travelDirection;
        result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
        return result;
    }
}
