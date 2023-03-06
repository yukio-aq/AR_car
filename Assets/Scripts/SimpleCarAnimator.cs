using UnityEngine;

[ExecuteInEditMode]
public class SimpleCarAnimator : MonoBehaviour
{
    [Header("Runtime :")]
    [Range(-1, 1)]
    public float turnAngle;
    public float movement;
    public bool autoStraighten = true;
    public bool autoBrake = true;

    [Header("Setup : ")]
    public Vector3 forward = Vector3.forward;
    public Vector3 up = Vector3.up;
    public Vector3 right = Vector3.right;
    public float rotationPoint;
    public float maxTurnAngle = 33.75f;
    [System.Serializable]
    public class Wheel
    {
        public Transform transform = null;
        public float radius = 45;
        public Vector3 spinOffset = Vector3.zero;
        public enum Turns { False, Standard, Reverse };
        public Turns turns = Turns.Standard;
        public bool spins = true;
        [HideInInspector]
        public float distance = 0;
        [HideInInspector]
        public Vector3 lastPosition = Vector3.zero;
    }
    public Wheel[] wheels;

    private void Reset()
    {
        wheels = new Wheel[1];
    }

    private void LateUpdate()
    {
        if (Application.isPlaying) return;
        UpdateVehicle(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (!Application.isPlaying) return;
        UpdateVehicle(Time.fixedDeltaTime);
    }

    public Vector3 GetForward()
    {
        return transform.InverseTransformDirection(forward);
    }

    public Vector3 GetRotationPoint()
    {
        return transform.position + (transform.TransformDirection(forward) * rotationPoint);
    }

    void UpdateVehicle(float deltaTime)
    {
        // turning        
        turnAngle = Mathf.Clamp(turnAngle, -1, 1);
        if (autoStraighten) turnAngle = Mathf.Lerp(turnAngle, 0, deltaTime * (Mathf.Abs(movement))); // straighten wheels over time
        if (autoBrake) movement = Mathf.Lerp(movement, 0, deltaTime); // gradually slow vehicle over time

        // movement
        Vector3 rotationPosition = GetRotationPoint();
        Vector3 move = (transform.TransformDirection(forward) * (movement * deltaTime));

        transform.RotateAround(rotationPosition, transform.TransformDirection(up), (turnAngle * maxTurnAngle) * (movement * deltaTime));
        transform.position += (move);

        // wheel rotations
        Vector3 currentForward = GetForward();

        for (int i = 0; i < wheels.Length; i++)
        {
            if (wheels[i].transform == null) return;
            Vector3 v = Vector3.Scale(transform.InverseTransformDirection(wheels[i].transform.position - wheels[i].lastPosition), forward);
            float velocity = v.x + v.y + v.z;
            wheels[i].distance += velocity;
            Quaternion spinRotation = wheels[i].spins ? Quaternion.Euler(wheels[i].spinOffset) * Quaternion.AngleAxis(wheels[i].distance * (wheels[i].radius * Mathf.PI), right * -1) : Quaternion.identity * Quaternion.Euler(wheels[i].spinOffset);
            switch (wheels[i].turns)
            {
                case Wheel.Turns.False:
                    wheels[i].transform.localRotation = spinRotation;
                    break;
                case Wheel.Turns.Standard:
                    wheels[i].transform.rotation = transform.rotation * Quaternion.AngleAxis(turnAngle * maxTurnAngle, up) * spinRotation;
                    break;
                case Wheel.Turns.Reverse:
                    wheels[i].transform.rotation = transform.rotation * Quaternion.AngleAxis(turnAngle * maxTurnAngle, up * -1) * spinRotation;
                    break;
                default:
                    break;
            }
            wheels[i].lastPosition = wheels[i].transform.position;
        }
    }

    [ContextMenu("Setup Wheel Groups")]
    public void SetupGroups()
    {
        wheels = new Wheel[6];

        GameObject go;
        go = new GameObject();
        go.name = "Wheel_FL";
        go.transform.SetParent(transform, false);
        wheels[0] = new Wheel();
        wheels[0].transform = go.transform;
        wheels[0].spins = true;
        wheels[0].turns = Wheel.Turns.Standard;

        go = new GameObject();
        go.name = "Wheel_FR";
        go.transform.SetParent(transform, false);
        wheels[1] = new Wheel();
        wheels[1].transform = go.transform;
        wheels[1].spins = true;
        wheels[1].turns = Wheel.Turns.Standard;

        go = new GameObject();
        go.name = "Brakes_FL";
        go.transform.SetParent(transform, false);
        wheels[2] = new Wheel();
        wheels[2].transform = go.transform;
        wheels[2].spins = false;
        wheels[2].turns = Wheel.Turns.Standard;

        go = new GameObject();
        go.name = "Brakes_FR";
        go.transform.SetParent(transform, false);
        wheels[3] = new Wheel();
        wheels[3].transform = go.transform;
        wheels[3].spins = false;
        wheels[3].turns = Wheel.Turns.Standard;

        go = new GameObject();
        go.name = "Wheels_RL";
        go.transform.SetParent(transform, false);
        wheels[4] = new Wheel();
        wheels[4].transform = go.transform;
        wheels[4].spins = true;
        wheels[4].turns = Wheel.Turns.False;

        go = new GameObject();
        go.name = "Wheels_RR";
        go.transform.SetParent(transform, false);
        wheels[5] = new Wheel();
        wheels[5].transform = go.transform;
        wheels[5].spins = true;
        wheels[5].turns = Wheel.Turns.False;
    }

    private void OnDrawGizmos()
    {
        Vector3 rotationPosition = transform.position + (transform.TransformDirection(forward) * rotationPoint);
        Gizmos.DrawLine(rotationPosition, rotationPosition + Vector3.up);
    }
}
