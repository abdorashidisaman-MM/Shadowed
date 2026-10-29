using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private GameObject normalVisuals;
    [SerializeField] private GameObject deathVisual;
    [SerializeField] private float destroyDelay = 1.5f;

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
        normalVisuals.SetActive(false);

        deathVisual.SetActive(true);

        Destroy(gameObject, destroyDelay);
    }
}