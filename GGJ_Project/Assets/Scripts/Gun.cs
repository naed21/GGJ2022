using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    // How many bullets per second.
    public float rateOfFire;

    private float _lastFireTime = -1;
    
    public void Fire()
    {
        if (Time.time > _lastFireTime + (1 / rateOfFire))
        {
            _lastFireTime = Time.time;
            Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        }
    }
}
