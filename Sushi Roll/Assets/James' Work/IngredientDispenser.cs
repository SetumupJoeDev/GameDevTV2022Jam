using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientDispenser : MonoBehaviour
{

    [Tooltip("The transform position at which ingredients are placed when dispensed.")]
    public Transform m_dispensePoint;

    [Tooltip("The array of pooled ingredients to dispense.")]
    public GameObject[] m_ingredientPool;

    [Tooltip("The type of ingredient this dispenser should dispense.")]
    public EIngredient m_ingredientType;

    private void Start( )
    {
        //Subscribes the DispenseIngredient method to the onKeyPress event so that when the correct key is pressed, this dispenser will activate
        EventManager.m_eventManager.onKeyPress += DispenseIngredient;
    }

    public void DispenseIngredient( EIngredient ingredientName )
    {

        //Checks to see if the ingredient type passed in is the same as the type this dispenser dispenses
        if ( ingredientName == m_ingredientType )
        {
            //Loops through the pool of ingredient objects to find an inactive one
            foreach ( GameObject ingredient in m_ingredientPool )
            {
                //Upon finding an inactive ingredient, it sets its position to the dispense point and sets its velocity to zero before activating it
                if ( !ingredient.activeSelf )
                {
                    ingredient.transform.position = m_dispensePoint.position;

                    ingredient.GetComponentInChildren<Rigidbody>( ).velocity = Vector3.zero;

                    ingredient.SetActive( true );

                    //Returns out of the method to avoid spawning multiple ingredients
                    return;
                }
            }

        }

    }

}
