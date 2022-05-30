using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{

    public ParticleSystem m_successParticles;

    public ParticleSystem m_failParticles;

    private void Start( )
    {
        EventManager.m_eventManager.onSuccessfulSushi += PlaySuccessVFX;

        EventManager.m_eventManager.onUnsuccessfulSushi += PlayFailVFX;
    }

    public void PlaySuccessVFX( )
    {
        m_successParticles.Play( );
    }

    public void PlayFailVFX( )
    {
        m_failParticles.Play( );
    }

}
