using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class FTicket
{
    public EIngredient[] IngredientList = new EIngredient[3];
}

[System.Serializable]
public struct FIngredientSprites
{
    public EIngredient Ingredient;
    public Texture Icon;
}

public class FIngredientUIElement
{
    public TMPro.TMP_Text _IngredientNameText;
    public Image _IngredientIconImage;
    public CanvasGroup _CanvasGroup;

    public void PopulateIngredientInfo(string text, Texture icon)
    {
        _IngredientNameText.text = text;
        _IngredientIconImage.image = icon;
        _CanvasGroup.alpha = 1f;
    }

    public void HideIngredientInfo()
    {
        _CanvasGroup.alpha = 0f;
    }
}

public struct FIngredientQuantity
{
    public EIngredient _Ingredient;
    public int _Number;
}