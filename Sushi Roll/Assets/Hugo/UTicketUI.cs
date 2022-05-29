using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UTicketUI : MonoBehaviour
{
    private FTicket _ticket;
    [SerializeField]
    private List<FIngredientUIElement> IngredientUIList = new List<FIngredientUIElement>();

    [SerializeField]
    private Data GameData;

    [SerializeField]
    private CanvasGroup TicketCG;

    [HideInInspector]
    public int IndexInList = 0;

    private Dictionary<EIngredient, Sprite> _TextureMap = new Dictionary<EIngredient, Sprite>();

    public void Initialise(int Index)
    {
        TicketCG.blocksRaycasts = false;
        TicketCG.interactable = false;
        TicketCG.alpha = 1f;

        foreach (FIngredientUIElement IngredientEntry in IngredientUIList)
        {
            IngredientEntry.HideIngredientInfo();
        }

        foreach (FIngredientSprites spriteMap in GameData.IngredientSprites)
        {
            _TextureMap.Add(spriteMap.Ingredient, spriteMap.Icon);
        }

        IndexInList = Index;
    }

    public void AssignTicket(FTicket Ticket)
    {
        List<EIngredient> type = new List<EIngredient>();
        List<int> quantities = new List<int>();

        foreach(FIngredientUIElement UI in IngredientUIList)
        {
            UI.HideIngredientInfo();
        }

        // this is dumb

        foreach (EIngredient ingredient in Ticket.IngredientList)
        {
            if (!type.Contains(ingredient))
            {
                type.Add(ingredient);
                quantities.Add(1);
            }
            else
            {
                int index = type.IndexOf(ingredient);
                quantities[index] = quantities[index] + 1;
            }
        }

        int bDisplayKeyOnTicket = PlayerPrefs.GetInt("DisplayKey", 0);

        for (int i = 0; i < type.Count; i++)
        {
            string text = "x " + quantities[i].ToString();
            if(bDisplayKeyOnTicket == 1)
            {
                text += " | " + ((int)type[i] + 1);
            }

            IngredientUIList[i].PopulateIngredientInfo(text, _TextureMap[type[i]]);
        }

        _ticket = Ticket;
    }

    public void Hide()
    {
        TicketCG.alpha = 0f;
    }

}
