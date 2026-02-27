using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int levelNum;
    public int enNum;
    bool e;
    GameObject[] enemies;
    Player player;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (levelNum != 0 && e)
        {
            e = false;
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            enNum = enemies.Length;
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
    }
    public void Level1Enter()
    {
        levelNum = UnityEngine.Random.Range(5, 7);
        SceneManager.LoadScene(levelNum);
        StartCoroutine("En");
        levelNum = 1;
    }
    public void Level2Enter()
    {
        levelNum = UnityEngine.Random.Range(1, 3);
        SceneManager.LoadScene(levelNum);
        StartCoroutine("En");
        levelNum = 2;
    }
    public void Level3Enter()
    {
        levelNum = UnityEngine.Random.Range(3, 5);
        SceneManager.LoadScene(levelNum);
        StartCoroutine("En");
        levelNum = 3;
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
            Level3Enter();
        }
        else if (enNum <= 0  && levelNum == 3)
        {
            End();
        }
    }
    public void End()
    {
        if (player.health <= 0)
        {
            SceneManager.LoadScene(7);
        }
        else if (player.health > 0)
        {
            SceneManager.LoadScene(7);
            StartCoroutine("Ends");
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    IEnumerator En()
    {
        yield return new WaitForSeconds(1);
        e = true;
    }
    IEnumerator Ends()
    {
        yield return new WaitForSeconds(0.1f);
        TextMeshProUGUI text = GameObject.FindGameObjectWithTag("UI_End").GetComponent<TextMeshProUGUI>();
        text.text = "You Win!";
    }
}
