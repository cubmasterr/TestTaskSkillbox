using UnityEngine;

public class EnemyTurret : Enemy
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Transform _cannon;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _bulletVelocity;
    [SerializeField] private float _bulletLifeTime;
    [SerializeField] private float _timeToShoot;
    [SerializeField] private int _bulletCount;
    private float _timer;
    private PoolMono<Bullet> _poolMono;
    protected override void Start()
    {
        base.Start();
        _poolMono = new PoolMono<Bullet>(_bulletPrefab, _bulletCount, _cannon);
        _poolMono.IsAutoExpand = true;
    }
    protected override void Death()
    {
        Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        base.Death();
    }
    private void Update()
    {
        if (_target != null && _isDeath == false)
        {
            _cannon.LookAt(_target);
            if (_timer < _timeToShoot)
            {
                _timer += Time.deltaTime;
            }
            else 
            {
                _timer = 0;
                var bullet = _poolMono.GetFreeElement();
                bullet.transform.position = _shootPoint.position;
                bullet.SetVariables(_damage, _bulletLifeTime);
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * _bulletVelocity;
                _poolMono.RemoveElement(bullet);
            }
        }
        else
        {
            _timer = 0;
        }

    }

}
