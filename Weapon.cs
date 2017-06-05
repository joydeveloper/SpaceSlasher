using System.Diagnostics.CodeAnalysis;
using Assets.Managers;
using UnityEngine;

namespace Assets.Utils
{
    public abstract class Weapon
    {
        public Bullet Bullets;
        protected float Firerate;
        protected float Range;
        protected int Barrelpower;//this is multyply for bulletdamage(equal of barrel lenght  or power of acccubat in laserguns)
        protected float Capacity;
        protected GameObject FireParticles;//Particlesystem objects
        protected float Firedelay;
        protected Transform Barrel;  
        //TODO capacity=maxammocount
        //  protected float ammocount;
        public virtual void Fire()
        {
            Firedelay += Time.deltaTime;
            // Debug.Log(firedelay);
            if (Capacity > 0 && Firedelay > Firerate)
            {
                Firedelay = 0;
                Capacity--;
                foreach (ParticleSystem ps in FireParticles.GetComponentsInChildren<ParticleSystem>())
                {
                    if (!ps.isPlaying)
                        ps.Play();
                }
                FireParticles.transform.position = new Vector3(Barrel.position.x + 0.2f, Barrel.position.y, Barrel.position.z + 3);
                Object.Destroy(Bullets.PushBullet(Barrel, Bullets.Damage), Range / Bullets.Speed);
            }
        }
        public void LoadAmmo(float ammocount)
        {
            Capacity += ammocount;
        }
    }
    public class MachineGun : Weapon
    {
        public MachineGun(GameObject fire, GameObject bullet, Transform trans)
        {
            Firerate = 0.2f;
            Range = 300;
            Barrelpower = 5;
            Capacity = Mathf.Infinity;
            FireParticles = Object.Instantiate(fire);
            Firedelay = 0f;
            Bullets = new MachineGunBullet(bullet);
            Bullets.Damage *= Barrelpower;
            Barrel = trans;
        }
    }
    public class FlameThrower : Weapon, ISprayWeapon
    {
        public FlameThrower(GameObject fire, Transform trans)
        {
            Firerate = 0.1f;
            Range = 50;
            Barrelpower = 1;
            Bullets = new FlameThrowerBullet();
            Bullets.Damage *= Barrelpower;
            FireParticles = Object.Instantiate(fire);
            Firedelay = 0.1f;
            Barrel = trans;
            // bullets.damage*-
        }
        public override void Fire()
        {
            Firedelay += Time.deltaTime;
            if (Capacity > 0 && Firedelay > Firerate)
            {
                Firedelay = 0;
                if(!FireParticles.transform.GetChild(0).gameObject.GetComponent<FlameDamage>())
                    FireParticles.transform.GetChild(0).gameObject.AddComponent<FlameDamage>().Damage = Bullets.Damage;
                foreach (ParticleSystem ps in FireParticles.GetComponentsInChildren<ParticleSystem>())
                {
                    ps.startSpeed = Bullets.Speed;
                    if (!ps.isPlaying)
                        ps.Play();
                }
                FireParticles.transform.position = new Vector3(Barrel.position.x + 0.2f, Barrel.position.y, Barrel.position.z + 0.5f);
            }
        }
        public void ImediatelyFire(float capacity)
        {
            Firedelay += Time.deltaTime;
            if (capacity > 0 && Firedelay > Firerate)
            {
                Firedelay = 0;
                if (!FireParticles.transform.GetChild(0).gameObject.GetComponent<FlameDamage>())
                    FireParticles.transform.GetChild(0).gameObject.AddComponent<FlameDamage>().Damage = Bullets.Damage;
                foreach (ParticleSystem ps in FireParticles.GetComponentsInChildren<ParticleSystem>())
                {
                    ps.startSpeed = Bullets.Speed;
                    if (!ps.isPlaying)
                        ps.Play();
                }
                FireParticles.transform.position = new Vector3(Barrel.position.x + 0.2f, Barrel.position.y, Barrel.position.z + 0.5f);
            }
        }
    }
    public interface ISprayWeapon
    {
        void ImediatelyFire(float capacity);
    }
    [SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
    public abstract class Bullet
    {
        protected GameObject BuleltObject;
        public float Speed;
        public float Damage;
        public GameObject PushBullet(Transform trans, float damage)
        {
            GameObject bul = Object.Instantiate(BuleltObject);
            bul.gameObject.name = "Bullet";
            bul.AddComponent<DamageStat>().TotalDamage = damage;
            bul.transform.position = new Vector3(trans.position.x, trans.position.y, trans.position.z + GameManager.GetPlayerManager().GetPlayer().transform.localScale.z * 2.1f);
            Rigidbody rbb = bul.AddComponent<Rigidbody>();
            rbb.useGravity = false;
            rbb.AddForce(new Vector3(trans.forward.x, 0, trans.forward.z) * Speed + GameManager.GetPlayerManager().GetPlayer().GetComponent<Rigidbody>().velocity, ForceMode.VelocityChange);
            return bul;
        }
    }
    public class MachineGunBullet : Bullet
    {
        public MachineGunBullet(GameObject go)
        {
            BuleltObject = go;
            Speed = 50;
            Damage = 1;
        }
    }
    public class FlameThrowerBullet : Bullet
    {
        public FlameThrowerBullet()
        {
            Speed = 10;
            Damage = 0.5f;
        }
    }
    public class DamageStat : MonoBehaviour
    {
        [HideInInspector]
        public float TotalDamage;
    }
}