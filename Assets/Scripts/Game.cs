using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Game : MonoBehaviour
{
    [SerializeField] float dayDuration = 60.0f;
    [SerializeField] int maxIngredientsInRecipe = 3;
    [SerializeField] int startingRating = 30;
    [SerializeField] int clientsForFirstDay = 3;
    [SerializeField] List<Ingredient> allAvailableIngredients;
    [SerializeField] TMP_Text ratingText;
    [SerializeField] TMP_Text currentDayText;
    [SerializeField] TMP_Text nextDayTimerText;
    [SerializeField] TMP_Text currentRecipeFlavorText;
    [SerializeField] TMP_Text initialCountdownDisplayText;
    [SerializeField] GameObject inventory;
    [SerializeField] GameObject clientCreatorObject;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject instructionMenu;
    [SerializeField] float initialInstructionsDisplay = 30f;

    private int currentDay = 0;
    private float nextDayTime = 0;
    private float nextClientTime = 0;
    private int currentRating = 0;
    private int clientsForToday = 0;
    private int clientsServed = 0;
    private int currentClientFlavor = 0;
    private Inventory inventoryComponent;
    private ClientCreator clientCreator;

    private int selectedIngredientIndex = 0;
    private List<Slot> recipe;
    private int recipeFlavor = 0;

    private int highestRating = 0;

    void Start()
    {
        currentRating = startingRating;
        highestRating = startingRating;
        clientsForToday = clientsForFirstDay;
        inventoryComponent = inventory.GetComponent<Inventory>();
        recipe = new List<Slot>();
        clientCreator = clientCreatorObject.GetComponent<ClientCreator>();
        currentRecipeFlavorText.text = "0";
        nextDayTime = initialInstructionsDisplay;
        nextClientTime = initialInstructionsDisplay;
        instructionMenu.SetActive(true);
    }

    void Update() {
        GetUserInput();

        if (Time.time <= initialInstructionsDisplay)
        {
            int countdown = (int)(initialInstructionsDisplay - Time.time);
            initialCountdownDisplayText.text = countdown.ToString();
        }
        else
        {
            initialCountdownDisplayText.enabled = false;
        }

        if (Time.time >= nextDayTime)
        {
            if (!clientCreator.IsFull()) {
                currentRating -= clientCreator.GetRating();
            }

            instructionMenu.SetActive(false);
            nextDayTime = Time.time + dayDuration;

            LoadInventory();
            inventoryComponent.CalculateMaxFlavor();
            inventoryComponent.Display();

            clientsForToday = Random.Range(
                Mathf.Max(1, currentDay),
                currentDay + clientsForFirstDay
            );
            inventoryComponent.Clean();
            recipe.Clear();
            nextClientTime = Time.time;
            currentDayText.text = currentDay.ToString();
            ++currentDay;
        }

        if (Time.time >= nextClientTime)
        {
            // if (!clientCreator.IsFull())
            // {
            //     currentRating -= clientCreator.GetRating();
            // }
            clientCreator.CreateClient(
                inventoryComponent.GetMaxFlavor(),
                (int)nextDayTime / clientsForToday
            );
            currentClientFlavor = clientCreator.GetFlavorTreshhold();
            nextClientTime = Time.time + clientCreator.GetPatience();
        }
        clientCreator.UpdateCurrentPatience((int)(nextClientTime - Time.time));
        ratingText.text = currentRating.ToString() + " / " + highestRating.ToString();

        int time2NextDay = (int)(nextDayTime - Time.time);
        nextDayTimerText.text = time2NextDay.ToString();
    }

    public void SetNextDayTime(int newTime) {
        nextDayTime = newTime;
    }

    public void SetNextClientTime(int newTime) {
        nextClientTime = newTime;
    }

    public void SetInitialInstructionDelay(int delay) {
        initialInstructionsDisplay = delay;
    }

    public int GetRating() {
        return currentRating;
    }

    private void LoadInventory() {
        int ingredientsCount = inventoryComponent.GetNumberOfSlots();
        for (int i = 0; i < ingredientsCount; ++i) {
            int randomIndex = Random.Range(
                0,
                allAvailableIngredients.Count
            );
            Ingredient newIngredient = allAvailableIngredients[randomIndex];

            if(inventoryComponent.HasIngredient(newIngredient)) {
                --i;
                continue;
            }

            inventoryComponent.AddIngredient(newIngredient, clientsForToday);
        }
    }

    private void GetUserInput() {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        int previousSelection = selectedIngredientIndex;
        if (Input.anyKeyDown) {
            if (x > 0.01f) {
                if (selectedIngredientIndex % 2 == 0) {
                    ++selectedIngredientIndex;
                }
                else {
                    --selectedIngredientIndex;
                }
            }
            if (x < -0.01f) {
                if (selectedIngredientIndex % 2 == 0) {
                    ++selectedIngredientIndex;
                }
                else {
                    --selectedIngredientIndex;
                }
            }
            if (y > 0.01f) {
                selectedIngredientIndex -= 2;
            }
            if (y < -0.01f) {
                selectedIngredientIndex += 2;
            }

            if(selectedIngredientIndex < 0) {
                selectedIngredientIndex
                    += inventoryComponent.GetNumberOfSlots();
            }
            if(selectedIngredientIndex > inventoryComponent.GetNumberOfSlots() - 1) {
                selectedIngredientIndex
                    -= inventoryComponent.GetNumberOfSlots();
            }
        }

        if(previousSelection != selectedIngredientIndex) {
            inventoryComponent.UnhoverSlot(previousSelection);
        }

        Slot pointedSlot = inventoryComponent.HoverSlot(
            selectedIngredientIndex
        );

        if(Input.GetKeyDown(KeyCode.X)) {
            if(recipe.Contains(pointedSlot)) {
                recipe.Remove(pointedSlot);
            }
            else {
                if (!pointedSlot.IsDepleted()) {
                    if (recipe.Count >= maxIngredientsInRecipe) {
                        recipe[0].ToggleSelect();
                        recipe.RemoveAt(0);
                    }
                    recipe.Add(pointedSlot);
                }
            }

            CalculateRecipeFlavor();
            currentRecipeFlavorText.text = recipeFlavor.ToString();

            pointedSlot.ToggleSelect();
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            Cook();

            clientCreator.Feed(recipeFlavor);
            if (clientCreator.GetFlavorTreshhold() <= recipeFlavor) {
                currentRating += Mathf.Max(recipeFlavor, clientCreator.GetRating());
                ++clientsServed;
                if(currentRating > highestRating) {
                    highestRating = currentRating;
                }
            }

            if (!clientCreator.IsFull()) {
                currentRating -= clientCreator.GetRating();
                clientCreator.Feed(0);
            }

            recipeFlavor = 0;
            currentRecipeFlavorText.text = recipeFlavor.ToString();
        }

        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(mainMenu.activeSelf == true) {
                Time.timeScale = 1;
            }
            else {
                Time.timeScale = 0;
                instructionMenu.SetActive(false);
            }

            mainMenu.SetActive(!mainMenu.activeSelf);
        }
    }

    private void Cook() {
        CalculateRecipeFlavor(true);
        recipe.Clear();
    }

    private void CalculateRecipeFlavor(bool isCooking = false) {
        if(recipe.Count == 1) {
            recipeFlavor = recipe[0].GetIngredient().flavor;
            if (isCooking) {
                recipe[0].Use();
            }
            return;
        }
        int i;
        recipeFlavor = 0;
        for (i = 0; i < recipe.Count - 1; ++i) {
            recipeFlavor += recipe[i].GetIngredient().Combine(recipe[i+1].GetIngredient());

            if (isCooking) {
                recipe[i].Use();
            }
        }
        if (isCooking) {
            recipe[i].Use();
        }
    }
}
