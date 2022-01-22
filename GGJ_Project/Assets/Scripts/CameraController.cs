using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform _player;
    [SerializeField]
    private Vector3 _cameraOffset;
    [SerializeField]
    private Vector2 _cameraSmoothing;
    [SerializeField]
    private Vector2 _cameraDeadZone;

    private Vector3 _cameraPos;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void FixedUpdate()
	{
        CalculateCameraPosition();
        transform.position = _cameraPos;
	}

    private void CalculateCameraPosition()
	{
        //Calc x and y with simple linear interpolation
        // between the current and target position + offset,
        // while smoothing the value
        _cameraPos.x = Mathf.Lerp(
            transform.position.x,
            _player.position.x + _cameraOffset.x,
            Time.deltaTime * _cameraSmoothing.x);

        _cameraPos.y = Mathf.Lerp(
            transform.position.y,
            _player.position.y + _cameraOffset.y,
            Time.deltaTime * _cameraSmoothing.y);

        //Clamp camera position inside the deadzone
        if (_cameraPos.x - _player.position.x > _cameraDeadZone.x)
            _cameraPos.x = _player.position.x + _cameraDeadZone.x;
        else if (_cameraPos.x - _player.position.x < -_cameraDeadZone.x)
            _cameraPos.x = _player.position.x - _cameraDeadZone.x;

        if (_cameraPos.y - _player.position.y > _cameraDeadZone.y)
            _cameraPos.y = _player.position.y + _cameraDeadZone.y;
        else if (_cameraPos.y - _player.position.y < -_cameraDeadZone.y)
            _cameraPos.y = _player.position.y - _cameraDeadZone.y;

        _cameraPos.z = _cameraOffset.z;
    }
}
