using System.Collections.Generic;
using UnityEngine;

public class PlayerShadowInteraction : MonoBehaviour
{
    private readonly List<ShadowRange> nearbyShadows = new List<ShadowRange>();

    public IReadOnlyList<ShadowRange> NearbyShadows => nearbyShadows;

    
    public void AddShadow(ShadowRange shadow)
    {
        if (shadow == null) return;

        if (!nearbyShadows.Contains(shadow))
        {
            nearbyShadows.Add(shadow);
        }

        Debug.Log(shadow.ShadowName);
        Debug.Log("Nearby shadows: " + nearbyShadows.Count);
    }

    public void RemoveShadow(ShadowRange shadow)
    {
        if (shadow == null) return;

        if (nearbyShadows.Contains(shadow))
        {
            nearbyShadows.Remove(shadow);
        }
        Debug.Log("Nearby shadows: " + nearbyShadows.Count);
    }
}

