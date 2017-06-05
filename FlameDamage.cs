using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Assets.Utils
{
    public class FlameDamage : MonoBehaviour
    {
        public float Damage;

        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private void OnParticleCollision(GameObject other)
        {
            if (other.GetComponent<AliveObject>())
            {
                other.GetComponent<AliveObject>().GetDamage(Damage);
            }      
        }
    }
}