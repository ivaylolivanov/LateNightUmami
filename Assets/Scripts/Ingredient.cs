using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Ingredient")]
public class Ingredient : ScriptableObject {
    public string ingredient = "New ingredient";
    public Sprite sprite;
    public int flavor;
    public List<Ingredient> strongerTogether;

    public int Combine(Ingredient otherIngredient) {
        int result = flavor;
        if(strongerTogether.Contains(otherIngredient)) {
            result += otherIngredient.flavor;
        }
        else {
            result -= otherIngredient.flavor;
        }
        return result;
    }
}
