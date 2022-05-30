using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    public string m_menuID;

    // Update is called once per frame
    public void AnimationComplete()
    {
        EventManager.m_eventManager.MenuAnimationComplete( m_menuID );
    }

}
