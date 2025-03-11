using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Animator))]
public class EnemyInfantry : Enemy
{
    [SerializeField] private Rigidbody[] _rigidbodies;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    protected override void Start()
    {
        base.Start();
        _animator = gameObject.GetComponent<Animator>();
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }
    protected override void Death()
    {       
        _animator.enabled = false;
        _navMeshAgent.isStopped=true;
        foreach (Rigidbody rigidbody in _rigidbodies)
        {
            rigidbody.isKinematic = false;
        } 
        base.Death();
    }
    private void Update()
    {
        if (_target != null && _isDeath == false)
        {
            _animator.SetBool("ChasePlayer", true);
            _navMeshAgent.destination = _target.position;
        }
        else
        {
            _navMeshAgent.destination = transform.position;
            _animator.SetBool("ChasePlayer", false);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponent<HealthController>();
        if (player != null&& _isDeath == false) 
        {
            player.TakeDamage(_damage);
            Instantiate(_explosionEffect,transform.position,Quaternion.identity);
            _timeToDestroy = 0;
            Death();
        }

    }
}
