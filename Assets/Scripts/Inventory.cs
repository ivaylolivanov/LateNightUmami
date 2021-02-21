using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    [SerializeField] List<GameObject> inventorySlotObjects;

    private int currentSlotIndex = 0;
    private List<Slot> slots;
    private Dictionary<Ingredient, Slot> inventory;

    private int maxFlavor;

    void Start() {
        slots = new List<Slot>();

        foreach(GameObject slotObj in inventorySlotObjects) {
            Slot slotComponent = slotObj.GetComponent<Slot>();
            if(slotComponent) {
                slots.Add(slotComponent);
            }
        }

        inventory = new Dictionary<Ingredient, Slot>();
        currentSlotIndex = 0;
        maxFlavor = 0;
    }

    public void AddIngredient(Ingredient ingredient, int quantity) {
        int randomizedQuantity = Random.Range(Mathf.Max(1,quantity - 5), quantity);
        slots[currentSlotIndex].SetIngredient(ingredient);
        slots[currentSlotIndex].SetCount(randomizedQuantity);
        slots[currentSlotIndex].DisableSelectImage();
        inventory.Add(ingredient, slots[currentSlotIndex]);

        ++currentSlotIndex;
    }

    public void CalculateMaxFlavor() {
        for (int i = 0; i < slots.Count; ++i ) {
            for (int j = 0; j < slots.Count; ++j) {
                if (i == j) { continue; }

                int currentFlavor = 0;
                for (int k = 0; k < slots.Count; ++k) {
                    if (k == j || k == i) { continue; }
                    currentFlavor =
                        slots[i].GetIngredient().Combine(slots[j].GetIngredient())
                        + slots[j].GetIngredient().Combine(slots[k].GetIngredient());
                    if(currentFlavor > maxFlavor) {
                        maxFlavor = currentFlavor;
                    }
                }
            }
        }
    }

    public int GetMaxFlavor() {
        return maxFlavor;
    }

    public bool HasIngredient(Ingredient ingredient) {
        return inventory.ContainsKey(ingredient);
    }

    public void Display() {
        foreach(Slot slot in slots) {
            slot.Display();
        }
    }

    public void UseIngredient(Ingredient ingredient) {
        if(HasIngredient(ingredient)) {
            inventory[ingredient].Use();

            if(inventory[ingredient].IsDepleted()) {
                inventory.Remove(ingredient);
            }
        }
    }

    public Slot HoverSlot(int index) {
        Slot result = null;

        if (index >= 0 && index < slots.Count) {
            slots[index].Hovering();
            result = slots[index];
        }

        return result;
    }

    public void UnhoverSlot(int index) {
        if (index >= 0 && index < slots.Count) {
            slots[index].TurnOffHovering();
        }
    }

    public int GetNumberOfSlots() {
        return slots.Count;
    }

    public void Clean() {
        inventory.Clear();
        currentSlotIndex = 0;
    }
}
