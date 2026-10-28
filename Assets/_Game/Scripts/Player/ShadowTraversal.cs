using UnityEngine;

public class ShadowTraversal : MonoBehaviour
{
    [SerializeField] private GameObject allyShadow;
    [SerializeField] private float moveSpeed=20f;
    [SerializeField] private float minimumdistance=0.1f;
    [SerializeField] private GameObject allyReleasePoint;
    private GameObject currentTarget;
    [SerializeField] private ShadowSurfaceMotor shadowSurfaceMotor;
    [Tooltip("Degrees/second used to return the Player root to its original rotation after the launch. " +
             "The gameplay root must not stay rotated (collider/surface logic).")]
    [SerializeField] private float rotationReturnSpeed = 180f;
    private bool firstDistanceRiched=true;
    private bool secondDistanceRiched=true;
    private Quaternion restRotation = Quaternion.identity;
    private bool returningRotation;
    void Update()
    {
        if (returningRotation)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, restRotation, rotationReturnSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, restRotation) < 0.05f)
            {
                transform.rotation = restRotation;
                returningRotation = false;
            }
        }

     if(!firstDistanceRiched&&!secondDistanceRiched)
     {
        currentTarget=allyShadow;
     if(Vector3.Distance(transform.position,allyShadow.transform.position)>=minimumdistance)
        {
            Travel(currentTarget);
        }
        else
        {
            firstDistanceRiched=true;
            currentTarget=allyReleasePoint;
        }
     }
     if(firstDistanceRiched&&!secondDistanceRiched)
        {
            if(Vector3.Distance(transform.position,allyReleasePoint.transform.position)>=minimumdistance)
        {
            Travel(currentTarget);
        }
        else
            {
                secondDistanceRiched=true;
                currentTarget=null;
                
                // Return root to vertical orientation.
                returningRotation = true;

                shadowSurfaceMotor.enabled=false;
                return;
            }

        }
    }
    public void Travel(GameObject target)
    {
        
        transform.position=  Vector3.MoveTowards(transform.position,target.transform.position,moveSpeed*Time.deltaTime);
        transform.right= Vector3.RotateTowards(transform.right,target.transform.position-transform.position,1f*Time.deltaTime,0);
    }
    public void TraversalQue()
    {
        firstDistanceRiched=false;
        secondDistanceRiched=false;
    }
}
