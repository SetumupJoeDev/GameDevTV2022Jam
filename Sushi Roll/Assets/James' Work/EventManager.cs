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

    public event Action onRecipeSuccess;

    public event Action<string> onMenuAnimationComplete;

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

    public void RecipeSuccess( )
    {
        if( onRecipeSuccess != null )
        {
            onRecipeSuccess( );
        }
    }

    public void MenuAnimationComplete( string id )
    {
        if( onMenuAnimationComplete != null )
        {
            onMenuAnimationComplete( id );
        }
    }

}
