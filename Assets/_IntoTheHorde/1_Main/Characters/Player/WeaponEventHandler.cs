using System;
using UnityEngine;

namespace IntoTheHorde
{
    /*
     *  Attach this script to the player
     */
    [DisallowMultipleComponent]
    public class WeaponEventHandler : MonoBehaviour
    {
        [SerializeField] Light           _muzzleFlashLight;
        [SerializeField] Camera          _mainCamera;
        [SerializeField] LayerMask       _shootableLayer;
        [SerializeField] float           _throwForce   = 700f;
        [SerializeField] CrosshairSpread _crosshairSpread;
        [SerializeField] GameObject      _muzzleFlash;
        [SerializeField] testScript      _spherePrefab;

        GunSpreadManager  m_gunSpreadManager;
        CameraController  m_camController;
        GameActor         m_actor;
        GunFiredEventArgs m_gunFiredArgs;
        Ray               m_ray;
        float             m_originalFOV;
        bool              m_weaponWasShotThisFrame;
        float             m_originalLookSens;

        void Awake()
        {
            if (_mainCamera == null)
                Debug.LogError("WeaponEventHandler: " + gameObject.name + " NULL camera reference");

            m_camController = _mainCamera.GetComponent<CameraController>();

            if (TryGetComponent<GameActor>( out m_actor ) == false)
                Debug.LogError("WeaponEventHandler: " + gameObject.name + " missing GameActor component");

            m_gunSpreadManager = GetComponent<GunSpreadManager>();
        }

        void Start()
        {
            _muzzleFlashLight.enabled = false;
            m_weaponWasShotThisFrame  = false;

            m_originalFOV      = _mainCamera.fieldOfView;
            m_originalLookSens = m_camController.LookSensitivity;
        }

        void OnEnable()
        {
            EventManager.AddListener(GameEvent.OnGunFired, OnShootHandler);
            EventManager.AddListener(GameEvent.OnGunScope, OnGunScopeHandler);
            EventManager.AddListener(GameEvent.OnGunUnscope, OnGunUnscopeHandler);
            EventManager.AddListener(GameEvent.OnUseThrowable, OnThrowableUsedHandler);
        }

        void OnDisable()
        {
            EventManager.RemoveListener(GameEvent.OnGunFired, OnShootHandler);
            EventManager.RemoveListener(GameEvent.OnGunScope, OnGunScopeHandler);
            EventManager.RemoveListener(GameEvent.OnGunUnscope, OnGunUnscopeHandler);
            EventManager.RemoveListener(GameEvent.OnUseThrowable, OnThrowableUsedHandler);
        }

        void Update()
        {
            if (m_weaponWasShotThisFrame)
            {
                _muzzleFlashLight.enabled = true;
                m_weaponWasShotThisFrame  = false;
            }
            else
            {
                _muzzleFlashLight.enabled = false;
            }
        }

        void OnShootHandler(EventArgs eventArgs)
        {
            m_gunFiredArgs = eventArgs as GunFiredEventArgs;

            if (m_gunFiredArgs.Shooter != m_actor)
                return;

            /*float[] spread = new float[3];
            spread[0] = UnityEngine.Random.Range( 0f, m_gunSpreadManager.Spread * 0.7f );
            spread[1] = UnityEngine.Random.Range( 0f, m_gunSpreadManager.Spread * 0.7f );
            spread[2] = UnityEngine.Random.Range( 0f, m_gunSpreadManager.Spread * 0.7f );

            for (int i = 0; i < 3; ++i)
            {
                if (UnityEngine.Random.Range(0f, 100f) <= 50f)
                    spread[i] = -spread[i];
            }  */ 

            Vector3 rayOrigin = _mainCamera.transform.position;

            Vector3 rayDir = _mainCamera.transform.forward;
            rayDir = transform.TransformDirection( rayDir );
            float spreadVal = m_gunSpreadManager.Spread;
            float spreadX = UnityEngine.Random.Range(0, spreadVal * 0.5f);
            float spreadY = UnityEngine.Random.Range(0, spreadVal * 0.5f);
            bool flipX = UnityEngine.Random.Range( 0f, 100f ) < 50f;
            bool flipY = UnityEngine.Random.Range( 0f, 100f ) < 50f;
            if (flipX) spreadX = -spreadX;
            if (flipY) spreadY = -spreadY;
            rayDir = Quaternion.Euler( spreadX, spreadY, 0f ) * rayDir;
            rayDir = _mainCamera.transform.InverseTransformDirection( rayDir );

            // Actual shooting/damaging
            m_ray = new Ray( rayOrigin, rayDir );
            if (Physics.Raycast(m_ray, out RaycastHit hitInfo, m_gunFiredArgs.GunRange, _shootableLayer))
            {
                hitInfo.collider.GetComponent<Hitbox>()?.OnHit( m_gunFiredArgs.BulletDamage );
                hitInfo.collider.GetComponent<RegularZombieController>()?.OnShot();

                // TODO : Delete debug
                // Instantiate(_spherePrefab, hitInfo.point, Quaternion.identity);
            }

            // Recoil
            float recoilX = UnityEngine.Random.Range( -1f, 1f );
            float recoilY = UnityEngine.Random.Range( 0.25f, 2.5f );
            m_camController.AddRecoil(new Vector2( recoilX, recoilY ));

            m_weaponWasShotThisFrame = true;
        }

        void OnThrowableUsedHandler(EventArgs eventArgs)
        {
            ThrowableUsedEventArgs args = eventArgs as ThrowableUsedEventArgs;

            if (args != null && args.Thrower == this.m_actor)
            {
                Vector3 spawnPos = _mainCamera.transform.position + _mainCamera.transform.forward * 1.5f;

                BaseThrowableBehavior throwable = Instantiate( args.ThrowablePrefab, spawnPos, Quaternion.identity, null );
                throwable.InitThrowForce(_mainCamera.transform.forward * _throwForce);
            }
        }

        void OnGunScopeHandler(EventArgs eventArgs)
        {
            GunScopeEventArgs args = eventArgs as GunScopeEventArgs;

            if (args != null)
            {
                _mainCamera.fieldOfView = args.ScopedFOV;
                m_camController.LookSensitivity *= 0.0833f;
            }
        }

        void OnGunUnscopeHandler(EventArgs eventArgs)
        {
            _mainCamera.fieldOfView = m_originalFOV;
            m_camController.LookSensitivity = m_originalLookSens;
        }

        void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            Gizmos.color = Color.green;
            Gizmos.DrawRay(_mainCamera.transform.position, m_ray.direction * 100f);
        }
    }
}
