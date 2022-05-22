using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EIngredient : int
{
    Wasabi = 0,
    Cucumber,
    Avocardo,
    Salmon,
    Tofu,
    Tuna,
    PoppySeeds,
    PickledGinger,
    GreenOnion
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
