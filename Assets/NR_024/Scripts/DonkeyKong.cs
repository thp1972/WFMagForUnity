using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PygameZero;

namespace NR_024
{
    public class DonkeyKong : MonoBehaviour
    {
        public GameObject backgroundPrefab;
        GameObject background;


        List<Actor> barrels = new List<Actor>();
        Image platformMap;
        int spacer = 0;

        // Start is called before the first frame update
        void Start()
        {
            background = Instantiate(backgroundPrefab);

            platformMap = new Image();
            platformMap.Load("map");
        }

        private void Draw()
        {
            ScreenUtility.Blit(background, (0, 0));
        }

        // Update is called once per frame
        void Update()
        {
            Draw();
        }
    }
}