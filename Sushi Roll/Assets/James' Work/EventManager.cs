using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    public static EventManager m_eventManager;

    private void Awake( )
    {
        m_eventManager = this;
    }

    public event Action<EIngredient> onKeyPress;

    public event Action onTimeUp;

    public void KeyPress( EIngredient id )
    {
        if( onKeyPress != null )
        {
            onKeyPress( id );
        }
    }

    public void TimeUp( )
    {
        if ( onTimeUp != null )
        {
            onTimeUp( );
        }
    }

}
