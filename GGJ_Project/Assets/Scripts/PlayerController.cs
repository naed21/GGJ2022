using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Gun), typeof(DamageReceiver))]
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
    private Gun _gun;
    private DamageReceiver _damageReceiver;

    private Animator _animController;

    public UIController ui;

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

    private Transform _transform;

    private void Awake()
	{
		DontDestroyOnLoad(this);
        _controller = GetComponent<CharacterController>();
        _animController = GetComponent<Animator>();
        _gun = GetComponent<Gun>();
        _damageReceiver = GetComponent<DamageReceiver>();
        _damageReceiver.onDamage += TakeDamage;
        _transform = transform;
    }

	private void Start()
	{
        if (ui != null)
        {
            ui.SetHealth(_health, _maxHealth);
            ui.SetMadness(_madness, _maxMadness);
            ui.SetUseItem(_useItem);
        }
    }

	// Update is called once per frame
	void Update()
    {
        if(_controller.isGrounded)
		{
            Vector3 _direction = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            _velocity = _direction * _speed;

            if (_direction.x > 0)
                _animController.SetTrigger("TrStrafeRight");
            else if (_direction.x < 0)
                _animController.SetTrigger("TrStrafeLeft");
            

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
            ui.SetMadness(_madness, _maxMadness);
            if(_madness >= _maxMadness)
			{
                _isEldritchVision = true;
                _isMaxMadness = true;
                ui.LockGoggles(true);
                ui.SetMaxMadness(true);
                EldritchVision.Activate();
            }
            else
                StartCoroutine(EldritchTime());
        }

        if(Input.GetKeyDown(KeyCode.F) && _useItem != CollectableEnum.None)
		{
            UseCollectable(_useItem);
            _useItem = CollectableEnum.None;
            ui.SetUseItem(CollectableEnum.None);
        }

        if (Input.GetMouseButtonDown(0))
        {
	        _gun.Fire();
        }

        var mouseWorldPos = Input.mousePosition;
        mouseWorldPos.z = GameManager.mainCameraController.transform.position.z * -1;
        mouseWorldPos = GameManager.mainCameraController.myCamera.ScreenToWorldPoint(mouseWorldPos);

        var playerOffsetPosition = _transform.position + Vector3.up * 0.9f;
        mouseWorldPos.z = playerOffsetPosition.z;
        var aimingDir = mouseWorldPos - playerOffsetPosition;
        aimingDir.Normalize();
        _gun.bulletSpawnPoint.transform.position = playerOffsetPosition + aimingDir;
        _gun.bulletSpawnPoint.transform.rotation = quaternion.LookRotation(aimingDir, Vector3.up);

        if(_isMaxMadness && !isTakingDamage)
		{
            StartCoroutine(TakeDamageOverTime());
		}
    }

	private void FixedUpdate()
	{
        _controller.Move(_velocity * Time.deltaTime);
	}


    bool isTakingDamage = false;
    public IEnumerator TakeDamageOverTime()
	{
        isTakingDamage = true;
        yield return new WaitForSeconds(1);
        TakeDamage(1);
        isTakingDamage = false;
    }

    public void AddCollectable(CollectableEnum collectable, int value)
	{
        //Throw error so we can fix it?
        if (ui == null)
            return;
        
        if(collectable == CollectableEnum.Key)
		{
            _keys += value;
            ui.SetKeyValue(_keys);
		}
        else if(collectable == CollectableEnum.Coin)
		{
            _coins += value;
            ui.SetCoinValue(_coins);
		}
        else if(collectable == CollectableEnum.Booze)
		{
			if (_useItem == CollectableEnum.None)
            {
                _useItem = CollectableEnum.Booze;
                ui.SetUseItem(CollectableEnum.Booze);
            }
            else
			{
                //TODO: Drop item instead of using
                UseCollectable(_useItem);

                _useItem = CollectableEnum.Booze;
                ui.SetUseItem(CollectableEnum.Booze);
			}
		}
        else if(collectable == CollectableEnum.Health)
		{
            if(_useItem == CollectableEnum.None)
			{
                _useItem = CollectableEnum.Health;
                ui.SetUseItem(CollectableEnum.Health);
			}
            else
			{
                //TODO: Drop item instead of using
                UseCollectable(_useItem);

                _useItem = CollectableEnum.Health;
                ui.SetUseItem(CollectableEnum.Health);
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
                    ui.SetMaxMadness(false);
                    EldritchVision.Deactivate();
                }
            }

            ui.SetMadness(_madness, _maxMadness);
        }
        else if(collectable == CollectableEnum.Health)
		{
            _health += _healthValue;

            if (_health > _maxHealth)
                _health = _maxHealth;

            ui.SetHealth(_health, _maxHealth);
		}
    }

    //Normal damage and extra damage if player decides it should take extra
    public void TakeDamage(int value)
	{
        _health -= value;
        ui.SetHealth(_health, _maxHealth);

        if (_health <= 0)
            Death();
	}

    public void TakeStress(int value)
	{
        _madness += value;
        //We can pass in negative values to reduce madness
        if(_madness <= 0)
		{
            _madness = 0;
            if(_isMaxMadness)
			{
                _isMaxMadness = false;
                ui.SetMaxMadness(false);
                EldritchVision.Deactivate();
            }
		}

        if(_madness > _maxMadness)
		{
            _madness = _maxMadness;

            _isEldritchVision = true;
            _isMaxMadness = true;
            ui.LockGoggles(true);
            ui.SetMaxMadness(true);
            EldritchVision.Activate();
        }

        ui.SetMadness(_madness, _maxMadness);
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
                ui.SetMaxMadness(false);
                EldritchVision.Deactivate();
            }    
		}

        ui.SetMadness(_madness, _maxMadness);
    }

    public void Death()
	{
        //TODO, Show Death UI? Button to restart. Tells player they earned coins
        //coins += currentLevel; //Make sure this carries over between levels
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
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

        if (!_isMaxMadness)
        {
            _isEldritchVision = false;
            EldritchVision.Toggle();
        }
    }

    /// <summary>
    /// Here be dragons!
    /// Because Unity is fucking dumb, why can't I just set the position of this transform and clear the CharacterController?
    /// Or just set the velocity value on the CharacterController, there is no reset function!
    /// So instead I have to do this dumb shit, save variables, destroy immediate the old controller, and make a new one with the saved
    /// variables... so dumb!
    /// </summary>
    /// <param name="position"></param>
    public void Respawn(Vector3 position)
    {
	    _velocity = Vector3.zero;
	    _velocityY = 0;
	    _transform.position = position;
	    
	    var center = _controller.center;
	    var height = _controller.height;
	    var radius = _controller.radius;
	    var slopeLimit = _controller.slopeLimit;
	    var stepOffset = _controller.stepOffset;
	    var skinWidth = _controller.skinWidth;
	    var minMoveDistance = _controller.minMoveDistance;

	    DestroyImmediate(_controller);
	    _controller = null;
	    
	    var fucker = gameObject.AddComponent<CharacterController>();

	    fucker.center = center;
	    fucker.height = height;
	    fucker.radius = radius;
	    fucker.slopeLimit = slopeLimit;
	    fucker.stepOffset = stepOffset;
	    fucker.skinWidth = skinWidth;
	    fucker.minMoveDistance = minMoveDistance;
	    
	    _controller = fucker;
    }
}
