using System;
using UnityEngine;

public class EldritchObject : MonoBehaviour
{
    [SerializeField]
    private Material _invisibleMat;

    [SerializeField]
    private Material _visibleMat;

    [SerializeField]
    private MeshRenderer _mesh;

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
    }

    public void SetInvisible()
    {
        _mesh.material = _invisibleMat;
    }

    public void OnDestroy()
    {
        EldritchVision.eldritchObjects.Remove(this);
    }
}
