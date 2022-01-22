using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class EldritchObject : MonoBehaviour
{
    [SerializeField]
    private Material _invisibleMat;

    [SerializeField]
    private Material _visibleMat;

    [SerializeField]
    private MeshRenderer _mesh;

    /// <summary>
    /// Will enable on/off matching the visible/invisible state.
    /// </summary>
    [SerializeField]
    private List<Behaviour> _extraObjects;

    private void Awake()
    {
        if (_mesh == null)
        {
            Debug.LogError("Needs a mesh to function!");
            Destroy(this);
        }
    }

    private void Start()
    {
        EldritchVision.eldritchObjects.Add(this);
        SetInvisible();
    }

    public void SetVisible()
    {
        _mesh.material = _visibleMat;
        SetExtraObjectsEnabled(true);
    }

    public void SetInvisible()
    {
        _mesh.material = _invisibleMat;
        SetExtraObjectsEnabled(false);
    }

    public void OnDestroy()
    {
        EldritchVision.eldritchObjects.Remove(this);
    }

    private void SetExtraObjectsEnabled(bool isEnabled)
    {
        foreach (var extraObject in _extraObjects)
        {
            extraObject.enabled = isEnabled;
        }
    }
}
