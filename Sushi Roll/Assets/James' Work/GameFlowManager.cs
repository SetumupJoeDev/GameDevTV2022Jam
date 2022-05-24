using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowManager : MonoBehaviour
{

    public enum GameState { inMainMenu, inGame, inLevelRating, inOptions, inLevelSelection, inPauseMenu };

    [Tooltip("The current state that the game is in.")]
    public GameState m_currentGameState;

    public EIngredient[] m_currentActiveIngredients;

    [Tooltip("An array that contains all of the different level data.")]
    public LevelData[] m_levelData;

    public GameTimer m_gameTimer;

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

}
