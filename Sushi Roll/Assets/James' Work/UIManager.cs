using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public TextMeshProUGUI m_timerText;

    public TextMeshProUGUI m_timeUpText;

    public Animator m_timeUpAnimator;

    private void Start( )
    {
        EventManager.m_eventManager.onTimeUp += TriggerTimeUpText;
    }

    public void UpdateTimerText( float timeRemaining )
    {
        m_timerText.text = timeRemaining.ToString( "f2" );
    }

    public void TriggerTimeUpText( )
    {

        m_timeUpText.gameObject.SetActive( true );

    }

}
