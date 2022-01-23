using System;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    public Action<int> onDamage;
    
    public void DoDamage(int damage)
    {
        onDamage?.Invoke(damage);
    }
}
