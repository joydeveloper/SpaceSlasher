using UnityEngine;

public class CameraTop : MonoBehaviour {
    public Transform target;
    public float toppad=4;
    public float height=10;

    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    private void LateUpdate () {
        transform.position = new Vector3(0, target.transform.position.y+height, target.transform.position.z + toppad);	
	}
}
