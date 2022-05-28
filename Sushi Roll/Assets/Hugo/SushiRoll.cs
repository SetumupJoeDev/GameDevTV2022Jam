using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiRoll : MonoBehaviour
{
    public List<MeshRenderer> SushiFillings = new List<MeshRenderer>();

    public void FillSushi(List<Material> materials)
    {
        for(int i = 0; i < SushiFillings.Count; i++)
        {
            SushiFillings[i].material = materials[i];
        }
    }
}
