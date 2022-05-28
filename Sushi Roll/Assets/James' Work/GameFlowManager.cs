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

    [Space]

    #endregion

    #region Options Menu
    [Header("Options Menu")]

    public CanvasGroup m_optionsMenuCanvas;

    [Space]

    #endregion

    #region Level Select
    [Header("Level Select Menu")]

    public CanvasGroup m_levelSelectCanvas;

    [Space]

    #endregion

    #region Pause Menu

    [Header("Pause Menu")]

    public CanvasGroup m_pauseMenuCanvas;

    #endregion

    private void Start( )
    {

        CanvasGroup[] menuCanvases = new CanvasGroup[]{m_optionsMenuCanvas, m_levelSelectCanvas, m_pauseMenuCanvas};

        foreach(CanvasGroup canvas in menuCanvases )
        {
            m_uiManager.ToggleCanvasGroup( false , canvas );
        }

        m_uiManager.ToggleCanvasGroup( true , m_mainMenuCanvas );

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

        

    }

    public void EnterOptionsMenu( )
    {

        m_uiManager.ExitMainMenu( );

        m_currentGameState = GameState.inOptions;

    }

    public void EnterNewMenu( )
    {
        switch ( m_currentGameState )
        {
            case GameState.inMainMenu:
                {

                    break;
                }
            case GameState.inOptions:
                {
                    //m_uiManager.EnterOptionsMenu( );
                    break;
                }
            case GameState.inLevelSelection:
                {

                    break;
                }
            case GameState.inPauseMenu:
                {

                    break;
                }
        }
    }

}
