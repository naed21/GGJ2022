using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldButton : MonoBehaviour
{
    [SerializeField]
    private bool _startState;
    [SerializeField]
    private int _activationCost;
    [Space]
    //Actions when player enters/exits trigger
    [SerializeField]
    private UnityEvent _enterTriggerActions;
    [SerializeField]
    private UnityEvent _exitTriggerActions;
    [Space]
    //Actions when player activates/deactivates button
    [SerializeField]
    private UnityEvent _activeActions;
    [SerializeField]
    private UnityEvent _inactiveActions;

    private bool _isActive;
    private bool _playerNearby;

    //private CoinManager _playerCoins;

    // Start is called before the first frame update
    void Start()
    {
        
    }

	private void Awake()
	{
        _isActive = _startState;
	}

	// Update is called once per frame
	void Update()
    {
        if(_playerNearby && Input.GetKeyDown(KeyCode.E))
		{
            //if (_activationCost == 0)
                ToggleActivation();
            //else if(_activationCost > 0 && _playerCoins
            //  && _playerCoins.CoinsFound >= _activationCost)
            //  ToggleActivation();
		}
    }

    private void ToggleActivation()
	{
        if (_isActive)
            _inactiveActions.Invoke();
        else
            _activeActions.Invoke();

        _isActive = !_isActive;

        //if (_activationCost > 0 && _playerCoins)
        //    _playerCoins.UsedCoin(_activationCost);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
            _playerNearby = true;
            //other.TryGetComponent(out _playerCoins);
            _enterTriggerActions.Invoke();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player"))
		{
            _playerNearby = false;
            _exitTriggerActions.Invoke();
		}
	}
}
