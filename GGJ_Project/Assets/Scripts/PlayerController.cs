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

    //Used only for Move() and isGrounded
    private CharacterController _controller;
    //Target velocity
    private Vector3 _velocity;
    //Seperate Y due to gravity and jump calcs
    private float _velocityY;
    //local variable for CharacterController isGrounded
    private bool _wasGrounded;
    
    // Start is called before the first frame update
    void Start()
    {
        
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
    }

	private void FixedUpdate()
	{
        _controller.Move(_velocity * Time.deltaTime);
	}

    public void FoundCollectable(int value)
	{

	}

    private void BecomeAirborne()
	{

	}

    private void BecomeGrounded()
    {

    }
}
