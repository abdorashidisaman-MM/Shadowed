using System.Collections;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

public class ShadowRange : MonoBehaviour
{
    [SerializeField] private Color normalLineColor = Color.white;
    [SerializeField] private Color targetedLineColor = Color.yellow;
    [SerializeField] private Collider interactionCollider;
    [SerializeField] private Collider projectileCollider;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private WeaponAnimation weaponAnimation;
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;
    [SerializeField] private SpriteRenderer HighlightSpriteRenderer;

    public Sprite WeaponSprite
{
    get
    {
        return weaponSpriteRenderer.sprite;
    }
}
    
   

    
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
    
    
    
    private enum ShadowState
    {
        World,
        Equipped,
        Thrown,
        Stuck
    }
    private ShadowState currentState;
    
  
    
    
    

    [Tooltip("A thrown weapon that hits nothing is removed after this many seconds.")]
    [SerializeField] private float maxFlightTime = 4f;

    private float throwTime;
    private bool isTargeted;

    // True while the shadow is lying in the world and can be grabbed.
    public bool IsGrabbable => currentState == ShadowState.World;

    // True while the shadow is flying and allowed to deal damage.
    public bool IsThrown => currentState == ShadowState.Thrown;
    public Color c;


    private void Awake()
    {
        shadowRenderer=GetComponent<MeshRenderer>();
        shadowRigidBody=GetComponent<Rigidbody>();
        playerLayerIndex = LayerMask.NameToLayer("Player");
        environmentLayerIndex=LayerMask.NameToLayer("Environment");
        connectionLine = GetComponent<LineRenderer>();
        currentState=ShadowState.World;
        c = HighlightSpriteRenderer.color;
        if (connectionLine != null)
        {
            connectionLine.positionCount = 2;
            connectionLine.enabled = false;
            lineMaterial = connectionLine.material;
        }
    }

    private void Update()
    {
        if (connectionLine != null  && playerTransform != null)
        {
            connectionLine.SetPosition(0, transform.position);
            connectionLine.SetPosition(1, playerTransform.position);
            Highlight();
        }

        if (currentState == ShadowState.Thrown &&
            Time.time - throwTime >= maxFlightTime)
        {
            Destroy(gameObject);
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
    
        HighlightSpriteRenderer.gameObject.SetActive(true);
       // if (connectionLine != null)
       // {
        //    connectionLine.enabled = true;
      //  }

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
    HighlightSpriteRenderer.gameObject.SetActive(false);
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
            HighlightSpriteRenderer.gameObject.SetActive(false);
            connectionLine.enabled = false;
        }

        playerTransform = null;
    }
    public void SetTargeted(bool targeted)
    {
        if (isTargeted == targeted)
            return;

        isTargeted = targeted;

        if (lineMaterial != null)
        {
            lineMaterial.color = targeted ? targetedLineColor : normalLineColor;
        }
    }

    // Gameplay ownership: called the moment the player grabs this shadow.
    // The world visual stays until HideWorldVisual() (grab-contact frame).
    public void Take(PlayerShadowInteraction owner)
    {
        if (owner == null || owner.equippedShadow != this)
            return;

        currentState = ShadowState.Equipped;
        weaponAnimation.WeaponAvailability();

        if (interactionCollider != null) interactionCollider.enabled = false;
        if (projectileCollider != null) projectileCollider.enabled = false;
        HighlightSpriteRenderer.gameObject.SetActive(false);
        if (connectionLine != null) connectionLine.enabled = false;

        isTargeted = false;
        owner.RemoveShadow(this);

        interaction = null;
        playerTransform = null;
    }

    // Visual: the shadow leaves the world (it is now drawn in the hand).
    public void HideWorldVisual()
    {
        if (currentState == ShadowState.Equipped && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    public void Throw(Vector3 aim,float speed)
    {
        if (aim.sqrMagnitude < 0.0001f)
            return;

        currentState=ShadowState.Thrown;
        throwTime = Time.time;
        weaponSpriteRenderer.transform.localPosition = Vector3.zero;
        gameObject.SetActive(true);
        

        if (shadowRigidBody != null)
        {
            shadowRigidBody.position = transform.position;
            shadowRigidBody.rotation = transform.rotation;
            shadowRigidBody.interpolation = RigidbodyInterpolation.Interpolate;
            shadowRigidBody.isKinematic=false;
            shadowRigidBody.linearVelocity=aim.normalized*speed;
        }

        if (projectileCollider != null) projectileCollider.enabled = true;
    }

    // Called by ProjectileHitbox. Returns true if this hit was consumed
    // (so the hitbox applies damage at most once per throw).
    public bool HandleProjectileHit(Collider other)
    {
        if (currentState != ShadowState.Thrown)
            return false;

        if (other.TryGetComponent<IDamageable>(out _))
        {
            HandleDamageableHit();
            return true;
        }

        if (other.gameObject.layer == environmentLayerIndex)
        {
            HandleEnvironmentHit();
        }

        return false;
    }

    private void HandleDamageableHit()
    {
        // Stop interacting immediately; Destroy is deferred to end of frame.
        currentState = ShadowState.Stuck;
        if (projectileCollider != null) projectileCollider.enabled = false;
        Destroy(gameObject);
    }

    private void HandleEnvironmentHit()
    {
        if (projectileCollider != null) projectileCollider.enabled = false;
        currentState=ShadowState.Stuck;
        if (shadowRigidBody != null && !shadowRigidBody.isKinematic)
        {
            shadowRigidBody.linearVelocity = Vector3.zero;
            shadowRigidBody.isKinematic = true;
            StartCoroutine(EnvironmentDisappear());
        }
    }

    private IEnumerator EnvironmentDisappear()
    {
        yield return new WaitForSeconds(1f);

        // The first weapon is a MeshRenderer, the final weapon is drawn by
        // a child SpriteRenderer. Fade whichever exists.
        SpriteRenderer sprite = shadowRenderer == null
            ? GetComponentInChildren<SpriteRenderer>()
            : null;
        Material meshMaterial = shadowRenderer != null ? shadowRenderer.material : null;

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float alpha = 1f - (elapsedTime / fadeDuration);

            if (meshMaterial != null)
            {
                color = meshMaterial.color;
                color.a = alpha;
                meshMaterial.color = color;
            }
            else if (sprite != null)
            {
                color = sprite.color;
                color.a = alpha;
                sprite.color = color;
            }

            yield return null;
        }
        Destroy(gameObject);
    }
    public void Highlight()
    {
        
          
      c.a = Mathf.PingPong(
        Time.time * 2,
        1f
    );

    HighlightSpriteRenderer.color = c;
    }
    
    
    
    
}


