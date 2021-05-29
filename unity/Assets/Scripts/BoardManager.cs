using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public static BoardManager instance = null;   
    private int columns = 15;                                      
    private int rows = 15;   
    private int enemyCount1 = 10;
    private int enemyCount2 = 15;                                       
    private Count wallCount = new Count (30, 50);                    
    private Count foodCount = new Count (10, 15);                
    public GameObject[] floorTiles;                                 
    public GameObject[] wallTiles;                                  
    public GameObject[] foodTiles;                                
    public GameObject[] enemyTiles;                                   
    public GameObject[] outerWallTiles;                               
    public GameObject player;                     
    private Transform boardHolder;                                  
    public List <Vector3> gridPositions = new List <Vector3> ();    

    void InitialiseList ()
    {
        gridPositions.Clear ();
        for(int x = 1; x < columns-1; x++)
        {
            for(int y = 1; y < rows-1; y++)
            {
                gridPositions.Add (new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup ()
    {
        boardHolder = new GameObject ("Board").transform;
        for(int x = -1; x < columns + 1; x++)
        {
            for(int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];
                if(x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
                GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent (boardHolder);
            }
        }
    }

    Vector3 RandomPosition ()
    {
        int randomIndex = Random.Range (0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt (randomIndex);
        return randomPosition;
    }

    public void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range (minimum, maximum+1);
        for(int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene ()
    {
        BoardSetup ();
        InitialiseList ();
        LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);
        LayoutObjectAtRandom (enemyTiles, enemyCount1, enemyCount2);
    }
}