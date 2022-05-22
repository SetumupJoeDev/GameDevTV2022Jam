using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UTicketUI : MonoBehaviour
{
    [SerializeField]
    private FTicket _ticket;
    [SerializeField]
    private List<FIngredientUIElement> IngredientUIList = new List<FIngredientUIElement>();

    [SerializeField]
    private Data GameData;

    private Dictionary<EIngredient, Sprite> _TextureMap = new Dictionary<EIngredient, Sprite>();

    public void Initialise()
    {
        // CHANGE TO [number of ingredients] LATER
        foreach(FIngredientUIElement IngredientEntry in IngredientUIList)
        {
            IngredientEntry.HideIngredientInfo();
        }

        foreach (FIngredientSprites spriteMap in GameData.IngredientSprites)
        {
            _TextureMap.Add(spriteMap.Ingredient, spriteMap.Icon);
        }
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

        for (int i = 0; i < type.Count; i++)
        {
            string text = "x" + quantities[i].ToString();

            IngredientUIList[i].PopulateIngredientInfo(text, _TextureMap[type[i]]);
        }

        _ticket = Ticket;
    }

}
