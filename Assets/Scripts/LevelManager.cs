using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] float sceneLoadDelay = 2f;
    ScoreKeeper scoreKeeper;

    void Awake(){
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    public void LoadGame(){
        scoreKeeper.ResetScore();
        SceneManager.LoadScene("Game");
    }

    public void LoadMainMenu(){
        SceneManager.LoadScene("MainMenu");
        FindObjectOfType<GameSession>().ResetGameSession();
    }

    public void LoadInstructions(){
        StartCoroutine(WaitAndLoad("Instructions", sceneLoadDelay));
    }

    public void QuitGame(){
        Application.Quit();
    }
    
    IEnumerator WaitAndLoad(string sceneName, float delay){
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}

