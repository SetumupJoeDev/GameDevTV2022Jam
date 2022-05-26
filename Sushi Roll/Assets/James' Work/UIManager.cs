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

    #region Menu Animators

    [Header("Menu Animators")]

    public Animator m_optionsMenu;

    public Animator m_mainMenu;

    public Animator m_levelSelect;

    public Animator m_pauseMenu;

    #endregion 

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

    public void ToggleCanvasGroup( bool enabled, CanvasGroup canvas )
    {
        if ( enabled )
        {
            //Sets the canvas' alpha to 1 so it is visible
            canvas.alpha = 1;

            //Sets the canvas to be interactable so any buttons or sliders can be used by the player
            canvas.interactable = true;
        }
        else
        {
            //Sets the canvas' alpha to 0 so it is no longer visible
            canvas.alpha = 0;

            //Sets the canvas to not be interactable so buttons and sliders cannot be used
            canvas.interactable = false;
        }
    }

    public void ExitMainMenu( )
    {
        m_mainMenu.SetTrigger( "ExitMenu" );
    }



}
