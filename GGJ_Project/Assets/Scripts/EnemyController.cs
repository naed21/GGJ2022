using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private EnemyTypeEnum _enemyType;

    [SerializeField]
    private float _viewRange;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private float _viewAngle;
    [Space]
    [SerializeField, Min(0)]
    private int _attackDamage;
    [SerializeField, Min(0)]
    private int _extraDamage;
    [SerializeField, Min(0)]
    private int _stressDamage;
    [SerializeField, Min(0)]
    private int _extraStress;
    [Space]
    [SerializeField, Min(0)]
    private int _stressHealOnKill;
    [SerializeField]
    private Dictionary<CollectableEnum, int> _lootTable;
    [Space]
    [SerializeField, Min(1)]
    private int _health;

    private PlayerController _player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_player == null)
		{
            //find player
            _player = FindObjectOfType<PlayerController>();

            //skip update, no player
            if (_player == null)
                return;
		}
        
        if(_enemyType == EnemyTypeEnum.Landmine)
		{
            //AI

		}
    }

	private void FixedUpdate()
	{
        //Skip if no player
        if (_player == null)
            return;

        bool canSeePlayer = false;
        bool inAttackRange = false;

        var distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance < _viewRange)
		{
            Vector3 direction = (_player.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, direction);
            if(angle < _viewAngle / 2)
			{
                //Is there anything in the way of the player
                if(!Physics.Linecast(transform.position, _player.transform.position))
				{
                    canSeePlayer = true;
				}
			}
		}

        if (distance < _attackRange)
            inAttackRange = true;

        if (_enemyType == EnemyTypeEnum.Landmine)
        {
            //Explode!
            if(inAttackRange)
			{
                _player.TakeDamage(_attackDamage + (canSeePlayer ? _extraDamage : 0));

                _player.TakeStress(_stressDamage + (canSeePlayer ? _extraStress : 0));

                if (canSeePlayer)
                    Debug.Log("Full Damage!");

                Death(false);
			}
        }
    }

    public void TakeDamage(int value)
	{
        _health -= value;

        if(_health <= 0)
		{
            Death();
		}
	}

    public void Death(bool rewardPlayer = true)
	{
        if(rewardPlayer)
		{
            //TODO: loot table, define items that can drop

            _player.TakeStress(-_stressHealOnKill);

            Destroy(this.gameObject);
		}
	}

	private void OnDrawGizmos()
	{
		if(_enemyType == EnemyTypeEnum.Landmine)
		{
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(this.transform.position, _viewRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, _attackRange);
		}
	}
}
