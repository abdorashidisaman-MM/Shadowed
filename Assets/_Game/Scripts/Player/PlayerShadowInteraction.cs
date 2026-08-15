using System.Collections.Generic;
using UnityEngine;

public class PlayerShadowInteraction : MonoBehaviour
{
    private List <ShadowRange> nearbyShadows = new List<ShadowRange>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetCurrentShadow(ShadowRange shadow)
    {
        if (!nearbyShadows.Contains(shadow))
            {
                nearbyShadows.Add(shadow);
            }
        Debug.Log(shadow.ShadowName);
    }
    public void ClearCurrentShadow(ShadowRange shadow)
{
    if (nearbyShadows.Contains(shadow))
            {
                nearbyShadows.Remove(shadow);
            }
}
}
