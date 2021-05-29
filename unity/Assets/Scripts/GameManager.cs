using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;           
    private BoardManager boardScript; 
    public List<Enemy> enemies; 

    [HideInInspector] public bool playersTurn = true;
    private bool enemiesMoving;
    private float turnDelay = 0.1f;
    private bool doingSetup = true; 

    public bool day_night = true;
    public int day = 1;
    public int time = 100;                              
    public GameObject[] enemyTiles; 

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);    
        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        enemies = new List<Enemy>();
        InitGame();
    }

    void Update()
    {
        if(playersTurn || enemiesMoving) return;
        if(enemies.Count < 5){
            boardScript.LayoutObjectAtRandom(boardScript.enemyTiles, 5, 10);
        }
        StartCoroutine (MoveEnemies());
    }

    void InitGame()
    {
        doingSetup = true;
        enemies.Clear();
        boardScript.SetupScene();
    }

    public void GameOver()
    {
        enabled = false;
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    public void DeleteEnemyFromList(Enemy script)
    {
        enemies.Remove(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0) yield return new WaitForSeconds(turnDelay);
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy ();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
        playersTurn = true;
        enemiesMoving = false;
    }
}