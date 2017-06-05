using Assets.Managers;
using UnityEngine;

namespace Assets.Utils
{
    /// <summary>
    /// Class-Component add-in to alive objects(can think,can die...)
    /// </summary>
    public abstract class AliveObject : MonoBehaviour//, ILifeCycle
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        protected AliveObject(int hp) { Born(hp); }
        protected float Hitpoints;
        // ReSharper disable once InconsistentNaming
        public float GetHP() { return Hitpoints; }
        public virtual void GetDamage(float damage) { }
        public virtual void Born(int hp) { }
        public virtual void Die() { }
    }
//public interface ILifeCycle
//{
//    void Born(int hp);
//    void Die();
//    void GetDamage(int damage);
//}
    public class AlivePlayer : AliveObject//, ILifeCycle
    {
        private const int Maxfuel = 2000;
        private int _fuel;
        public AlivePlayer() : base(100) { }
        public override void Born(int hp)
        {
            Hitpoints = hp;
            _fuel = Maxfuel;
        }
        public override void Die()
        {
            gameObject.SetActive(false);
            GameManager.GetLevelState().SetState(LevelState.GameState.End);
        }
        public override void GetDamage(float damage)
        {
            Hitpoints -= damage;
            if (Hitpoints <= 0)
                Die();
        }
        public int GetFuel()
        {
            return _fuel;
        }
        public void ExpenditureFuel(int fuelval)
        {
            if (_fuel - fuelval < 0)
            {
                _fuel = 0;
            }
            else
                _fuel -= fuelval;
        }
        public void ChargeFuel(int fuelval)
        {
            if (_fuel + fuelval > Maxfuel)
            {
                _fuel = 100;
            }
            else
            {
                _fuel += fuelval;
            }
        }
    }
    public class AliveCoocon : AliveObject//, ILifeCycle
    {
        public float Damage;
        public AliveCoocon() : base(20)
        {
        }
        public override void Born(int hp)
        {
            Hitpoints = hp;
            Damage = 0.1f;
        }
        public override void Die()
        {
            GetComponent<ParticleSystem>().Play();
            GameManager.GetPlayerManager().Kills++;
            GameManager.GetObjectmanager().GetPool().SetDeleted(gameObject);
        }
        public override void GetDamage(float damage)
        {
            Hitpoints -= damage;
            GetComponent<ParticleSystem>().Play();
            if (Hitpoints <= 0)
                Die();
        }

        // ReSharper disable once UnusedMember.Local
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name == "Bullet")
            {
                GetDamage(collision.gameObject.GetComponent<DamageStat>().TotalDamage);
            }
        }

        // ReSharper disable once UnusedMember.Local
        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<AliveObject>().GetDamage(Damage);
            }
        }
    }
}