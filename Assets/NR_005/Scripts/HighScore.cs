using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// By Pellegrino ~thp~ Principe
namespace NR_005
{
    public class HighScore : MonoBehaviour
    {
        public GameObject inputPlaceholder;
        public GameObject inputFieldPrefab;
        public GameObject tablePlaceholder;
        public GameObject textToShowPrefab;

        List<(int, string)> highscores;

        GameObject inputField;

        int score;
        int[] scores = { 64, 30, 87 };
        int currentIndexScores;

        // Start is called before the first frame update
        void Start()
        {
            // highscore list is initially filled with low scores
            highscores = Enumerable.Range(0, 10).Select((i) => (0, "Player")).ToList();

            // I can't use addscore(64) addscore(30) addscore(87) because Unity is event driven...
            CreateInputField();
        }


        // get the player's name
        public void AddScore(string inputValue)
        {
            // only add the score if it is greater than the
            // current lowest score in the highscores list
            if (score < highscores[9].Item1)
            {
                currentIndexScores++;
                return;
            }

            // starting at 0, increment the 'pos' variable
            // until it's at the position to insert the score
            int pos = 0;
            while (pos < highscores.Count && score <= highscores[pos].Item1)
            {
                pos += 1;
            }

            // add the (score, name) tuple
            // at the correct place in the list
            highscores.Insert(pos, (score, inputValue));

            // only store the top 10 scores in the list
            highscores.RemoveAt(10);

            currentIndexScores++;
            if (currentIndexScores >= scores.Length) // no more scores; return
            {
                DrawTableText();
                DrawTableGame();
                inputField.GetComponent<InputField>().onEndEdit.RemoveListener(AddScore);
                return;
            }

            CreateInputField();
        }

        // prints the table to standard output - in Unity to the console
        void DrawTableText()
        {
            print("Score\tName");
            foreach (var s in highscores)
            {
                print($"{s.Item1}\t{s.Item2}");
            }
        }

        // prints the table with Unity UI
        void DrawTableGame()
        {
            tablePlaceholder.SetActive(true);

            foreach (var s in highscores)
            {
                var textToShowScore = Instantiate(textToShowPrefab);
                var textToShowName = Instantiate(textToShowPrefab);
                textToShowScore.transform.SetParent(tablePlaceholder.transform);
                textToShowScore.GetComponent<Text>().text = s.Item1.ToString();
                textToShowName.transform.SetParent(tablePlaceholder.transform);
                textToShowName.GetComponent<Text>().text = s.Item2;
            }
        }

        void CreateInputField()
        {
            score = scores[currentIndexScores];
            inputField = Instantiate(inputFieldPrefab);
            inputField.transform.SetParent(inputPlaceholder.transform);
            var iField = inputField.GetComponent<InputField>();
            iField.onEndEdit.AddListener(AddScore);
            iField.ActivateInputField(); // set focus automatically!
        }
    }
}