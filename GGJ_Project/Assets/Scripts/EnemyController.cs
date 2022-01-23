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
    [SerializeField]
    private float _attackDelay;
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

    private bool _canSeePlayer;
    private bool _inAttackRange;
    private bool _attacking;

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

        _canSeePlayer = false;

        var distance = CheckCanSeeAndGetDistance();

        if (distance < _attackRange)
            _inAttackRange = true;
        else
            _inAttackRange = false;

        if (_enemyType == EnemyTypeEnum.Landmine)
        {
            //Explode!
            if(!_attacking && _inAttackRange)
			{
                StartCoroutine(WaitForAttack(_attackDelay));
			}
        }
    }

    public float CheckCanSeeAndGetDistance()
	{
        var distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance < _viewRange)
        {
            Vector3 direction = (_player.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, direction);
            if (angle < _viewAngle / 2)
            {
                //Is there anything in the way of the player
                if (!Physics.Linecast(transform.position, _player.transform.position))
                {
                    _canSeePlayer = true;
                }
            }
        }

        return distance;
    }

    public IEnumerator WaitForAttack(float time)
	{
        _attacking = true;

        yield return new WaitForSeconds(time);

        _attacking = false;

        Attack();
	}

    public void Attack()
	{
        if(_enemyType == EnemyTypeEnum.Landmine)
		{
            var distance = CheckCanSeeAndGetDistance();

            if (distance < _attackRange)
                _inAttackRange = true;
            else
                _inAttackRange = false;

            if (_inAttackRange)
            {
                _player.TakeDamage(_attackDamage + (_canSeePlayer ? _extraDamage : 0));

                _player.TakeStress(_stressDamage + (_canSeePlayer ? _extraStress : 0));
            }

            if (_canSeePlayer)
                Debug.Log("Full Damage!");

            Death(false);
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
        else
		{
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
