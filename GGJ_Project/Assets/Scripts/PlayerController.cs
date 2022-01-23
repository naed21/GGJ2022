using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Min(0)]
    private float _speed = 5f;
    [SerializeField, Min(0)]
    private float _jumpHeight = 5f;
    [SerializeField, Min(0)]
    private float _gravity = 1f;
    [SerializeField, Min(0)]
    private float _airborneSpeed = 2.5f;
    [SerializeField, Min(0)]
    private float _eldritchTime;
    [Space]
    [SerializeField, Min(0)]
    private int _keys;
    [SerializeField, Min(0)]
    private int _coins;
    [Space]
    [SerializeField, Min(1)]
    private int _health = 100;
    [SerializeField, Min(1)]
    private int _maxHealth = 100;
    [SerializeField, Min(0)]
    private int _madness = 0;
    [SerializeField, Min(1)]
    private int _maxMadness = 100;

    //Used only for Move() and isGrounded
    private CharacterController _controller;
    //Target velocity
    private Vector3 _velocity;
    //Seperate Y due to gravity and jump calcs
    private float _velocityY;
    //local variable for CharacterController isGrounded
    private bool _wasGrounded;
    private bool _isEldritchVision;
    private bool _isMaxMadness = false;

    private UIController _ui;

    [Space]
    [SerializeField]
    private CollectableEnum _useItem = CollectableEnum.None;
    [Space]
    //How much madness is reduced when using booze
    [SerializeField, Min(0)]
    private int _boozeValue = 20;
    //How much health is restored when using health item
    [SerializeField, Min(0)]
    private int _healthValue = 10;
    //How much madness increases when using goggles
    [SerializeField, Min(0)]
    private int _goggleValue = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        Canvas target = null;
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach(var can in canvases)
            if(can.CompareTag("MainUI"))
			{
                target = can;
                break;
			}

        _ui = target.GetComponent<UIController>();
    }

	private void Awake()
	{
        _controller = GetComponent<CharacterController>();
	}

	// Update is called once per frame
	void Update()
    {
        if(_controller.isGrounded)
		{
            Vector3 _direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            _velocity = _direction * _speed;

            if (!_controller.isGrounded && _wasGrounded)
                BecomeAirborne();
            if (_controller.isGrounded && !_wasGrounded)
                BecomeGrounded();

            if (Input.GetKeyDown(KeyCode.Space))
                _velocityY = _jumpHeight;
		}
        else
		{
            Vector3 _direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            _velocity = _direction * _airborneSpeed;

            if (_controller.collisionFlags == CollisionFlags.CollidedAbove)
                _velocityY = Mathf.Min(0, _velocityY);

            //Clamp downward velocity
            if (_velocityY > -30)
                _velocityY -= _gravity;
		}

        _velocity.y = _velocityY;
        _wasGrounded = _controller.isGrounded;

        if (Input.GetKeyDown(KeyCode.Q) && !_isEldritchVision && !_isMaxMadness)
        {
            _madness += _goggleValue;
            if(_madness >= _maxMadness)
			{
                _isEldritchVision = true;
                _isMaxMadness = true;
                _ui.LockGoggles(true);
			}
            else
                StartCoroutine(EldritchTime());
        }

        if(Input.GetKeyDown(KeyCode.F) && _useItem != CollectableEnum.None)
		{
            UseCollectable(_useItem);
            _useItem = CollectableEnum.None;
            _ui.SetUseItem(CollectableEnum.None);
        }
    }

	private void FixedUpdate()
	{
        _controller.Move(_velocity * Time.deltaTime);
	}

    public void AddCollectable(CollectableEnum collectable, int value)
	{
        //Throw error so we can fix it?
        if (_ui == null)
            return;
        
        if(collectable == CollectableEnum.Key)
		{
            _keys += value;
            _ui.SetKeyValue(_keys);
		}
        else if(collectable == CollectableEnum.Coin)
		{
            _coins += value;
            _ui.SetCoinValue(_coins);
		}
        else if(collectable == CollectableEnum.Booze)
		{
            if (_useItem == CollectableEnum.None)
            {
                _useItem = CollectableEnum.Booze;
                _ui.SetUseItem(CollectableEnum.Booze);
            }
            else
			{
                //TODO: Drop item instead of using
                UseCollectable(_useItem);

                _useItem = CollectableEnum.Booze;
                _ui.SetUseItem(CollectableEnum.Booze);
			}
		}
        else if(collectable == CollectableEnum.Health)
		{
            if(_useItem == CollectableEnum.None)
			{
                _useItem = CollectableEnum.Health;
                _ui.SetUseItem(CollectableEnum.Health);
			}
            else
			{
                //TODO: Drop item instead of using
                UseCollectable(_useItem);

                _useItem = CollectableEnum.Health;
                _ui.SetUseItem(CollectableEnum.Health);
			}
		}
	}

    public void UseCollectable(CollectableEnum collectable)
	{        
        if (collectable == CollectableEnum.Booze)
        {
            //lower madness
            _madness -= _boozeValue;

            if (_madness <= 0)
            {
                _madness = 0;

                if (_isMaxMadness)
                {
                    _isMaxMadness = false;
                    _ui.SetMaxMadness(false);
                }
            }

            _ui.SetMadness(_madness, _maxMadness);
        }
        else if(collectable == CollectableEnum.Health)
		{
            _health += _healthValue;

            if (_health > _maxHealth)
                _health = _maxHealth;

            _ui.SetHealth(_health, _maxHealth);
		}
    }

    public void TakeDamage(int value)
	{
        _health -= value;
        _ui.SetHealth(_health, _maxHealth);

        if (_health <= 0)
            Death();
	}

    public void EnemyDeath(EnemyTypeEnum enemyType, int value)
	{
        //Value = amount of madness reduced
        //TODO, should the enemy Type be replaced with enemy controller?

        _madness -= value;
        
        if (_madness <= 0)
		{
            _madness = 0;
            if(_isMaxMadness)
			{
                _isMaxMadness = false;
                _ui.SetMaxMadness(false);
			}    
		}

        _ui.SetMadness(_madness, _maxMadness);
    }

    public void Death()
	{
        //TODO, Show Death UI? Button to restart. Tells player they earned coins
        //coins += currentLevel; //Make sure this carries over between levels
	}

    private void BecomeAirborne()
	{
        //was in copied code, idk what it's suppose to be for
	}

    private void BecomeGrounded()
    {
        //was in copied code, idk what it's suppose to be for
    }

    private IEnumerator EldritchTime()
    {
	    EldritchVision.Toggle();
	    _isEldritchVision = true;

	    yield return new WaitForSeconds(_eldritchTime);

	    _isEldritchVision = false;
	    EldritchVision.Toggle();
    }
}
