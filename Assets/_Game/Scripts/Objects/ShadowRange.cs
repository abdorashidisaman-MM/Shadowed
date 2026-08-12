using System;
using Unity.VisualScripting;
using UnityEngine;

public class ShadowRange : MonoBehaviour
{
    
    
    
    void OnTriggerEnter(Collider other)
    {
        int layerIndex = LayerMask.NameToLayer("Player");
        if(other.gameObject.layer == layerIndex)
        {
            Debug.Log("entered");
        }
    }
    void OnTriggerExit(Collider other)
    {
        int layerIndex = LayerMask.NameToLayer("Player");
        if(other.gameObject.layer == layerIndex)
        {
            Debug.Log("Exited");
        }
    }

}
