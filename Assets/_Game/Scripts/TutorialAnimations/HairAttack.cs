using Shadowed.Input;
using UnityEngine;
using UnityEngine.Playables;
public class HairAttack : MonoBehaviour
{
    [SerializeField] private PlayableDirector hairExecution;
    [SerializeField] private PlayerInputReader playerInputReader;
    [SerializeField] private EnemyShadow Enemy2;
    [SerializeField] private ShadowSurfaceMotor shadowSurfaceMotor;
    [SerializeField] private Animator hairVisuals;
    private float damageAmount=100f;
    private bool hairAttackReady;
    private static readonly int HairAnimation =
        Animator.StringToHash("Hair");
    void Update()
    {
        if(!hairAttackReady)
            return;

        // Consume the press so it is used exactly once.
        if(!playerInputReader.ConsumeSpecialRequest())
            return;

        StartHairExecution();
    }
    public void SpecialAttackReady()
    {
        hairAttackReady=true;

        if (hairVisuals != null)
        {
            hairVisuals.SetTrigger(HairAnimation);
        }
    }
    public void StartHairExecution()
    {
        // Start once; previously Play() was called every frame
        // until the ExecutionFinished signal.
        hairAttackReady=false;
        hairExecution.Play();
    }
    public void HairImpact()
    {
        if (Enemy2 != null)
        {
            Enemy2.TakeDamage(damageAmount);
        }
    }
    public void hairExecutionFinished()
    {
        hairAttackReady=false;
        // Control is normally already returned by the RetrieveControl signal;
        // kept as a safety net.
        shadowSurfaceMotor.enabled=true;
    }
    
}
