using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameData", order = 1)]
public class Data : ScriptableObject
{
    // READ ONLY
    [Header("Relationships")]
    public List<FIngredientSprites> IngredientSprites = new List<FIngredientSprites>();
    public List<FInputBindings> InputBindings = new List<FInputBindings>();
    public List<FIngredientMaterials> IngredientMaterials = new List<FIngredientMaterials>();
    public List<Sprite> InputQuantities = new List<Sprite>();

    [Header("Quantities")]
    public int NumberOfVisibleTickets = 4;
    public int NumberOfIngredients = 3;
    public int NumberOfPossibleIngredients = 4;
    
    [Header("Obsolete for now")]
    public List<EIngredient> ActiveIngredients = new List<EIngredient>();

}
