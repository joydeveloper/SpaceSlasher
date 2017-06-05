using Assets.Managers;
using UnityEngine;

namespace Assets.Utils
{
    public class FuelBar : MonoBehaviour {
        public int Bonusfuel=100;

        // ReSharper disable once SuggestBaseTypeForParameter
        // ReSharper disable once UnusedMember.Local
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag != "Player") return;
            GameManager.GetObjectmanager().GetPool().SetDeleted(gameObject);
            GameManager.GetPlayerManager().GetPlayerStatus().ChargeFuel(Bonusfuel);
        }
    }
}
