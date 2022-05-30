using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{

    public float m_sfxVolume;

    private void Start( )
    {
        m_sfxVolume = 1;

        EventManager.m_eventManager.onSFXPlay += PlaySFX;
    }

    public void PlaySFX( AudioClip soundToPlay )
    {

        AudioSource.PlayClipAtPoint( soundToPlay , transform.position , m_sfxVolume );

    }

}
