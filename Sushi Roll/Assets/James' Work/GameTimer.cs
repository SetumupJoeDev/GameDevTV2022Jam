using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{

    public float m_timeRemaining;

    public UIManager m_uiManager;

    private void Update( )
    {

        m_timeRemaining -= Time.deltaTime;

        if( m_timeRemaining <= 0.0f )
        {

            m_timeRemaining = 0.0f;

            EventManager.m_eventManager.TimeUp( );
        }

        m_uiManager.UpdateTimerText( m_timeRemaining );

    }

}
