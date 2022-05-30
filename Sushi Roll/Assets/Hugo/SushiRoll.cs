using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiRoll : MonoBehaviour
{
    public List<MeshRenderer> SushiFillings = new List<MeshRenderer>();

    public bool m_success;

    public void FillSushi(List<Material> materials)
    {
        for(int i = 0; i < SushiFillings.Count; i++)
        {
            SushiFillings[i].material = materials[i];
        }
    }

    public void DisableSushi()
    {

        if ( m_success )
        {
            EventManager.m_eventManager.SuccessfulSushi( );
        }
        else
        {
            EventManager.m_eventManager.UnsuccessfulSushi( );
        }

        gameObject.SetActive(false);
    }
}
