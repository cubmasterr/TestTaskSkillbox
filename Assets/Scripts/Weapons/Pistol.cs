using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField] private float _recoilAmount;
    [SerializeField] private float _recoilTime;
    [SerializeField] private int _bulletCount;
    [SerializeField] public float _bulletVelocity;
    [SerializeField] public float _bulletLifeTime;
    [SerializeField] private GunCharacteristics _gunCharacteristics;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private KeyCode _shootKeyCode;
    private Vector3 _destination;
    private PoolMono<Bullet> _poolMono;
    private float recoilTimer = 0f;
    private bool isRecoiling = false;
    private Vector3 _originalPosition;

    private void Start()
    {
        _poolMono = new PoolMono<Bullet>(_bulletPrefab, _bulletCount, transform);
        _poolMono.IsAutoExpand = true;
        _originalPosition = transform.localPosition;
    }
    private void Update()
    {
        if (Input.GetKeyDown(_shootKeyCode))
        {
            Shoot();
        }
        if (isRecoiling)
        {
            recoilTimer -= Time.deltaTime;
            if (recoilTimer <= 0)
            {
                transform.localPosition = _originalPosition;
                isRecoiling = false;
            }
        }
    }
    private void Shoot()
    {
        var bullet = _poolMono.GetFreeElement();
        bullet.transform.position = _shootPoint.position;
        bullet.SetVariables(_gunCharacteristics.damage, _bulletLifeTime);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * _bulletVelocity; 
        _poolMono.RemoveElement(bullet);
        ApplyRecoil();
        void ApplyRecoil()
        {
            transform.localPosition -= _shootPoint.forward * _recoilAmount;
            recoilTimer = _recoilTime;
            isRecoiling = true;
        }
    }
}