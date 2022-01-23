using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private List<Image> _allImages;
    private List<Text> _allText;

    private Image _healthRef;
    private Image _madnessRef;
    private Image _madnessBackRef;
    private Image _gunCoolDownRef;
    private Image _goggleCoolDownRef;
    private Image _itemCoolDownRef;

    private Text _healthTextRef;
    private Text _madnessTextRef;
    private Text _coinTextRef;
    private Text _keyTextRef;

    private Vector2 _madnessStartSize;
    private Vector2 _healthStartSize;
    private Vector2 _gunCdStartSize;
    private Vector2 _goggleCdStartSize;
    private Vector2 _itemCdStartSize;

    //private Vector3 _madnessStartPos;
    //private Vector3 _healthStartPos;

    private bool _maxMadness = false;

    // Start is called before the first frame update
    void Start()
    {
        SetHealth(1, 1000);
    }

	private void Awake()
	{
        _allImages = new List<Image>(GetComponentsInChildren<Image>());
        _allText = new List<Text>(GetComponentsInChildren<Text>());

        _healthRef = _allImages.First(x => x.name == "Health");
        _madnessRef = _allImages.First(x => x.name == "Madness");
        _madnessBackRef = _allImages.First(x => x.name == "MadnessBackground");
        _gunCoolDownRef = _allImages.First(x => x.name == "GunCoolDown");
        _goggleCoolDownRef = _allImages.First(x => x.name == "GoggleCoolDown");
        _itemCoolDownRef = _allImages.First(x => x.name == "ItemCoolDown");

        _healthTextRef = _allText.First(x => x.name == "HealthValue");
        _madnessTextRef = _allText.First(x => x.name == "MadnessValue");
        _coinTextRef = _allText.First(x => x.name == "CoinValue");
        _keyTextRef = _allText.First(x => x.name == "KeyValue");

        _healthStartSize = _healthRef.rectTransform.sizeDelta;
        _madnessStartSize = _madnessRef.rectTransform.sizeDelta;
        _gunCdStartSize = _gunCoolDownRef.rectTransform.sizeDelta;
        _goggleCdStartSize = _goggleCoolDownRef.rectTransform.sizeDelta;
        _itemCdStartSize = _itemCoolDownRef.rectTransform.sizeDelta;

        //_healthStartPos = _healthRef.rectTransform.position;
        //_madnessStartPos = _madnessRef.rectTransform.position;

        _madnessBackRef.enabled = false;
    }

    //int test = 900;
	// Update is called once per frame
	void Update()
    {
        //SetMadness(test++, 1000);

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

        _healthTextRef.text = amount.ToString() + "/" + max.ToString();
	}

    public void SetMadness(int amount, int max)
	{
        if(amount >= max)
		{
            amount = max; 
		}
        
        float percent = (float)amount / (float)max;

        float newWidth = _madnessStartSize.x * percent;

        _madnessRef.rectTransform.sizeDelta = new Vector2(newWidth, _madnessStartSize.y);

        _madnessTextRef.text = amount.ToString() + "/" + max.ToString();
	}

    public void SetMaxMadness(bool value)
	{
        _maxMadness = value;
        _madnessBackRef.enabled = value;
    }

    public void SetGunCoolDown(int amount, int max)
    {
        if (amount > max)
            amount = max;

        float percent = (float)amount / (float)max;

        float newWidth = _gunCdStartSize.x * percent;

        _gunCoolDownRef.rectTransform.sizeDelta = new Vector2(newWidth, _gunCdStartSize.y);
    }

    public void SetGoggleCoolDown(int amount, int max)
    {
        if (amount > max)
            amount = max;

        float percent = (float)amount / (float)max;

        float newWidth = _goggleCdStartSize.x * percent;

        _goggleCoolDownRef.rectTransform.sizeDelta = new Vector2(newWidth, _goggleCdStartSize.y);
    }

    public void SetItemCoolDown(int amount, int max)
    {
        if (amount > max)
            amount = max;

        float percent = (float)amount / (float)max;

        float newWidth = _itemCdStartSize.x * percent;

        _itemCoolDownRef.rectTransform.sizeDelta = new Vector2(newWidth, _itemCdStartSize.y);
    }

    public void SetKeyValue(int amount)
	{
        _keyTextRef.text = amount.ToString();
	}

    public void SetCoinValue(int amount)
	{
        _coinTextRef.text = amount.ToString();
	}

    public void SetUseItem(CollectableEnum collectable)
	{
        //TODO, show that item's image in the itemRef
	}

    public void LockGoggles(bool setLock)
	{
        if(setLock)
		{

		}
        else
		{

		}
	}
}
