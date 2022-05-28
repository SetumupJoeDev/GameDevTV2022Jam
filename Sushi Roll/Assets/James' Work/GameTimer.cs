using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{

    [Tooltip("The amount of time remaining on the timer.")]
    public float m_timeRemaining;

    [Tooltip("The game UI manager.")]
    public UIManager m_uiManager;

    public void RunTimer( )
    {
        //Reduces the amount of time remaining by the time passed this frame
        m_timeRemaining -= Time.deltaTime;

        //If the timer goes below zero, it is set to zero so the timer displays a normal value and the TimeUp event is called
        if( m_timeRemaining <= 0.0f )
        {

            m_timeRemaining = 0.0f;

            EventManager.m_eventManager.TimeUp( );
        }

        //The timer UI is updated to show the up-to-date value
        m_uiManager.UpdateTimerText( m_timeRemaining );

    }

}
