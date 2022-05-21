using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientDispenser : MonoBehaviour
{

    public Transform m_dispensePoint;

    public GameObject[] m_ingredientPool;

    public string m_ingredientName;

    private void Start( )
    {
        EventManager.m_eventManager.onKeyPress += DispenseIngredient;
    }

    public void DispenseIngredient( string ingredientName )
    {

        if ( ingredientName == m_ingredientName )
        {

            foreach ( GameObject ingredient in m_ingredientPool )
            {
                if ( !ingredient.activeSelf )
                {
                    ingredient.transform.position = m_dispensePoint.position;

                    ingredient.GetComponent<Rigidbody>( ).velocity = Vector3.zero;

                    ingredient.SetActive( true );

                    return;
                }
            }

        }

    }

}
