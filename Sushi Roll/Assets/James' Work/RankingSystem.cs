using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingSystem : MonoBehaviour
{
    //The number of succesfully made recipes
    private int m_successfulRecipes;

    //The current level's initial time limit
    public float m_timeLimit;

    [Tooltip("The number of stars awarded to the player for the current level.")]
    public int m_numStarsAwarded;

    // Start is called before the first frame update
    void Start()
    {
        //Subscribes the IncrementTotalSuccesses method to the onRecipeSuccess event to keep track of the value
        EventManager.m_eventManager.onRecipeSuccess += IncrementTotalSuccesses;
    }

    
    public void IncrementTotalSuccesses( )
    {
        //Increases the number of successes by one
        m_successfulRecipes++;

    }

    public int CalculateRanking( )
    {
        //Divides the number of successes by the time limit to determine the number of successes per second
        float successPerSec = m_successfulRecipes / m_timeLimit;

        //Awards a star rating based on the results
        if( successPerSec > 0.5f )
        {
            //3 star rank
            m_numStarsAwarded = 3;
        }
        else if(successPerSec < 0.5f && successPerSec > 0.1f )
        {
            //2 star rank
            m_numStarsAwarded = 2;
        }
        else if( successPerSec < 0.1f )
        {
            //3 star rank
            m_numStarsAwarded = 1;
        }

        Debug.Log( "You got " + m_numStarsAwarded + " stars!" );

        return m_numStarsAwarded;

    }

}
