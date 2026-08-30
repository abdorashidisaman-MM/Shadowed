using UnityEngine;
using System.Collections;

public class EnemyVisual : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;
    private Color color;
    private MeshRenderer humanBody;
    private EnemyHealth parentHealth;
    void Awake()
    {
        humanBody=GetComponent<MeshRenderer>();
        parentHealth=GetComponentInParent<EnemyHealth>();
    }
    void OnEnable()
    {
        parentHealth.OnDeath+=HumanDeath;
    }
    void OnDisable()
    {
        parentHealth.OnDeath-=HumanDeath;
    }
    private void HumanDeath()
    {
         StartCoroutine(VisualDisappear());
    }
    private IEnumerator VisualDisappear()
    {
        yield return new WaitForSeconds(1f);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float alpha = 1f - (elapsedTime / fadeDuration);
            color = humanBody.material.color;
            color.a = alpha;
            humanBody.material.color = color;
            yield return null;
        }
        Destroy(gameObject);
    }
}

