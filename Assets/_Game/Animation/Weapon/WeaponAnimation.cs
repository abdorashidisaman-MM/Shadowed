using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
[SerializeField] private GameObject Hand;
private bool weaponGrabed=true;
private float minimumdistance=0.01f;
    void Update()
    {
        
        if(weaponGrabed)
        return;
        Debug.Log(Vector3.Distance(transform.position, Hand.transform.position));
     if(Vector3.Distance(transform.position,Hand.transform.position)>=minimumdistance)
        {
            WeaponGrab();
        }
        else
        {
            weaponGrabed=true;
            transform.SetParent(Hand.transform);
            transform.localPosition=Vector3.zero;         
        }
    }
    public void WeaponGrab()
    {
        Debug.Log("it started");
        transform.position=Vector3.MoveTowards(transform.position,Hand.transform.position,30f*Time.deltaTime);
        transform.localEulerAngles=Vector3.RotateTowards(transform.position,Hand.transform.position,30f*Time.deltaTime,0);
    }
    public void WeaponAvailability()
    {
        weaponGrabed=false;
    }
    public void WeaponConsuption()
    {
        gameObject.SetActive(false);
    }
}
