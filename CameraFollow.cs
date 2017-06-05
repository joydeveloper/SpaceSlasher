using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 20.0f;
    public float height = 5.0f;
    public float heightDamping = 2.0f;
    public float lookAtHeight = 0.0f;
    public Rigidbody parentRigidbody;
    public float rotationSnapTime = 0.3F;
    public float distanceSnapTime;
    public float distanceMultiplier;
    private Vector3 lookAtVector;
    private float usedDistance;
    private float wantedRotationAngle;
    private float wantedHeight;
    private float currentRotationAngle;
    private float currentHeight;
    private Quaternion currentRotation;
    private Vector3 wantedPosition;
    private float yVelocity;
    private float zVelocity;

    private void Start()
    {
        parentRigidbody = GameObject.Find("Flycopter(Clone)").GetComponent<Rigidbody>();
        target = GameObject.Find("Flycopter(Clone)").transform.FindChild("Body").transform;
        lookAtVector = new Vector3(0, lookAtHeight, 0);
    }

    private void LateUpdate()
    {
        wantedHeight = target.position.y + height;
        currentHeight = transform.position.y;
        wantedRotationAngle = target.eulerAngles.y;
        currentRotationAngle = transform.eulerAngles.y;
        currentRotationAngle = Mathf.SmoothDampAngle(currentRotationAngle, wantedRotationAngle, ref yVelocity, rotationSnapTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
        wantedPosition = target.position;
        wantedPosition.y = currentHeight;
        usedDistance = Mathf.SmoothDampAngle(usedDistance, distance + (parentRigidbody.velocity.magnitude * distanceMultiplier), ref zVelocity, distanceSnapTime);
        wantedPosition += Quaternion.Euler(0, currentRotationAngle, 0) * new Vector3(0, 0, -usedDistance);
        transform.position = wantedPosition;
        transform.LookAt(target.position + lookAtVector);
    }
}