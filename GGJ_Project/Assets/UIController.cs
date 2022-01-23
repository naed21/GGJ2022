using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private List<Image> _allImages;
    private Image _healthRef;
    private Image _madnessRef;
    private Image _madnessBackRef;

    private Vector2 _madnessStartSize;
    private Vector2 _healthStartSize;

    private Vector3 _madnessStartPos;
    private Vector3 _healthStartPos;

    private bool _maxMadness = false;

    // Start is called before the first frame update
    void Start()
    {
        SetHealth(1, 1000);
    }

	private void Awake()
	{
        _allImages = new List<Image>(GetComponentsInChildren<Image>());

        _healthRef = _allImages.First(x => x.name == "Health");
        _madnessRef = _allImages.First(x => x.name == "Madness");
        _madnessBackRef = _allImages.First(x => x.name == "MadnessBackground");

        _healthStartSize = _healthRef.rectTransform.sizeDelta;
        _madnessStartSize = _madnessRef.rectTransform.sizeDelta;

        _healthStartPos = _healthRef.rectTransform.position;
        _madnessStartPos = _madnessRef.rectTransform.position;

        _madnessBackRef.enabled = false;
    }

    int test = 900;
	// Update is called once per frame
	void Update()
    {
        SetMadness(test++, 1000);

        if(_maxMadness)
		{
            float size = Mathf.Sin(Time.time * 10) + 1; //0 -> 2
            var percent = size / 2;

            _madnessRef.rectTransform.sizeDelta = new Vector2(_madnessRef.rectTransform.sizeDelta.x, _madnessStartSize.y * percent);
		}
    }

    public void SetHealth(int amount, int max)
	{
        if (amount > max)
            amount = max;
        
        float percent = (float)amount / (float)max;

        float newWidth = _healthStartSize.x * percent;

        _healthRef.rectTransform.sizeDelta = new Vector2(newWidth, _healthStartSize.y);

        //float diff = _healthStartSize.x - newWidth;

		//_healthRef.rectTransform.position = new Vector3(_healthStartPos.x + newWidth,
		//	_healthStartPos.y,
		//	_healthStartPos.z);
	}

    public void SetMadness(int amount, int max)
	{
        if(amount >= max)
		{
            amount = max;
            _maxMadness = true;
            _madnessBackRef.enabled = true;
		}
        else if(_maxMadness)
		{
            _maxMadness = false;

		}
        
        float percent = (float)amount / (float)max;

        float newWidth = _madnessStartSize.x * percent;

        //float diff = _madnessStartSize.x - newWidth;

        _madnessRef.rectTransform.sizeDelta = new Vector2(newWidth, _madnessStartSize.y);
        //_madnessRef.rectTransform.localPosition = new Vector3(diff + _madnessRef.rectTransform.localPosition.x,
        //    _madnessRef.rectTransform.localPosition.y,
        //    _madnessRef.rectTransform.localPosition.z);
	}
}
