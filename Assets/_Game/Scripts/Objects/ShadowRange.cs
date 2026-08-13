using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class ShadowRange : MonoBehaviour
{
    private LineRenderer connectionLine;
    private Transform playerTransform;
    public string ShadowName;
    private PlayerShadowInteraction Interaction;
    

    void Start()
    {
        connectionLine = GetComponent<LineRenderer>();
        connectionLine.positionCount=2;
        connectionLine.enabled=false;
        
        
    }
    void Update()
    {
        if(playerTransform!=null)
        {
             connectionLine.SetPosition(0, transform.position);
             connectionLine.SetPosition(1, playerTransform.position);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        int layerIndex = LayerMask.NameToLayer("Player");
        if(other.gameObject.layer == layerIndex)
        {
            Debug.Log("entered");
            playerTransform=other.transform;
            connectionLine.enabled=true;
           Interaction= other.GetComponent<PlayerShadowInteraction>();
           Interaction.SetCurrentShadow(this);
            
            
            
        }
    }
    void OnTriggerExit(Collider other)
    {
        int layerIndex = LayerMask.NameToLayer("Player");
        if(other.gameObject.layer == layerIndex)
        {
            Debug.Log("Exited");
            connectionLine.enabled=false;
            playerTransform = null;
            Interaction= other.GetComponent<PlayerShadowInteraction>();
            Interaction.ClearCurrentShadow(this);
            
        }
    }

}
