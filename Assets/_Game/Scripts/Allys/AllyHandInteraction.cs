using UnityEngine;
using Shadowed.Input;
using UnityEngine.Playables;

public class AllyHandInteraction : MonoBehaviour
{
    [SerializeField] private PlayableDirector allyAssistDirector;
    [SerializeField] private ShadowSurfaceMotor shadowSurfaceMotor;

    private bool assistStarted;
    private PlayerInputReader playerInputReader;

    private bool playerInReach;
    private int playerLayer;


    private void Awake()
    {
        playerLayer =
            LayerMask.NameToLayer("Player");
    }


    private void Update()
    {
        if (!playerInReach)
        {
            return;
        }

        if (playerInputReader == null)
        {
            return;
        }

        if (!playerInputReader.ConsumeGrabRequest())
        {
            return;
        }

        if (assistStarted)
        {
            return;
        }
        shadowSurfaceMotor.enabled=false;
        allyAssistDirector.Play();
        Debug.Log("ALLY ASSIST STARTED");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != playerLayer)
        {
            return;
        }


        playerInputReader =
            other.GetComponentInParent<PlayerInputReader>();


        if (playerInputReader == null)
        {
            Debug.LogWarning(
                "Player entered AllyHand zone but no PlayerInputReader was found."
            );

            return;
        }


        playerInReach = true;

        Debug.Log(
            "player is in the zone"
        );
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != playerLayer)
        {
            return;
        }


        playerInReach = false;
        playerInputReader = null;

        Debug.Log(
            "player is out"
        );
    }
}