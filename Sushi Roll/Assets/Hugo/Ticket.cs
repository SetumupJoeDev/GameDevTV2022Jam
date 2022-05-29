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

[System.Serializable]
public struct FIngredientMaterials
{
    public EIngredient Ingredient;
    public Material IngredientMaterial;
}

[System.Serializable]
public struct FInputBindings
{
    public EIngredient Ingredient;
    public KeyCode InputKey;
}
