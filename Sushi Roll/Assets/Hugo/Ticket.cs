using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class FTicket
{
    public EIngredient[] IngredientList = new EIngredient[3];
}

[System.Serializable]
public struct FIngredientSprites
{
    public EIngredient Ingredient;
    public Sprite Icon;
}