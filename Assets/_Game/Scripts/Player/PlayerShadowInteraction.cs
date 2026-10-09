using System.Collections.Generic;
using Shadowed.Input;
using UnityEngine;
using System;

[RequireComponent(typeof(PlayerInputReader))]
public class PlayerShadowInteraction : MonoBehaviour
{
    [SerializeField] private float minimumTargetScore = 0.7f;
    [SerializeField] private float attackSpeed = 10f;

    private readonly List<ShadowRange> nearbyShadows = new List<ShadowRange>();

    public IReadOnlyList<ShadowRange> NearbyShadows => nearbyShadows;
    public event Action<ShadowRange> OnShadowEquipped;

    private PlayerInputReader inputReader;
    private PlayerHealth playerHealth;

    private Vector2 aimDirection;
    private float bestScore = -1f;
    private float bestDistance;
    private  ShadowRange targetShadow;
    public  ShadowRange equippedShadow {get; private set;}
    private Vector3 attackAim;
    private bool normalAttackAllowed;

     

    private void Awake()
    {
        inputReader = GetComponent<PlayerInputReader>();
        playerHealth=GetComponent<PlayerHealth>();
    }
    void OnEnable()
    {
        playerHealth.OnDeath+=PlayerDeath;
    }
    void OnDisable()
    {
        playerHealth.OnDeath-=PlayerDeath;
    }
    private void Update()
    {
        aimDirection = inputReader.AimInput.normalized;
        
        if (aimDirection == Vector2.zero)
        {
            ClearTarget();
        }
        else
        {
           FindBestTarget(); 
        }

        
        GrabHandle();
        AttackHandle();
    }

    private void FindBestTarget()
    {
        bestDistance = Mathf.Infinity;
        if (targetShadow != null)
        {
            targetShadow.SetTargeted(false);
        }
        bestScore = -1f;
        targetShadow = null;

        foreach (ShadowRange shadow in nearbyShadows)
        {
            Vector2 toShadow = new Vector2(
    shadow.transform.position.x - transform.position.x,
    shadow.transform.position.y - transform.position.y
            );

            float distance = toShadow.magnitude;
            Vector2 directionToShadow = toShadow.normalized;

            float score = Vector2.Dot(aimDirection, directionToShadow);

            

            if (targetShadow == null)
                {
                    bestScore = score;
                    bestDistance = distance;
                    targetShadow = shadow;
                }
                else
                {
                    float scoreDifference = score - bestScore;

                    if (scoreDifference > 0.01f)
                    {
                        // Clearly better aim.
                        bestScore = score;
                        bestDistance = distance;
                        targetShadow = shadow;
                    }
                    else if (Mathf.Abs(scoreDifference) <= 0.01f && distance < bestDistance)
                    {
                        // Aim is basically tied, so choose the closer shadow.
                        bestScore = score;
                        bestDistance = distance;
                        targetShadow = shadow;
                    }
                }
                
        }

        if (bestScore < minimumTargetScore)
        {
            targetShadow = null;
        }
        if (targetShadow != null)
        {
            targetShadow.SetTargeted(true);
        }
        
    }

    private void ClearTarget()
    {
        if (targetShadow != null)
        {
            targetShadow.SetTargeted(false);
        }
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
    private void GrabHandle()
        {
            if (!inputReader.ConsumeGrabRequest())
                return;

            if (targetShadow != null)
            {
                EquipShadow(targetShadow);
            }
        }
    private void EquipShadow(ShadowRange shadow)
    {
        if(equippedShadow==null)
        {
           
        equippedShadow=shadow;
        equippedShadow.Take();
        OnShadowEquipped?.Invoke(equippedShadow);
        Debug.Log("Equipped Shadow : " + equippedShadow);
        
        }
        else
        {
            Debug.Log("a weapon is already equipped");
        }
    }
     private void AttackHandle()
        {
            if(!normalAttackAllowed)
            {
             return;
            }
            if (!inputReader.ConsumeAttackRequest())
            {
                return;
            }
            if(equippedShadow!=null)
            {
                Debug.Log("3 should happen");
                Attack();
            }
        }
    private void Attack()
    {
        
        
        {
            
            
            attackAim=new Vector3(aimDirection.x,aimDirection.y,0);
            equippedShadow.Throw(attackAim,attackSpeed);    
            equippedShadow=null;
        }
    }
    private void PlayerDeath()
    {
        ClearTarget();
        enabled=false;
    }
    public void ConsumeEquippedShadow()
    {
        if(equippedShadow==null)
        return;
        equippedShadow.enabled=false;
        equippedShadow=null;
        
        
    }
    public void NormalAttackEnabler()
    {
        normalAttackAllowed=true;
    }
}