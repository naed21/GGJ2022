using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DamageReceiver))]
public class SimpleHealth : MonoBehaviour
{
    public int health;
    
    private DamageReceiver _damageReceiver;
    
    // Start is called before the first frame update
    void Start()
    {
        _damageReceiver = GetComponent<DamageReceiver>();
        _damageReceiver.onDamage += TakeDamage;
    }

    private void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
