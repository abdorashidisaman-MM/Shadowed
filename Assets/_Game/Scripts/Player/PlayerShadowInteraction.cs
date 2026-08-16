using System.Collections.Generic;
using Shadowed.Input;
using UnityEngine;

[RequireComponent(typeof(PlayerInputReader))]
public class PlayerShadowInteraction : MonoBehaviour
{
    [SerializeField] private float minimumTargetScore = 0.7f;

    private readonly List<ShadowRange> nearbyShadows = new List<ShadowRange>();

    public IReadOnlyList<ShadowRange> NearbyShadows => nearbyShadows;

    private PlayerInputReader inputReader;

    private Vector2 aimDirection;
    private float bestScore = -1f;
    private ShadowRange targetShadow;

    private void Awake()
    {
        inputReader = GetComponent<PlayerInputReader>();
    }

    private void Update()
    {
        aimDirection = inputReader.AimInput.normalized;

        if (aimDirection == Vector2.zero)
        {
            ClearTarget();
            return;
        }

        FindBestTarget();
    }

    private void FindBestTarget()
    {
        bestScore = -1f;
        targetShadow = null;

        foreach (ShadowRange shadow in nearbyShadows)
        {
            Vector2 directionToShadow = new Vector2(
                shadow.transform.position.x - transform.position.x,
                shadow.transform.position.y - transform.position.y
            ).normalized;

            float score = Vector2.Dot(aimDirection, directionToShadow);

            if (score > bestScore)
            {
                bestScore = score;
                targetShadow = shadow;
            }
        }

        if (bestScore < minimumTargetScore)
        {
            targetShadow = null;
        }

        
    }

    private void ClearTarget()
    {
        bestScore = -1f;
        targetShadow = null;
    }

    public void AddShadow(ShadowRange shadow)
    {
        if (shadow == null)
            return;

        if (!nearbyShadows.Contains(shadow))
        {
            nearbyShadows.Add(shadow);
        }

        
    }

    public void RemoveShadow(ShadowRange shadow)
    {
        if (shadow == null)
            return;

        nearbyShadows.Remove(shadow);

        if (targetShadow == shadow)
        {
            ClearTarget();
        }

        
    }
}