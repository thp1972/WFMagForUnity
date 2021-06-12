using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NRHandler : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.ForceSoftware;
    public Vector2 hotSpot = Vector2.zero;
    public NRDefinitions[] definitions;

    public static int PH_NUMBERS = 3;
    Transform[] placeHolders = new Transform[PH_NUMBERS];

    private void OnEnable()
    {
        EventBroker.OnGoToPage += HandlerOnGoToPage;
        EventBroker.OnSetCursor += SetCursor;
    }

    private void OnDisable()
    {
        EventBroker.OnGoToPage -= HandlerOnGoToPage;
        EventBroker.OnSetCursor -= SetCursor;
    }

    void Start()
    {
        // get the number of pages to navigate
        EventBroker.TriggerOnGetTotalPages((int)Mathf.Ceil((definitions.Length * 1f) / PH_NUMBERS));
        EventBroker.TriggerOnGetTotalNumbers(definitions.Length);
        PlaceholderInitialization(0);
    }

    void PlaceholderInitialization(int page)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            // there are only 3 placeholders but as many definitions as the issues of the magazine with the source code processed
            // this is important for a correct navigation
            int defCounter = PH_NUMBERS * page + i;

            var ph = transform.GetChild(i);
            ph.GetComponent<Image>().sprite = definitions[defCounter].cover;
            ph.transform.GetChild(0).GetComponent<Text>().text = definitions[defCounter].NrAndDate;

            var button = ph.GetComponent<Button>();
            button.spriteState = new SpriteState() { highlightedSprite = definitions[defCounter].coverSwap };

            if (definitions[defCounter].sceneName == "Empty")
            {
                button.onClick.RemoveAllListeners();
            }
            else
            {
                button.onClick.AddListener(() => { GameManager.Instance.PreloadScene(definitions[defCounter].sceneName); });
            }
        }
    }

    public void SetCursor(bool set)
    {
        Cursor.SetCursor(set ? cursorTexture : null, hotSpot, cursorMode);
    }

    private void HandlerOnGoToPage(int page)
    {
        PlaceholderInitialization(page);
    }
}
