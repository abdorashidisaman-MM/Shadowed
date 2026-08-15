using UnityEngine;

public class ShadowRange : MonoBehaviour
{
    public string ShadowName;

    private LineRenderer connectionLine;
    private Transform playerTransform;
    private PlayerShadowInteraction interaction;
    private int playerLayerIndex;

    private void Awake()
    {
        playerLayerIndex = LayerMask.NameToLayer("Player");
        connectionLine = GetComponent<LineRenderer>();

        if (connectionLine != null)
        {
            connectionLine.positionCount = 2;
            connectionLine.enabled = false;
        }
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
        if (other.gameObject.layer == playerLayerIndex)
        {
            Debug.Log("entered");

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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == playerLayerIndex)
        {
            Debug.Log("Exited");

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
}

