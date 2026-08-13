using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class ShadowRange : MonoBehaviour
{
    private LineRenderer connectionLine;
    private Transform playerTransform;

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
        }
    }

}
