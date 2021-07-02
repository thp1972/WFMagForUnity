using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NR_015
{
    public class IsometricMap : MonoBehaviour
    {
        int WIDTH = 600; // Width of window
        int HEIGHT = 400; // Height of window
        int mapPositionX = 268; // start displaying the map from 
        int mapPositionY = -100; // these window co-ordinates
                                 // the width and height of the map
        const int mapHeight = 20;
        const int mapWidth = 20;
        int[,,] mapBlocks = new int[mapWidth, mapHeight, 3]; // make a blank map in a 3 dimensional list

        public GameObject block;
        bool blockInstantiate = true;

        void Start()
        {
            ScreenUtility.Fill((150, 255, 255));

            // Map building section - make a border, some arches and some pyramids
            foreach (var x in Enumerable.Range(0, mapWidth))
            {
                foreach (var y in Enumerable.Range(0, mapHeight))
                {
                    if (x == 0 || x == mapWidth - 1 || y == 0 || y == mapHeight - 1)
                        mapBlocks[x, y, 0] = 1;
                    if (x == 5 && (y == 4 || y == 13))
                        MakeArch(x, y);
                    if (x == 12 && y == 14)
                        MakeArch(x, y);
                    if ((x == 4 || x == 12) && y == 7)
                        MakePyramid(x, y);
                }
            }

            DrawMap();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                mapPositionX -= 4;
                DrawMap();
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                mapPositionX += 4;
                DrawMap();
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                mapPositionY -= 4;
                DrawMap();
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                mapPositionY += 4;
                DrawMap();
            }
        }

        void DrawMap()
        {
            ScreenUtility.BlitClear("block");
            foreach (var z in Enumerable.Range(0, 3))
            {
                foreach (var x in Enumerable.Range(0, mapWidth))
                {
                    foreach (var y in Enumerable.Range(0, mapHeight))
                    {
                        var bx = (x * 32) - (y * 32) + mapPositionX;
                        var by = (y * 16) + (x * 16) - (z * 32) + mapPositionY;

                        // Only display blocks that are in the window
                        if ((-64 <= bx && bx < WIDTH + 32) && (-64 <= by && by < HEIGHT + 32))
                        {
                            if (mapBlocks[x, y, z] == 1)
                            {
                                string blockName = $"block{x}{y}{z}";

                                // 1 means a block is in this position
                                // The next line needs an image called "block.png" to be in
                                // a subdirectory called "images"
                                ScreenUtility.Blit(block, (bx, by), blitRef: blockName, instantiate: blockInstantiate);
                            }
                        }
                    }
                }
            }
        }

        // Make a three layer arch
        void MakeArch(int x, int y)
        {
            foreach (var z in Enumerable.Range(0, 3))
            {
                mapBlocks[x, y, z] = 1;
                mapBlocks[x, y + 2, z] = 1;
            }
            mapBlocks[x, y + 1, 2] = 1;
        }

        // Make a three layer pyramid
        public void MakePyramid(int x, int y)
        {
            foreach (var px in Enumerable.Range(0, 5))
            {
                foreach (var py in Enumerable.Range(0, 5))
                {
                    mapBlocks[px + x, py + y, 0] = 1;
                }
            }
            foreach (var px in Enumerable.Range(1, 4 - 1))
            {
                foreach (var py in Enumerable.Range(1, 4 - 1))
                {
                    mapBlocks[px + x, py + y, 1] = 1;
                }
            }
            mapBlocks[x + 2, y + 2, 2] = 1;
        }
    }
}