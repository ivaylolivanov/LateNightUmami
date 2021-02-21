using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Storyteller : MonoBehaviour {
    [SerializeField] TMP_Text storyText;

    private string[] story;
    private int storyIndex;

    void Start() {
        storyIndex = 0;
        story = new string[6];
        story[0] = "You successfully opened a restaurant near a busy area.";
        story[1] = "Unexpected rush hour happens every day a short while before closing.";
        story[2] = "The restaurant embodies your idea to feed every client with the best dish possible.";
        story[3] = "Use the remaining ingredients to satisfy the flavor hunger of the incoming clients...";
        story[4] = "and do not let your rating go negative. Negative rating in busy area is deadly for a restaurant.";
        story[5] = "Good luck and most of all have fun!";
        storyText.text = story[storyIndex];
    }

    public void Next() {
        if(storyIndex < story.Length - 1) {
            storyText.text = story[++storyIndex];
        }
        else {
            SceneManager.LoadScene(2);
        }
    }

}
