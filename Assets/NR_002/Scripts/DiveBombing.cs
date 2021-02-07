using System;
using System.Collections;
using UnityEngine;

// By Pellegrino ~thp~ Principe
namespace NR_002
{
    public class DiveBombing : MonoBehaviour
    {
        readonly public int WIDTH = 400;
        readonly public int HEIGHT = 800;

        public GameObject shipPrefab;
        Ship ship;

        // Start is called before the first frame update
        void Awake()
        {
#if !UNITY_EDITOR
        Screen.SetResolution(WIDTH, HEIGHT, true);
#endif
            ship = Instantiate(shipPrefab).GetComponent<Ship>();
            StartCoroutine(Shedule(ship.StartDive, 3));
        }

        public IEnumerator Shedule(Action callback, float sec)
        {
            yield return new WaitForSeconds(sec);
            callback();
        }
    }
}

