using UnityEngine;

public class ShadowRange : MonoBehaviour
{
    [SerializeField] private Color normalLineColor = Color.white;
    [SerializeField] private Color targetedLineColor = Color.yellow;
    [SerializeField] private BoxCollider interactionCollider;
    [SerializeField] private BoxCollider projectileCollider;
    public string ShadowName;

    public LineRenderer connectionLine;
    private Material lineMaterial;
    private Transform playerTransform;
    private PlayerShadowInteraction interaction;
    private Rigidbody shadowRigidBody;
    private int playerLayerIndex;
    private int enemyLayerIndex;
    private int environmentLayerIndex;
    private bool isWorldShadow = true;
    
  
    
    
    

    private void Awake()
    {
        shadowRigidBody=GetComponent<Rigidbody>();
        playerLayerIndex = LayerMask.NameToLayer("Player");
        enemyLayerIndex = LayerMask.NameToLayer("Enemy");
        environmentLayerIndex=LayerMask.NameToLayer("Environment");
        connectionLine = GetComponent<LineRenderer>();

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
    if (!isWorldShadow)
        return;

    if (other.gameObject.layer != playerLayerIndex)
        return;

    if (other.TryGetComponent<PlayerShadowInteraction>(out interaction))
    {
        playerTransform = other.transform;

        if (connectionLine != null)
        {
            connectionLine.enabled = true;
        }

        interaction.AddShadow(this);
    }
}
private void OnTriggerExit(Collider other)
{
    if (!isWorldShadow)
        return;

    if (other.gameObject.layer != playerLayerIndex)
        return;

    if (other.TryGetComponent<PlayerShadowInteraction>(out var exitInteraction))
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
            isWorldShadow=false;
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
     
     transform.SetParent(null);
     gameObject.SetActive(true);
     projectileCollider.enabled = true;
     shadowRigidBody.isKinematic=false;
     shadowRigidBody.linearVelocity=aim*speed;
    }
    public void HandleProjectileHit(Collider other)
        {
            if(other.gameObject.layer==enemyLayerIndex)
        {
            HandleEnemyHit();
        }
        else if(other.gameObject.layer==environmentLayerIndex)
        {
            HandleEnvironmentHit();
        }
        }
    private void HandleEnemyHit()
    {
        Destroy(gameObject);
    }
    private void HandleEnvironmentHit()
    {
        projectileCollider.enabled = false;

        if (!shadowRigidBody.isKinematic)
        {
            shadowRigidBody.linearVelocity = Vector3.zero;
            shadowRigidBody.isKinematic = true;
        }
    }
}

