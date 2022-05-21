using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReader : MonoBehaviour
{

    public static InputReader current;

    private void Awake( )
    {
        current = this;
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.anyKeyDown )
        {
            if ( Input.GetKeyDown( KeyCode.Alpha1 ) )
            {
                EventManager.m_eventManager.OnKeyPress( "Rice" );
            }
            else if ( Input.GetKeyDown( KeyCode.Alpha2 ) )
            {
                EventManager.m_eventManager.OnKeyPress( "Fish" );
            }
            else if( Input.GetKeyDown( KeyCode.Alpha3 ) )
            {
                EventManager.m_eventManager.OnKeyPress( "Nori" );
            }
        }    
    }
}
