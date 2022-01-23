using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CollectableController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1;
    [SerializeField]
    private float _scale = 1;
    [SerializeField]
    private Vector3 _rotateDirAndSpeed = Vector3.forward;
    [Space]
    [SerializeField, Range(1, 100)]
    private int _value;
    [SerializeField]
    private CollectableEnum _collectableType;

    private float _originY;
    private TextMeshPro[] _text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

	private void Awake()
	{
        _originY = transform.position.y;
        _text = GetComponentsInChildren<TextMeshPro>();
        if (_value > 1)
            foreach (TextMeshPro t in _text)
                t.SetText(_value.ToString());
	}

	// Update is called once per frame
	void Update()
    {
        transform.position = new Vector3(
            transform.position.x,
            _originY + (Mathf.Sin(Time.time * _speed) * _scale),
            transform.position.z);

        transform.Rotate(_rotateDirAndSpeed);
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
            other.TryGetComponent(out PlayerController player);
            player?.AddCollectable(_collectableType, _value);

            Destroy(gameObject);
		}
	}
}
