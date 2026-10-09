using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
[SerializeField] private GameObject player;
private bool weaponGrabed=true;
private float minimumdistance=0.01f;
    void Update()
    {
        
        if(weaponGrabed)
        return;
        Debug.Log(Vector3.Distance(transform.position, player.transform.position));
     if(Vector3.Distance(transform.position,player.transform.position)>=minimumdistance)
        {
            WeaponGrab();
        }
        else
        {
            weaponGrabed=true;
            gameObject.SetActive(false);
        }
    }
    public void WeaponGrab()
    {
        Debug.Log("it started");
        transform.position=Vector3.MoveTowards(transform.position,player.transform.position,30f*Time.deltaTime);
        transform.localEulerAngles=Vector3.RotateTowards(transform.position,player.transform.position,20f*Time.deltaTime,0);
    }
    public void WeaponAvailability()
    {
        weaponGrabed=false;
    }
}
