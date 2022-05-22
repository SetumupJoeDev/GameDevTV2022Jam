using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FTicket
{
    public List<EIngredient> IngredientList = new List<EIngredient>();
}

[System.Serializable]
public struct FIngredientSprites
{
    public EIngredient Ingredient;
    public Sprite Icon;
}