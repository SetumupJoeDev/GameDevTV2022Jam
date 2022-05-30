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

    #region Menu Canvas Groups

    public CanvasGroup m_mainMenuCanvas;

    public CanvasGroup m_optionsMenuCanvas;

    public CanvasGroup m_levelSelectCanvas;

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

    public void EnterMainMenu( )
    {
        m_mainMenuCanvas.alpha = 1;
        m_mainMenuCanvas.interactable = true;
        m_mainMenuCanvas.blocksRaycasts = true;

        m_optionsMenu.ResetTrigger( "EnterMenu" );
        m_mainMenu.SetTrigger( "EnterMenu" );
    }

    public void EnterOptionsMenu( )
    {
        m_optionsMenuCanvas.alpha = 1;
        m_optionsMenuCanvas.interactable = true;
        m_optionsMenuCanvas.blocksRaycasts = true;

        m_mainMenu.ResetTrigger( "ExitMenu" );
        m_optionsMenu.SetTrigger( "EnterMenu" );
    }

    public void ExitOptionsMenu( )
    {
        m_optionsMenu.SetTrigger( "ExitMenu" );
    }

    public void EnterLevelSelect( )
    {
        m_levelSelectCanvas.alpha = 1;
        m_levelSelectCanvas.interactable = true;
        m_levelSelectCanvas.blocksRaycasts = true;

        m_levelSelect.SetTrigger( "EnterMenu" );
    }

    public void ExitLevelSelect( )
    {
        m_levelSelect.SetTrigger( "ExitMenu" );
    }

    public void HideCanvas( string id )
    {
        switch ( id )
        {
            case ( "MainMenu" ):
                {
                    m_mainMenuCanvas.alpha = 0;
                    m_mainMenuCanvas.interactable = false;
                    m_mainMenuCanvas.blocksRaycasts = false;
                    break;
                }
            case ( "OptionsMenu" ):
                {
                    m_optionsMenuCanvas.alpha = 0;
                    m_optionsMenuCanvas.interactable = false;
                    m_optionsMenuCanvas.blocksRaycasts = false;
                    break;
                }
            case ( "LevelSelect" ):
                {
                    m_levelSelectCanvas.alpha = 0;
                    m_levelSelectCanvas.interactable = false;
                    m_levelSelectCanvas.blocksRaycasts = false;
                    break;
                }
        }
    }

}
