using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    [SerializeField] private EnemyHealth enemyHealth;

    [SerializeField] private GameObject shadowVisuals;
    [SerializeField] private GameObject deathVisual;

    [SerializeField] private float deathVisualDuration = 1.5f;


    private void Awake()
    {
        deathVisual.SetActive(false);
    }


    private void OnEnable()
    {
        enemyHealth.OnDeath += PlayDeath;
    }


    private void OnDisable()
    {
        enemyHealth.OnDeath -= PlayDeath;
    }


    private void PlayDeath()
    {
        // Hide only the enemy's normal SHADOW visual.
        if (shadowVisuals != null)
            shadowVisuals.SetActive(false);

        // Show shadow dissolve / explosion.
        if (deathVisual != null)
        {
            deathVisual.SetActive(true);

            Destroy(
                deathVisual,
                deathVisualDuration
            );
        }

        // IMPORTANT:
        // Do NOT destroy the Enemy root.
        // Human body remains in the scene.
    }
}