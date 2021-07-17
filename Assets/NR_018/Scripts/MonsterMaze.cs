using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NR_018
{
    public class MonsterMaze : MonoBehaviour
    {
        // Unity stuff
        public GameObject backPrefab;
        GameObject back;
        public GameObject[] leftPrefabs;
        GameObject[] left = new GameObject[5];
        public GameObject[] rightPrefabs;
        GameObject[] right = new GameObject[5];
        public GameObject[] midPrefabs;
        GameObject[] mid = new GameObject[5];

        int WIDTH = 600;
        int HEIGHT = 600;

        int[,] maze = new int[,]
        {
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        { 1, 1, 0, 1, 0, 1, 0, 0, 0, 1 },
        { 1, 0, 0, 0, 0, 1, 0, 1, 0, 1 },
        { 1, 1, 0, 1, 0, 0, 0, 0, 0, 1 },
        { 1, 1, 0, 1, 0, 1, 0, 1, 0, 1 },
        { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 1, 0, 1, 0, 1, 1, 0, 1, 1 },
        { 1, 1, 0, 1, 0, 1, 1, 0, 1, 1 },
        { 1, 1, 0, 0, 0, 0, 0, 0, 0, 1 },
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
        };

        int playerX = 1;
        int playerY = 4;
        int playerDir = 2;
        int[] dirX = { -1, 0, 1, 0 };
        int[] dirY = { 0, 1, 0, -1 };

        // Start is called before the first frame update
        void Start()
        {
            InitializeMazeBlocks();

        }

        // Update is called once per frame
        void Update()
        {
            Draw();
            OnKeyDown();
        }

        void Draw()
        {
            DeactivateMazeBlocks();
            ScreenUtility.Fill((255, 255, 255));
            ScreenUtility.Blit(back, (0, 0), "back", isActive: true);
            DrawMaze();
        }

        void OnKeyDown()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                int newX, newY;
                newX = playerX + dirX[playerDir];
                newY = playerY + dirY[playerDir];
                if (maze[newX, newY] == 0)
                {
                    playerX = newX;
                    playerY = newY;
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                int newX, newY;
                newX = playerX - dirX[playerDir];
                newY = playerY - dirY[playerDir];
                if (maze[newX, newY] == 0)
                {
                    playerX = newX;
                    playerY = newY;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                playerDir -= 1;
                if (playerDir < 0)
                    playerDir = 3;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                playerDir += 1;
                if (playerDir > 3)
                    playerDir = 0;
            }
        }

        void DrawMaze()
        {
            int dm = 1;
            if (playerDir == 1 || playerDir == 3)
                dm = -1;
            for (int l = 4; l > -1; l--)
            {
                int x = playerX + (l * dirX[playerDir]);
                int y = playerY + (l * dirY[playerDir]);
                if (x >= 0 && x < 10 && y >= 0 && y < 10)
                {
                    int xl = x + (dirY[playerDir] * dm);
                    int yl = y + (dirX[playerDir] * dm);

                    if (maze[xl, yl] == 1)
                        ScreenUtility.Blit(left[l], (0, 0), $"left{l}", isActive: true);
                    int xr = x - (dirY[playerDir] * dm);
                    int yr = y - (dirX[playerDir] * dm);
                    if (maze[xr, yr] == 1)
                        ScreenUtility.Blit(right[l], (0, 0), $"right{l}", isActive: true);
                    if (maze[x, y] == 1)
                        ScreenUtility.Blit(mid[l], (0, 0), $"mid{l}", isActive: true);
                }
            }
        }

        void InitializeMazeBlocks()
        {
            back = Instantiate(backPrefab);
            back.GetComponent<SpriteRenderer>().sortingOrder = -5;
            ScreenUtility.Blit(back, (0, 0), "back", isActive: false);
            for (int i = 0; i < 5; i++)
            {
                left[i] = Instantiate(leftPrefabs[i]);
                left[i].GetComponent<SpriteRenderer>().sortingOrder = -i;
                ScreenUtility.Blit(left[i], (0, 0), $"left{i}", isActive: false);
                right[i] = Instantiate(rightPrefabs[i]);
                right[i].GetComponent<SpriteRenderer>().sortingOrder = -i;
                ScreenUtility.Blit(right[i], (0, 0), $"right{i}", isActive: false);
                mid[i] = Instantiate(midPrefabs[i]);
                mid[i].GetComponent<SpriteRenderer>().sortingOrder = i;
                ScreenUtility.Blit(mid[i], (0, 0), $"mid{i}", isActive: false);
            }
        }

        void DeactivateMazeBlocks()
        {
            back.SetActive(false);
            for (int i = 0; i < 5; i++)
            {
                left[i].SetActive(false);
                right[i].SetActive(false);
                mid[i].SetActive(false);
            }
        }
    }
}