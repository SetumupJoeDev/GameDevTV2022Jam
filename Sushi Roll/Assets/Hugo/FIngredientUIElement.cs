using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FIngredientUIElement : MonoBehaviour
{
    public TMPro.TMP_Text _IngredientNameText;
    public Image _IngredientIconImage;
    public CanvasGroup _CanvasGroup;

    public void PopulateIngredientInfo(string text, Sprite icon)
    {
        _IngredientNameText.text = text;
        _IngredientIconImage.sprite = icon;
        _CanvasGroup.alpha = 1f;
    }

    public void HideIngredientInfo()
    {
        _CanvasGroup.alpha = 0f;
    }
}
