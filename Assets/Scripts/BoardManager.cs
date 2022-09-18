using System.Collections;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

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

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();
    
    void InitializeList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        bool isWin = false;
        BoardSetup();
        //InitializeList();
        //LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        //LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        //int enemyCount = (int)Mathf.Log(level, 2f);
        //LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        //Instantiate(exit, new Vector3(columns - 1, rows - 1, 0F), Quaternion.identity);

        //Assets/Resources/day1.txt
        TextAsset levelAsset = null;
        if (level == 1)
        {
            levelAsset = Resources.Load("day1") as TextAsset;
        }
        else if (level == 2)
        {
            levelAsset = Resources.Load("day2") as TextAsset;
        }
        else if (level == 3)
        {
            levelAsset = Resources.Load("day3") as TextAsset;
        }
        else
        {
            isWin = true;
        }

        if (!isWin)
        {
            var lines = levelAsset.text.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == 'X') //Wall
                    {
                        GameObject wall = wallTiles[Random.Range(0, wallTiles.Length)];
                        //i - 1 needs to be inverted
                        Instantiate(wall, new Vector3(j - 1, 8 - i, 0F), Quaternion.identity);
                    } else if (lines[i][j] == 'F')    //Food
                    {
                        GameObject food = foodTiles[Random.Range(0, foodTiles.Length)];
                        Instantiate(food, new Vector3(j - 1, 8 - i, 0F), Quaternion.identity);
                    } else if (lines[i][j] == 'E')    //Enemy
                    {
                        GameObject enemy = enemyTiles[Random.Range(0, enemyTiles.Length)];
                        Instantiate(enemy, new Vector3(j - 1, 8 - i, 0F), Quaternion.identity);
                    } else if (lines[i][j] == 'T')    //Exit
                    {
                        Instantiate(exit, new Vector3(j - 1, 8 - i, 0F), Quaternion.identity);
                    }
                }
            }
        }
    }
}
