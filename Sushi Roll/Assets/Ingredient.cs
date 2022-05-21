using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EIngredient : int
{
    Wasabi = 0,
    Cucumber,
    Avocardo,
    Salmon,
    Tofu,
    Tuna,
    PoppySeeds,
    PickledGinger,
    GreenOnion
}
public class Ingredient : MonoBehaviour
{

    public AudioClip m_hitSound;

    private void OnTriggerEnter( Collider other )
    {
        if( other.gameObject.name == "Crusher" )
        {
            gameObject.SetActive( false );
        }
    }

    private void OnCollisionEnter( Collision collision )
    {
        AudioSource.PlayClipAtPoint( m_hitSound, transform.position );
    }

}
