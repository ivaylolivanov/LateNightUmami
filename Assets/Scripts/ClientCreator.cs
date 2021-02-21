using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ClientCreator : MonoBehaviour {
    [SerializeField] private List<Sprite> bodies;
    [SerializeField] private List<Sprite> hairs;
    [SerializeField] private List<Sprite> availableEyes;
    [SerializeField] private List<Sprite> mouths;
    [SerializeField] private List<Sprite> shirts;

    [SerializeField] GameObject body;
    [SerializeField] GameObject hair;
    [SerializeField] GameObject eyes;
    [SerializeField] GameObject mouth;
    [SerializeField] GameObject shirt;

    [SerializeField] private int minPatience;

    [SerializeField] private GameObject thoughts;
    [SerializeField] private TMP_Text flavorText;
    [SerializeField] private TMP_Text patienceText;
    [SerializeField] private TMP_Text ratingText;

    private int flavorTreshhold;
    private int patience;
    private int rating;
    private bool isFull;

    void Start() {
        isFull = true;
        flavorTreshhold = 0;
        patience = 0;
        rating = 0;
    }

    public void CreateClient(int maxFlavor, int maxPatience) {
        isFull = false;
        flavorTreshhold = Random.Range(
            maxFlavor / 2,
            maxFlavor
        );
        patience = Random.Range(minPatience, maxPatience);
        rating = flavorTreshhold;

        thoughts.SetActive(true);

        ChooseRandomFrom(body, bodies);
        ChooseRandomFrom(hair, hairs);
        ChooseRandomFrom(eyes, availableEyes);
        ChooseRandomFrom(mouth, mouths);
        ChooseRandomFrom(shirt, shirts);

        flavorText.text = flavorTreshhold.ToString();
        patienceText.text = patience.ToString();
        ratingText.text = rating.ToString();
    }

    public void UpdateCurrentPatience(int remainingPatience) {
        patienceText.text = remainingPatience.ToString();
    }

    public int GetFlavorTreshhold() {
        return flavorTreshhold;
    }

    public int GetPatience() {
        return patience;
    }

    public int GetRating() {
        return rating;
    }

    public bool IsFull() {
        return isFull;
    }

    public void Feed(int recipeFlavor) {
        if (flavorTreshhold <= recipeFlavor) {
            isFull = true;
        }
        ExitShop();
    }

    private Sprite GetRandomSprite(List<Sprite> list) {
        int index = Random.Range(0, list.Count);
        return list[index];
    }

    private void ExitShop() {
        thoughts.SetActive(false);
        body.SetActive(false);
        hair.SetActive(false);
        eyes.SetActive(false);
        mouth.SetActive(false);
        shirt.SetActive(false);
    }

    private void ChooseRandomFrom(GameObject clientPart, List<Sprite> list) {
        clientPart.SetActive(true);
        Image image = clientPart.GetComponent<Image>();
        if(image) {
            image.sprite = GetRandomSprite(list);
        }
    }
}
