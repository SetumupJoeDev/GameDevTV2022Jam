using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [Tooltip("The text element used to display the current timer value.")]
    public TextMeshProUGUI m_timerText;

    [Tooltip("The text used to inform the player that their time us up.")]
    public TextMeshProUGUI m_timeUpText;


    private void Start( )
    {
        //Subscribes the TriggerTimeUpText method to the onTimeUp event
        EventManager.m_eventManager.onTimeUp += TriggerTimeUpText;
    }

    public void UpdateTimerText( float timeRemaining )
    {
        //Updates the timer display, rounding the value to 2 decimal places
        m_timerText.text = timeRemaining.ToString( "f2" );
    }

    public void TriggerTimeUpText( )
    {
        //Activates the TimeUp text
        m_timeUpText.gameObject.SetActive( true );

    }

}
