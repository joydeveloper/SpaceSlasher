using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI
{
    public class ChangeColor : MonoBehaviour {
        //TODO adds public variables to control color and time
      
        // ReSharper disable once UnusedMember.Local
        private void Update () {
            GetComponent<Text>().color = Color.Lerp(Color.white, Color.blue, Mathf.PingPong(Time.time, 0.5f));
        }
    }
}
