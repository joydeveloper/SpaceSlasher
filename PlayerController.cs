using Assets.Managers;
using UnityEngine;

namespace Assets.Utils
{
    public class PlayerController : MonoBehaviour
    {
        //  public float power;
        //TODO const to refs
        public readonly string[] Flypart = { "RightSpray", "LeftSpray", "FlyLight"};
        public float Sidespeed;
        public float Forwardspeed;
        public float Brakeval = 0f;
        public float Pitch;
        //public float yaw;
        public float Roll;
        public Weapon[] Weapons;
        public GameObject[] Bullets;
        private float _moveHorizontal;
        private float _moveVertical;
        private readonly float maxenginepower=40;
        private const int Enginefuelrate = 1;
        private Rigidbody _rb;
        private ParticleSystem _rightspray;
        private ParticleSystem _leftspray;
        public GameObject[] Fire;

        private ISprayWeapon _isp;
        // ParticleSystem damage;
        private Light _flylight;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rightspray = transform.FindChild("RightSpray").GetComponent<ParticleSystem>();
            _leftspray = transform.FindChild("LeftSpray").GetComponent<ParticleSystem>();
            _flylight = transform.FindChild("FlyLight").GetComponent<Light>();
            _flylight.enabled = false;
        }

        private void Start()
        {
            Weapons = new Weapon[3];
            MachineGun mg = new MachineGun(Fire[0], Bullets[0], transform);
            FlameThrower lft = new FlameThrower(Fire[1], _leftspray.transform);
            FlameThrower rft = new FlameThrower(Fire[1], _rightspray.transform);
            Weapons[0] = mg;
            Weapons[1] = lft;
            Weapons[2] = rft;
        }

        private void FixedUpdate()
        {
            _rb.AddForce(Vector3.forward * Mathf.Min(Brakeval*_moveVertical, _moveVertical) * Forwardspeed,ForceMode.Acceleration);
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y,Mathf.Min(_rb.velocity.z , maxenginepower));
            _rb.velocity = new Vector3(_moveHorizontal * Sidespeed, 0, _rb.velocity.z);
            _rb.rotation = Quaternion.Euler(Pitch * _moveVertical, _rb.rotation.y, -Roll * _moveHorizontal);
            if (_rb.velocity.z < 0)
                _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y, 1f);
        }

        private void Update()
        {
            _moveHorizontal = Input.GetAxis("Horizontal");
            _moveVertical = Input.GetAxis("Vertical");
            if (_moveVertical > 0.2f)
            {
                GameManager.GetPlayerManager().GetPlayerStatus().ExpenditureFuel(Enginefuelrate);
            }
            if (Input.GetButtonDown("Light"))
            {
                _flylight.enabled = !_flylight.enabled;
            }
            if (Input.GetButton("Fire1"))
            {
                Weapons[0].Fire();
            }
            if (Input.GetButton("Fire2"))
            {
                GameManager.GetPlayerManager().GetPlayerStatus().ExpenditureFuel(1);
                _isp=(FlameThrower)Weapons[1];
                _isp.ImediatelyFire(GameManager.GetPlayerManager().GetPlayerStatus().GetFuel());
                _isp = (FlameThrower)Weapons[2];
                _isp.ImediatelyFire(GameManager.GetPlayerManager().GetPlayerStatus().GetFuel());
            }
        }
    }
}
