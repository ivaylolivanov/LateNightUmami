using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Slot : MonoBehaviour {
    [SerializeField] Image ingredientImage;
    [SerializeField] Image hoverImage;
    [SerializeField] Image selectImage;
    [SerializeField] TMP_Text ingredientsCounter;

    private Ingredient ingredient;
    private int initialCount;
    private int count;

    public void SetIngredient(Ingredient newIngredient) {
        ingredient = newIngredient;
    }

    public Ingredient GetIngredient() {
        return ingredient;
    }

    public void SetCount(int count) {
        initialCount = count;
        this.count = initialCount;
    }

    public void Use() {
        if (count > 0) {
            selectImage.enabled = false;
            --count;
            count = Mathf.Clamp(count, 0, initialCount);
            Display();
        }
    }

    public bool IsDepleted() {
        return (count <= 0);
    }

    public void Display() {
        ingredientImage.sprite = ingredient.sprite;
        ingredientsCounter.text = count.ToString() + " / " + initialCount;
    }

    public void DisableSelectImage() {
        selectImage.enabled = false;
    }

    public void ToggleSelect() {
        if(IsDepleted()) {
            selectImage.enabled = false;
        }
        else {
            selectImage.enabled = !selectImage.enabled;
        }
    }

    public void Hovering() {
        hoverImage.enabled = true;
    }

    public void TurnOffHovering() {
        hoverImage.enabled = false;
    }
}
