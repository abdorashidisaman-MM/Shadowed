using System.Collections.Generic;
using UnityEngine;

public class PlayerStealth : MonoBehaviour
{
    private readonly HashSet<ShadowHideZone> activeHideZones = new();

    public event System.Action<bool> OnStealthChanged;

    public bool IsHidden => activeHideZones.Count > 0;

    public void EnterShadow(ShadowHideZone zone)
    {
        bool wasHidden = IsHidden;

        activeHideZones.Add(zone);

        Debug.Log("Hidden: " + IsHidden + " | Zones: " + activeHideZones.Count);

        if (wasHidden != IsHidden)
        {
            OnStealthChanged?.Invoke(IsHidden);
        }
    }

    public void ExitShadow(ShadowHideZone zone)
    {
        bool wasHidden = IsHidden;

        activeHideZones.Remove(zone);

        Debug.Log("Hidden: " + IsHidden + " | Zones: " + activeHideZones.Count);

        if (wasHidden != IsHidden)
        {
            OnStealthChanged?.Invoke(IsHidden);
        }
    }
}