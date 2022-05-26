using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    // Update is called once per frame
    public void AnimationComplete()
    {
        EventManager.m_eventManager.MenuAnimationComplete( );
    }

}
