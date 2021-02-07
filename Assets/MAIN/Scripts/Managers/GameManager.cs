using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public GameObject fader;
    string currentSceneName;
    Animator animator;
    bool gamePaused;

    private void Start()
    {
        SceneManager.sceneLoaded += HandlerSceneLoaded;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PreloadScene("Main");
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
    Application.Quit();
#endif
        }
        else if(Input.GetKeyDown(KeyCode.P))
        {
            // this scene uses input so no pause is required... 
            if (currentSceneName == "HighScore") return;

            gamePaused = !gamePaused;
            Time.timeScale = gamePaused ? 0 : 1;
        }
    }

    public void PreloadScene(string sceneName)
    {
        currentSceneName = sceneName;
        animator.SetTrigger("fadeOut");
        fader.transform.parent.gameObject.SetActive(true);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(currentSceneName);
    }

    private void HandlerSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fader.transform.parent.gameObject.SetActive(false);
    }
}
