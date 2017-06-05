using Assets.Managers;
using UnityEngine;

namespace Assets.Utils
{
    public class BombObject : MonoBehaviour
    {
        public GameObject Explosion;
        private const int Damage = 10;
        public float Radius = 5.0F;
        public float Power = 10.0F;

        // ReSharper disable once UnusedMember.Local
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.GetComponent<AliveObject>()) return;
            Instantiate(Explosion, gameObject.transform.transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<AliveObject>().GetDamage(Damage);
            GameManager.GetObjectmanager().GetPool().SetDeleted(gameObject);
        }
    }
}
