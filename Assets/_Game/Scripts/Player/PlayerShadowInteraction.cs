using UnityEngine;

public class PlayerShadowInteraction : MonoBehaviour
{
    private ShadowRange currentShadow;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetCurrentShadow(ShadowRange shadow)
    {
        currentShadow=shadow;
        Debug.Log(shadow.ShadowName);
    }
    public void ClearCurrentShadow(ShadowRange shadow)
{
    if (currentShadow == shadow)
    {
        currentShadow = null;
        Debug.Log("No current shadow");
    }
}
}
