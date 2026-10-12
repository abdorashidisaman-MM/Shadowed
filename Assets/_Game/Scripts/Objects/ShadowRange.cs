using System.Collections;
using UnityEngine;

public class ShadowRange : MonoBehaviour
{
    [SerializeField] private Color normalLineColor = Color.white;
    [SerializeField] private Color targetedLineColor = Color.yellow;
    [SerializeField] private Collider interactionCollider;
    [SerializeField] private Collider projectileCollider;
    [SerializeField] private float fadeDuration = 1f;
    
   

    
    private Color color;
    private MeshRenderer shadowRenderer;
    public string ShadowName;

    public LineRenderer connectionLine;
    private Material lineMaterial;
    private Transform playerTransform;
    private PlayerShadowInteraction interaction;
    private Rigidbody shadowRigidBody;
    private int playerLayerIndex;
    private int environmentLayerIndex;
    [SerializeField]
    private WeaponAnimation weaponVisuals;
    
    
    private enum ShadowState
    {
        World,
        Equipped,
        Thrown,
        Stuck
    }
    private ShadowState currentState;
    
  
    
    
    

    private void Awake()
    {
        shadowRenderer=GetComponent<MeshRenderer>();
        shadowRigidBody=GetComponent<Rigidbody>();
        playerLayerIndex = LayerMask.NameToLayer("Player");
        environmentLayerIndex=LayerMask.NameToLayer("Environment");
        connectionLine = GetComponent<LineRenderer>();
        currentState=ShadowState.World;
        
        if (connectionLine != null)
        {
            connectionLine.positionCount = 2;
            connectionLine.enabled = false;
        }
        lineMaterial=connectionLine.material;
        
    }

    private void Update()
    {
        if (connectionLine != null && playerTransform != null)
        {
            connectionLine.SetPosition(0, transform.position);
            connectionLine.SetPosition(1, playerTransform.position);
        }
        
    }

   private void OnTriggerEnter(Collider other)
{
    if (currentState!=ShadowState.World)
        return;

    if (other.gameObject.layer != playerLayerIndex)
        return;

    PlayerShadowInteraction foundInteraction =
    other.GetComponentInParent<PlayerShadowInteraction>();

if (foundInteraction != null)
{
    interaction = foundInteraction;
    playerTransform = foundInteraction.transform;

        if (connectionLine != null)
        {
            connectionLine.enabled = true;
        }

        interaction.AddShadow(this);
    }
}
private void OnTriggerExit(Collider other)
{
    if (currentState!=ShadowState.World)
        return;

    if (other.gameObject.layer != playerLayerIndex)
        return;

    PlayerShadowInteraction exitInteraction =
    other.GetComponentInParent<PlayerShadowInteraction>();

    if (exitInteraction != null)
    {
        exitInteraction.RemoveShadow(this);
    }
    else if (interaction != null)
    {
        interaction.RemoveShadow(this);
    }

    if (connectionLine != null)
    {
        connectionLine.enabled = false;
    }

    playerTransform = null;
    interaction = null;
}

    private void OnDisable()
    {
        if (interaction != null)
        {
            interaction.RemoveShadow(this);
            interaction = null;
        }

        if (connectionLine != null)
        {
            connectionLine.enabled = false;
        }

        playerTransform = null;
    }
    public void SetTargeted(bool targeted)
{
    if (targeted)
    {
        lineMaterial.color = targetedLineColor;
    }
    else
    {
        lineMaterial.color = normalLineColor;
    }
}
    public void Take()
        {
            if(interaction.equippedShadow==this)
            {
                weaponVisuals.WeaponAvailability();
            currentState=ShadowState.Equipped;
            interactionCollider.enabled = false;
            projectileCollider.enabled = false;
            connectionLine.enabled=false;
            transform.SetParent(interaction.transform);
            transform.localPosition = Vector3.zero;
            gameObject.SetActive(false);
            
            
            }
        }
    public void Throw(Vector3 aim,float speed)
    {
     currentState=ShadowState.Thrown;
     transform.SetParent(null);
     gameObject.SetActive(true);
     projectileCollider.enabled = true;
     shadowRigidBody.isKinematic=false;
     shadowRigidBody.linearVelocity=aim*speed;
    }
    public void HandleProjectileHit(Collider other)
        {
            if (currentState != ShadowState.Thrown)
              return;
            if(other.TryGetComponent<IDamageable>(out _))
        {
            HandleDamageableHit();
        }
        else if(other.gameObject.layer==environmentLayerIndex)
        {
            HandleEnvironmentHit();
        }
        }
    private void HandleDamageableHit()
    {
        Destroy(gameObject);
    }
    private void HandleEnvironmentHit()
    {
        projectileCollider.enabled = false;
        currentState=ShadowState.Stuck;
        if (!shadowRigidBody.isKinematic)
        {
            shadowRigidBody.linearVelocity = Vector3.zero;
            shadowRigidBody.isKinematic = true;
            StartCoroutine(EnvironmentDisappear());
        }
    }
    private IEnumerator EnvironmentDisappear()
    {
        yield return new WaitForSeconds(1f);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float alpha = 1f - (elapsedTime / fadeDuration);
            color = shadowRenderer.material.color;
            color.a = alpha;
            shadowRenderer.material.color = color;
            yield return null;
        }
        Destroy(gameObject);
    }
    
}


