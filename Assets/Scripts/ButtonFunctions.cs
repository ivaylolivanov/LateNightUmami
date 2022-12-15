using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour {
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject instructionsMenu;
    [SerializeField] GameObject gameObj;

    private Game game;
    void Start() {
        game = gameObj.GetComponent<Game>();
    }

    void Update() {
        if(game == null) return;

        if(game.GetRating() <= 0) {
            LoadEndGameScene();
        }
    }

    public void Instructions() {
        instructionsMenu.SetActive(true);
    }

    public void Play() {
        SceneManager.LoadScene(1);
    }

    public void PlayAgain() {
        SceneManager.LoadScene(0);
    }

    public void LoadEndGameScene() {
        SceneManager.LoadScene(3);
    }

    public void Quit() {
        Application.Quit();
    }

    public void Back() {
        game.SetNextDayTime((int)Time.time + 3);
        game.SetNextClientTime((int)Time.time + 3);
        game.SetInitialInstructionDelay((int)Time.time + 3);
        instructionsMenu.SetActive(false);
    }
}
