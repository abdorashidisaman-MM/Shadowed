using Shadowed.Input;
using UnityEngine;
using UnityEngine.Playables;
public class HairAttack : MonoBehaviour
{
    [SerializeField] private PlayableDirector hairExecution;
    [SerializeField] private PlayerInputReader playerInputReader;
    [SerializeField] private EnemyShadow Enemy2;
    [SerializeField] private ShadowSurfaceMotor shadowSurfaceMotor;
    private float damageAmount=100f;
    private bool hairAttackReady;
    void Update()
    {
        if(!hairAttackReady)
        return;
        if(!playerInputReader.SpecialRequest)
        return;
        StartHairExecution();
    }
    public void SpecialAttackReady()
    {
    hairAttackReady=true;
    }
    public void StartHairExecution()
    {
        hairExecution.Play();
    }
    public void HairImpact()
    {
        Enemy2.TakeDamage(damageAmount);
    }
    public void hairExecutionFinished()
    {
        hairAttackReady=false;
        shadowSurfaceMotor.enabled=true;
    }
    
}
