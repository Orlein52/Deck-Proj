using UnityEngine;

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
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
                GameObject p = Instantiate(rangedAtk, transform.position, transform.rotation);
                Destroy(p, 0.1f);
            }
            else if (melee)
            {
                GameObject p = Instantiate(meleeAtk, transform.position, transform.rotation);
                Destroy(p, 0.1f);
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
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Attack")
        {
            health -= player.GetComponent<Player>().damage;
        }
    }
}
