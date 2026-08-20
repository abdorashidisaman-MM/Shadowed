using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float enemyHealth=100f;

    public void Damage()
    {
        enemyHealth-=10f;
        Debug.Log("Current Damage : "+enemyHealth);
    }
}
