using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// By Pellegrino ~thp~ Principe
namespace NR_012
{
    class Player
    {
        private Vector2 position;
        private GameObject player;
        public GameObject image;

        public Player(GameObject playerPrefab)
        {
            image = player = GameObject.Instantiate(playerPrefab);
        }

        private int mapx;
        public int Mapx
        {
            get => mapx;
            set
            {
                mapx = value;
                position = new Vector2(value, position.y);
                player.transform.position = ScreenUtility.Position(position);
            }
        }

        private int mapy;
        public int Mapy
        {
            get => mapy;
            set
            {
                mapy = value;
                position = new Vector2(position.x, value);
                player.transform.position = ScreenUtility.Position(position);
            }
        }
    }

    class Tile
    {
        public float timer;
        public int t;
        public GameObject i;

        public Tile(int type)
        {
            Set(type);
        }

        public void Set(int type, bool active = false)
        {
            timer = 0;
            t = type;
 
            var op = Addressables.LoadAssetAsync<GameObject>(Bombs.images[type]);
            GameObject g = op.WaitForCompletion(); // force sync!

            // Unity stuff
            if (i != null) GameObject.Destroy(i);
            i = GameObject.Instantiate(g);
            i.SetActive(active);
        }
    }

    public class Bombs : MonoBehaviour
    {
        // set game and screen sizes
        const int SIZE = 9;
        const int WIDTH = SIZE * 45 - 5;
        const int HEIGHT = SIZE * 45 - 5;

        // bomb range
        const int RANGE = 3;

        // constants for tile types
        public static int GROUND = 0;
        public static int WALL = 1;
        public static int BRICK = 2;
        public static int BOMB = 3;
        public static int EXPLOSION = 4;

        // images for tile types
        public static string[] images = { "ground", "wall", "brick", "bomb", "explosion" };

        Player player;
        public GameObject playerPrefab;

        List<List<Tile>> tilemap;

        // Start is called before the first frame update
        void Start()
        {
            // create player in top-left of the game
            player = new Player(playerPrefab);
            player.Mapx = 0;
            player.Mapy = 0;

            tilemap = (from x in Enumerable.Range(0, 10)
                       select (from y in Enumerable.Range(0, 10)
                               select x % 2 == 1 && y % 2 == 1 ? new Tile(WALL) : new Tile(GROUND)).ToList()).ToList();
            tilemap[3][2].Set(BRICK);
            tilemap[4][7].Set(BRICK);
        }

        // Update is called once per frame
        void Update()
        {
            // iterate through each tile in tilemap
            foreach (var x in Enumerable.Range(0, SIZE))
            {
                foreach (var y in Enumerable.Range(0, SIZE))
                {
                    var tile = tilemap[x][y];

                    // decrement timer
                    if (tile.timer > 0)
                        tile.timer -= 1;

                    if (tile.timer <= 0)
                    {
                        // explosions eventually become ground
                        if (tile.t == EXPLOSION)
                        {
                            tile.Set(GROUND);
                        }

                        // bombs eventually create explosions
                        if (tile.t == BOMB)
                        {
                            // bombs radiate out in all 4 directions
                            for (int angle = 0; angle < 360; angle += 90)
                            {
                                var cosa = (int)Mathf.Cos(Mathf.Deg2Rad * angle);
                                var sina = (int)Mathf.Sin(Mathf.Deg2Rad * angle);
                                // RANGE determines bomb reach
                                foreach (var ran in Enumerable.Range(1, RANGE - 1))
                                {
                                    var xoffset = ran * cosa;
                                    var yoffset = ran * sina;
                                    // only create explosions within the tilemap, and only on ground and brick tiles
                                    if (Enumerable.Range(0, SIZE).Contains(x + xoffset) &&
                                        Enumerable.Range(0, SIZE).Contains(y + yoffset) &&
                                        new List<int> { GROUND, BRICK }.Contains(tilemap[x + xoffset][y + yoffset].t))
                                    {
                                        tilemap[x + xoffset][y + yoffset].Set(EXPLOSION);
                                        tilemap[x + xoffset][y + yoffset].timer = 50;
                                    }
                                    else
                                    {
                                        break;
                                    }

                                }
                            }
                            // remove bomb
                            tile.Set(EXPLOSION);
                            tile.timer = 50;
                        }

                    }
                }
            }

            OnKeyDown();
            Draw();

        }
        private void Draw()
        {
            // draw the tilemap
            foreach (var x in Enumerable.Range(0, SIZE))
            {
                foreach (var y in Enumerable.Range(0, SIZE))
                {
                    ScreenUtility.Blit(tilemap[x][y].i, (x * 45, y * 45));
                }
            }

            // draw the player
            ScreenUtility.Blit(player.image, (player.Mapx * 45, player.Mapy * 45));
        }

        private void OnKeyDown()
        {
            // store new temporary player position
            var newx = player.Mapx;
            var newy = player.Mapy;

            // update new position using keyboard
            if (Input.GetKeyDown(KeyCode.LeftArrow) && player.Mapx > 0)
                newx -= 1;
            else if (Input.GetKeyDown(KeyCode.RightArrow) && player.Mapx < SIZE - 1)
                newx += 1;
            else if (Input.GetKeyDown(KeyCode.UpArrow) && player.Mapy > 0)
                newy -= 1;
            else if (Input.GetKeyDown(KeyCode.DownArrow) && player.Mapy < SIZE - 1)
                newy += 1;

            // move player to new position if allowed
            if (new List<int>() { GROUND, EXPLOSION }.Contains(tilemap[newx][newy].t))
            {
                player.Mapx = newx;
                player.Mapy = newy;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                tilemap[player.Mapx][player.Mapy].Set(BOMB, active: true);
                tilemap[player.Mapx][player.Mapy].timer = 150;
            }
        }
    }
}