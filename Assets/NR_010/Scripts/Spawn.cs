using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using PygameZero; // contains Actor class

namespace NR_010
{
    class Enemy : Actor
    {
        // static list, to keep track of all enemies
        public static List<Actor> enemies = new List<Actor>();
        public Enemy(string image, Vector2 pos) : base(image, pos)
        {
            enemies.Add(this);
        }

        public override void Destroy()
        {
            base.Destroy();
            // remove self from the enemies list
            enemies.Remove(this);
        }
    }

    class LargeEnemy : Enemy
    {
        // all large-sized enemies have the same image
        public LargeEnemy(Vector2 pos) : base("large_enemy", pos) { }

        public override void Destroy()
        {
            // spawn 2 medium-sized enemies when destroying
            var m1 = new MediumEnemy(pos: new Vector2(pos.x - 40, pos.y - 40));
            var m2 = new MediumEnemy(pos: new Vector2(pos.x + 40, pos.y + 40));
            base.Destroy();
        }
    }

    class MediumEnemy : Enemy
    {
        // all medium-sized enemies have the same image
        public MediumEnemy(Vector2 pos) : base("medium_enemy", pos) { }

        public override void Destroy()
        {
            //  spawn 2 small-sized enemies when destroying
            var s1 = new SmallEnemy(pos: new Vector2(pos.x - 20, pos.y - 20));
            var s2 = new SmallEnemy(pos: new Vector2(pos.x + 20, pos.y + 20));
            base.Destroy();
        }
    }

    class SmallEnemy : Enemy
    {
        // all small-sized enemies have the same image
        public SmallEnemy(Vector2 pos) : base("small_enemy", pos) { }
    }

    public class Spawn : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            // start with 2 large-sized enemies
            var l1 = new LargeEnemy(pos: new Vector2(300, 150));
            var l2 = new LargeEnemy(pos: new Vector2(150, 300));
        }

        // Update is called once per frame
        void Update()
        {
            // draw all enemies in static enemies list
            foreach (var e in Enemy.enemies)
                e.Draw();

            //  destroy the first enemy in the enemies list
            if (Input.anyKeyDown)
            {
                if (Enemy.enemies.Count > 0)
                    Enemy.enemies[0].Destroy();
            }
        }
    }
}