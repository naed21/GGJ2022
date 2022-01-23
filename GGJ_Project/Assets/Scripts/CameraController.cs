using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public Transform player;
    [SerializeField]
    private Vector3 _cameraOffset;
    [SerializeField]
    private Vector2 _cameraSmoothing;
    [SerializeField]
    private Vector2 _cameraDeadZone;

    private Vector3 _cameraPos;
    
    public Camera myCamera;

    private void Awake()
    {
        myCamera = GetComponent<Camera>();
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
            player.position.x + _cameraOffset.x,
            Time.deltaTime * _cameraSmoothing.x);

        _cameraPos.y = Mathf.Lerp(
            transform.position.y,
            player.position.y + _cameraOffset.y,
            Time.deltaTime * _cameraSmoothing.y);

        //Clamp camera position inside the deadzone
        if (_cameraPos.x - player.position.x > _cameraDeadZone.x)
            _cameraPos.x = player.position.x + _cameraDeadZone.x;
        else if (_cameraPos.x - player.position.x < -_cameraDeadZone.x)
            _cameraPos.x = player.position.x - _cameraDeadZone.x;

        if (_cameraPos.y - player.position.y > _cameraDeadZone.y)
            _cameraPos.y = player.position.y + _cameraDeadZone.y;
        else if (_cameraPos.y - player.position.y < -_cameraDeadZone.y)
            _cameraPos.y = player.position.y - _cameraDeadZone.y;

        _cameraPos.z = _cameraOffset.z;
    }
}
