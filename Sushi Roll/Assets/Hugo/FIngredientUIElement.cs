using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FIngredientUIElement : MonoBehaviour
{
    public Image _IngredientNameText;
    public Image _IngredientIconImage;
    public CanvasGroup _CanvasGroup;

    public void PopulateIngredientInfo(Sprite text, Sprite icon)
    {
        _IngredientNameText.sprite = text;
        _IngredientIconImage.sprite = icon;
        _CanvasGroup.alpha = 1f;
    }

    public void HideIngredientInfo()
    {
        _CanvasGroup.alpha = 0f;
    }
}
