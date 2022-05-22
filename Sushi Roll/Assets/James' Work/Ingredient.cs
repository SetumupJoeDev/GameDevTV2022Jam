using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EIngredient : int
{
    Cucumber = 0,
    SpringOnion,
    Avocado,
    Eel,
    Salmon,
    Tuna,
    Mayo,
    Tofu,
    Egg
}

public class Ingredient : MonoBehaviour
{
    private void OnTriggerEnter( Collider other )
    {
        if( other.gameObject.name == "Crusher" )
        {
            gameObject.SetActive( false );
        }
    }

}
