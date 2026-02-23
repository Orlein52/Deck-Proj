using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    public int levelNum;
    public int enNum;
    bool e;
    GameObject[] enemies;
    Player player;
    void Start()
    {
        if(GameObject.FindGameObjectWithTag("Manager") != gameObject)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (levelNum != 0 && e)
        {
            e = false;
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            enNum = enemies.Length - 1;
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
    }
    public void Level1Enter()
    {
        levelNum = UnityEngine.Random.Range(1, 3);
        SceneManager.LoadScene(levelNum);
        StartCoroutine("En");
        levelNum = 1;
    }
    public void Level2Enter()
    {
        SceneManager.LoadScene(3);
        StartCoroutine("En");
        levelNum++;
    }
    public void EnDeath()
    {
        enNum--;
        if (enNum <= 0 && levelNum == 1)
        {
            Level2Enter();
        }
        else if (enNum <= 0 && levelNum == 2)
        {
            //lvl 3
        }
        else if (enNum <= 0  && levelNum == 3)
        {
            //win
        }
    }
    IEnumerator En()
    {
        yield return new WaitForSeconds(0.5f);
        e = true;
    }
}
