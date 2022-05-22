using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameData", order = 1)]
public class Data : ScriptableObject
{
    // READ ONLY
    public List<FIngredientSprites> IngredientSprites = new List<FIngredientSprites>();
    public List<FInputBindings> InputBindings = new List<FInputBindings>();
    public int NumberOfIngredients = 3;
    public List<EIngredient> ActiveIngredients = new List<EIngredient>();
    public int NumberOfVisibleTickets = 4;
}
