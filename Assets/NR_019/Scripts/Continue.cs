using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UTILITY;

public class Continue : MonoBehaviour
{
    EventsDetector ev;
    Draw draw;
    Text text1, text2, text3;

    class State
    {
        // a dictionary containing possible states,
        // and rules for moving to them
        public Dictionary<State, Func<bool>> rules;
        public Action Draw = () => { };
        public Action Update = () => { };
        public State()
        {
            rules = new Dictionary<State, Func<bool>>();
        }

        // adds a state:rule pair to the 'rules' dictionary
        public void AddRule(State state, Func<bool> rule)
        {
            rules[state] = rule;
        }
    }

    class StateMachine
    {
        public State current;
        public float frame;

        public void Update()
        {
            // only update current state if one exists
            if (current == null) return;
            // increment the frame
            frame += 1 * Time.deltaTime;
            // if any rule evaluates to 'True' then
            // switch to that state (and reset the timer)
            foreach (var sr in current.rules)
            {
                var s = sr.Key;
                var r = sr.Value;
                if (r())
                {
                    current = s;
                    frame = 0;
                }
            }
            // call the current state's update() method
            current.Update();
        }

        public void Draw()
        {
            if (current == null) return;
            current.Draw();
        }
    }
    StateMachine sm = new StateMachine();

    // Start is called before the first frame update
    void Start()
    {
        ev = FindObjectOfType<EventsDetector>();
        draw = GetComponent<Draw>();
        text1 = draw.CreateText(40, (255, 255, 255), isActive: false);
        text2 = draw.CreateText(40, (255, 255, 255), isActive: false);
        text3 = draw.CreateText(40, (255, 255, 255), isActive: false);

        // pygame stuff
        var tileScreen = new State();
        tileScreen.Draw = DrawTitle;

        var gameScreen = new State();
        gameScreen.Draw = DrawGame;

        var continueScreen = new State();
        continueScreen.Draw = DrawContinue;

        tileScreen.AddRule(gameScreen, () => EventsDetector.Keyboard.Space);
        gameScreen.AddRule(continueScreen, () => EventsDetector.Keyboard.E);
        continueScreen.AddRule(tileScreen, () => sm.frame >= 10);
        continueScreen.AddRule(gameScreen, () => EventsDetector.Keyboard.Space);

        sm.current = tileScreen;
    }

    void Draw()
    {
        Clear();
        sm.Draw();
    }

    // Update is called once per frame
    void Update()
    {
        sm.Update();
        Draw();
    }

    void DrawTitle()
    {
        draw.DrawTex(text1, (50, 50), text: "Title screen");
        draw.DrawTex(text2, (50, 80), text: "Press [space] to start");
    }

    void DrawGame()
    {
        draw.DrawTex(text1, (50, 50), text: "Game screen");
        draw.DrawTex(text2, (50, 80), text: "Press [e] to end game");
    }

    void DrawContinue()
    {
        draw.DrawTex(text1, (50, 50), text: "Continue  screen");
        draw.DrawTex(text2, (50, 80), text: "Press [space] to play again");
        // display the time remaining until 10 seconds have passed
        draw.DrawTex(text3, (50, 110), text: $"{(int)((10 - sm.frame)) + 1}");
    }

    void Clear()
    {
        text1.gameObject.SetActive(false);
        text2.gameObject.SetActive(false);
        text3.gameObject.SetActive(false);
    }

}
