using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SimpleCarAnimator))]
public class SimpleCarAutoDrive : MonoBehaviour
{

    Vector3 destination;
    SimpleCarAnimator simpleCarAnimator;
    [SerializeField] float steeringSpeed = 3f;
    [SerializeField] float acceleration = 5f;
    [SerializeField] float speedMultiplier = 1f;
    [SerializeField] float stoppingDistance = 1;
    float angleToDestination = 0;

    // Start is called before the first frame update
    void Start()
    {
        destination = transform.position;
        simpleCarAnimator = GetComponent<SimpleCarAnimator>();
    }

    [SerializeField] bool stopOnCollision;

    private void OnCollisionEnter(Collision collision)
    {
        if (stopOnCollision)
        {
            simpleCarAnimator.movement = 0;
            destination = transform.position;
        }
    }

    bool inFront;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current != null) if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                destination = hit.point;
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            destination = transform.position;
        }

        if (Vector3.Distance(transform.position, destination) > stoppingDistance)
        {
            simpleCarAnimator.movement = Mathf.Lerp(simpleCarAnimator.movement, (inFront ? 1 : -1) * speedMultiplier, Time.deltaTime * acceleration);            
            angleToDestination = Vector3.Angle(simpleCarAnimator.transform.TransformDirection(simpleCarAnimator.forward), destination - transform.position);
            Vector3 cross = Vector3.Cross(simpleCarAnimator.transform.TransformDirection(simpleCarAnimator.forward), destination - transform.position);
            inFront = angleToDestination < 90;
            if (cross.y < 0) angleToDestination = -angleToDestination;
            if (!inFront) angleToDestination *= -1;
            simpleCarAnimator.turnAngle = Mathf.Lerp(simpleCarAnimator.turnAngle, angleToDestination / simpleCarAnimator.maxTurnAngle, Time.deltaTime * steeringSpeed);
        }
        else
        {
            simpleCarAnimator.movement = Mathf.Lerp(simpleCarAnimator.movement, 0, Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        if (this.enabled) Gizmos.DrawWireSphere(destination, 0.25f);
    }
}
