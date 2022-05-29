using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{

    #region Game Flow

    public enum GameState { inMainMenu, inGame, inLevelRating, inOptions, inLevelSelection, inPauseMenu };

    [Tooltip("The current state that the game is in.")]
    public GameState m_currentGameState;

    public EIngredient[] m_currentActiveIngredients;

    [Tooltip("An array that contains all of the different level data.")]
    public LevelData[] m_levelData;

    public GameTimer m_gameTimer;

    public UIManager m_uiManager;

    #endregion

    #region Main Menu

    [Header("Main Menu")]

    public CanvasGroup m_mainMenuCanvas;

    #endregion

    public bool m_hasNavigated = false;

    private void Start( )
    {

        EventManager.m_eventManager.onMenuAnimationComplete += EnterNewMenu;

        m_currentGameState = GameState.inMainMenu;

    }

    public void LoadLevel(LevelData levelToLoad )
    {
        //Sets the time remaining on the timer to the time limit of the level data passed in
        m_gameTimer.m_timeRemaining = levelToLoad.m_timeLimit;

        //Sets the available ingredients to that of the level data passed in
        m_currentActiveIngredients = levelToLoad.m_availableIngredients;

    }

    private void Update( )
    {
        switch ( m_currentGameState )
        {
            case GameState.inMainMenu:
                {
                    break;
                }
            case GameState.inOptions:
                {
                    break;
                }
            case GameState.inLevelSelection:
                {
                    break;
                }
            case GameState.inLevelRating:
                {
                    break;
                }
            case GameState.inGame:
                {

                    m_gameTimer.RunTimer( );

                    break;
                }
            case GameState.inPauseMenu:
                {

                    break;
                }
        }
    }


    public void EnterMainMenu( )
    {

        switch ( m_currentGameState )
        {
            case ( GameState.inOptions ):
                {
                    m_uiManager.ExitOptionsMenu( );
                    break;
                }
            case ( GameState.inLevelSelection ):
                {
                    m_uiManager.ExitLevelSelect( );
                    break;
                }
            case ( GameState.inPauseMenu ):
                {
                    //Do stuff
                    break;
                }
            default:
                {
                    Debug.LogError( "No transition found!" );
                    break;
                }
        }

        m_currentGameState = GameState.inMainMenu;

    }

    public void EnterOptionsMenu( )
    {

        m_uiManager.ExitMainMenu( );

        m_currentGameState = GameState.inOptions;

        m_hasNavigated = true;

    }

    public void EnterLevelSelect( )
    {
        m_uiManager.ExitMainMenu( );

        m_hasNavigated = true;
    }

    private void EnterNewMenu( string id )
    {
        if ( m_hasNavigated )
        {

            switch ( m_currentGameState )
            {
                case ( GameState.inMainMenu ):
                    {
                        m_uiManager.EnterMainMenu( );
                        break;
                    }
                case ( GameState.inOptions ):
                    {
                        m_uiManager.EnterOptionsMenu( );
                        break;
                    }
                case ( GameState.inLevelSelection ):
                    {
                        m_uiManager.EnterLevelSelect( );
                        break;
                    }
            }
        }
    }

}
