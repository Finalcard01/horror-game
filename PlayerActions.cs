using TMPro;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private Camera cam;
    public Transform selectedDoor;
    public GameObject dragPointObject;
    [SerializeField] public LayerMask doorLayer;

    void Update()
    {
        // Do raycasts 
        RaycastHit hitInfo;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, 5, doorLayer))
        {
            if (Input.GetMouseButtonDown(0))
            {
                selectedDoor = hitInfo.collider.gameObject.transform;
            }
        }

        if (selectedDoor != null)
        {
            // Get the hinge
            HingeJoint joint = selectedDoor.GetComponent<HingeJoint>();
            JointMotor motor = joint.motor;
            
            // Create a drag point where mouse is
            if (dragPointObject == null)
            {
                dragPointObject = new GameObject("Ray Door");
                dragPointObject.transform.parent = selectedDoor.transform;
            }
            
            Ray ray = cam.ScreenPointToRay(Input.mousePosition); 
            dragPointObject.transform.position = ray.GetPoint(Vector3.Distance(selectedDoor.position, transform.position));
            dragPointObject.transform.rotation = selectedDoor.rotation;
            
            float delta = Mathf.Pow(Vector3.Distance(dragPointObject.transform.position, selectedDoor.position), 3);

            float speedMulti = 600000;
            if (Mathf.Abs(selectedDoor.parent.forward.x) > 0.5f)
            {
                if (dragPointObject.transform.position.x > selectedDoor.position.x)
                {
                    motor.targetVelocity = delta * -speedMulti * Time.deltaTime;
                }
                else
                {
                    motor.targetVelocity = delta * speedMulti * Time.deltaTime;
                }
            }
            else
            {
                if (dragPointObject.transform.position.z > selectedDoor.position.z)
                {
                    motor.targetVelocity = delta * -speedMulti * Time.deltaTime;
                }
                else
                {
                    motor.targetVelocity = delta * speedMulti * Time.deltaTime;
                }
            }
           joint.motor = motor;

           if (Input.GetMouseButtonUp(0))
           {
               selectedDoor = null;
               motor.targetVelocity = 0;
               joint.motor = motor;
               Destroy(dragPointObject);
           }
        }
    }

}
