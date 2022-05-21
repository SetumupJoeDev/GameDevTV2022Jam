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

    public event Action<string> onKeyPress;

    public void OnKeyPress( string id )
    {
        if( onKeyPress != null )
        {
            onKeyPress( id );
        }
    }

}
