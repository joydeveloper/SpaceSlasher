using System.Collections;
using UnityEngine;

//TODO extend this
namespace Assets.Utils
{
    public class SimpleMove : MonoBehaviour
    {
        public float Tilt;
        public float Dodge;
        public float Smoothing;
        public Vector2 StartWait;
        public Vector2 ManeuverTime;
        public Vector2 ManeuverWait;
        private float _currentSpeed=10;
        // private float targetManeuver;
        private void Start()
        {
            StartCoroutine(Evade());
        }

        private IEnumerator Evade()
        {
            yield return new WaitForSeconds(Random.Range(StartWait.x, StartWait.y));
            while (true)
            {
                //  targetManeuver = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x);
                yield return new WaitForSeconds(Random.Range(ManeuverTime.x, ManeuverTime.y));
                //   targetManeuver = 0;
                yield return new WaitForSeconds(Random.Range(ManeuverWait.x, ManeuverWait.y));
                _currentSpeed = Random.Range(7, 12);
            }
        }

        private void FixedUpdate()
        {
            //  float newManeuver = Mathf.MoveTowards(GetComponent<Rigidbody>().velocity.x, targetManeuver, smoothing * Time.deltaTime);
            GetComponent<Rigidbody>().velocity =-Vector3.forward*_currentSpeed;
            // GetComponent<Rigidbody>().rotation = Quaternion.Euler(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, GetComponent<Rigidbody>().velocity.x * -tilt);
        }
    }
}
