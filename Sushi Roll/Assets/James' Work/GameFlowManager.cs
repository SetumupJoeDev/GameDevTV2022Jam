using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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

    public int m_currentLevelIndex;

    public GameTimer m_gameTimer;

    public CanvasGroup m_timerCanvas;

    public UIManager m_uiManager;

    private bool m_loadingComplete = false;

    public RankingSystem m_rankingSystem;

    public GameObject m_levelRating;

    private int m_levelStarCount;

    #endregion

    #region Main Menu

    [Header("Main Menu")]

    public CanvasGroup m_mainMenuCanvas;

    public bool m_hasNavigated = false;

    #endregion

    public Slider m_musicVolumeSlider;

    public Slider m_sfxVolumeSlider;

    [Header("Music & Sound")]

    public AudioSource m_gameMusicSource;

    public SFXManager m_sxfManager;

    public AudioClip m_menuMusic;

    public AudioClip m_gameMusic;

    public AudioClip m_buttonPress;





    private void Start( )
    {

        EventManager.m_eventManager.onMenuAnimationComplete += EnterNewState;

        EventManager.m_eventManager.onTimeUp += TimeUp;

        m_currentGameState = GameState.inMainMenu;

        m_currentLevel = m_levelData[0];

    }

    public void LoadLevel( int levelIndex )
    {
        m_currentLevel = m_levelData[levelIndex];

        m_currentLevelIndex = levelIndex;

        EventManager.m_eventManager.SFXPlay( m_buttonPress );

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
                    if ( m_loadingComplete )
                    {
                        m_gameTimer.RunTimer( );
                    }

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
        
        EventManager.m_eventManager.SFXPlay( m_buttonPress );

    }

    public void EnterOptionsMenu( )
    {

        m_uiManager.ExitMainMenu( );

        m_currentGameState = GameState.inOptions;

        EventManager.m_eventManager.SFXPlay( m_buttonPress );

        m_hasNavigated = true;

    }

    public void EnterLevelSelect( )
    {
        m_uiManager.ExitMainMenu( );

        m_currentGameState = GameState.inLevelSelection;

        EventManager.m_eventManager.SFXPlay( m_buttonPress );

        m_hasNavigated = true;
    }

    public void PlayGame( )
    {

        m_uiManager.ExitMainMenu( );

        m_currentGameState = GameState.inGame;
        
        EventManager.m_eventManager.SFXPlay( m_buttonPress );

        m_hasNavigated = true;

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

                        m_rankingSystem.m_timeLimit = m_currentLevel.m_timeLimit;

                        m_gameTimer.m_timeRemaining = m_currentLevel.m_timeLimit;

                        List<EIngredient> availableIngredients = new List<EIngredient>();

                        foreach(EIngredient ingredient in m_currentLevel.m_availableIngredients )
                        {
                            availableIngredients.Add( ingredient );
                        }

                        m_sushiManager.BeginRound( availableIngredients );

                        m_loadingComplete = true;
                        break;
                    }
                case ( GameState.inLevelRating ):
                    {
                        m_levelRating.SetActive( true );
                        m_levelRating.GetComponent<Animator>( ).SetInteger( "StarCount", m_levelStarCount );
                        break;
                    }
            }
            
        }
    }

    public void UpdateMusicVolume( )
    {
        m_gameMusicSource.volume = m_musicVolumeSlider.value;
    }

    public void UpdateSFXVolume( )
    {
        m_sxfManager.m_sfxVolume = m_sfxVolumeSlider.value;
    }

    public void TimeUp( )
    {
        m_levelStarCount = m_rankingSystem.CalculateRanking( );

        m_sushiManager.EndRound( );

        m_gameMusicSource.Stop( );

        m_currentGameState = GameState.inLevelRating;

        m_currentLevelIndex++;

        if ( m_currentLevelIndex < m_levelData.Length )
        {
            m_currentLevel = m_levelData[m_currentLevelIndex];
        }
    }

    public void StartNextLevel( )
    {
        m_currentGameState = GameState.inGame;

        EventManager.m_eventManager.SFXPlay( m_buttonPress );

        m_gameMusicSource.Play( );

        m_rankingSystem.m_timeLimit = m_currentLevel.m_timeLimit;

        m_uiManager.HideTimeUpText( );
        
        m_gameTimer.m_timeRemaining = m_currentLevel.m_timeLimit;

        List<EIngredient> availableIngredients = new List<EIngredient>();

        foreach ( EIngredient ingredient in m_currentLevel.m_availableIngredients )
        {
            availableIngredients.Add( ingredient );
        }

        m_sushiManager.BeginRound( availableIngredients );
    }

}
