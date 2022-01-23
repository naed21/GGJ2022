using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float velocity;
    public int damage;

    private Rigidbody _rigidbody;
    private Transform _transform;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_transform.forward * velocity + _transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        var damageRec = other.GetComponent<DamageReceiver>();
        if (damageRec != null)
        {
            damageRec.DoDamage(damage);
        }
        Destroy(gameObject);
    }
}
