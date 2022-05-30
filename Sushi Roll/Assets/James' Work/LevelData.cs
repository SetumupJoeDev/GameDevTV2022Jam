using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{

    [Tooltip("The level's time limit in seconds.")]
    public float m_timeLimit;

    [Tooltip("A list of all available ingredients in this level.")]
    public List<EIngredient> m_availableIngredients;

}
