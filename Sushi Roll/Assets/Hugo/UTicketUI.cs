using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UTicketUI : MonoBehaviour
{
    private FTicket _ticket;
    private List<FIngredientUIElement> IngredientUIList = new List<FIngredientUIElement>();

    [SerializeField]
    public List<FIngredientSprites> IngredientSprites = new List<FIngredientSprites>();
    private Dictionary<EIngredient, Texture> _TextureMap = new Dictionary<EIngredient, Texture>();

    public void Initialise()
    {
        // CHANGE TO [number of ingredients] LATER

        foreach (FIngredientSprites spriteMap in IngredientSprites)
        {
            _TextureMap.Add(spriteMap.Ingredient, spriteMap.Icon);
        }
    }

    public void AssignTicket(FTicket Ticket)
    {
        List<EIngredient> type = new List<EIngredient>();
        List<int> quantities = new List<int>();

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
                quantities[index] = quantities[index]++;
            }
        }

        for (int i = 0; i < type.Count; i++)
        {
            string text = "X" + quantities[i].ToString();

            IngredientUIList[i].PopulateIngredientInfo(text, _TextureMap[type[i]]);
        }

        _ticket = Ticket;
    }

}
