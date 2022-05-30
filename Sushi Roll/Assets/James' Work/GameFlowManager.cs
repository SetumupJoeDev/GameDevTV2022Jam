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

    public SushiManager m_sushiManager;

    [Tooltip("An array that contains all of the different level data.")]
    public LevelData[] m_levelData;

    public LevelData m_currentLevel;

    public GameTimer m_gameTimer;

    public CanvasGroup m_timerCanvas;

    public UIManager m_uiManager;

    #endregion

    #region Main Menu

    [Header("Main Menu")]

    public CanvasGroup m_mainMenuCanvas;

    public bool m_hasNavigated = false;

    #endregion

    [Header("Music")]

    public AudioSource m_gameMusicSource;

    public AudioClip m_menuMusic;

    public AudioClip m_gameMusic;



    private void Start( )
    {

        EventManager.m_eventManager.onMenuAnimationComplete += EnterNewState;

        m_currentGameState = GameState.inMainMenu;

    }

    public void LoadLevel( int levelIndex )
    {
        m_currentLevel = m_levelData[levelIndex];

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

        m_currentGameState = GameState.inLevelSelection;

        m_hasNavigated = true;
    }

    public void PlayGame( )
    {

        m_uiManager.ExitMainMenu( );

        m_currentGameState = GameState.inGame;

    }

    private void EnterNewState( string id )
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
                case ( GameState.inGame ):
                    {
                        m_gameMusicSource.clip = m_gameMusic;

                        m_gameMusicSource.Play( );

                        m_uiManager.ToggleCanvasGroup( true , m_timerCanvas );

                        m_sushiManager.BeginRound( m_currentLevel.m_availableIngredients );
                        break;
                    }
            }
            
        }
    }

}
