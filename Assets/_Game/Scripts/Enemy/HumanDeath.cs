using UnityEngine;

public class LastEnemyDeath : MonoBehaviour
{
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private Animator humanAnimator;
      private static readonly int Die =
        Animator.StringToHash("Die");

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
           humanAnimator.SetTrigger(Die);
    }
}