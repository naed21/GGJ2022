using System.Collections.Generic;
using UnityEngine;

public static class EldritchVision
{
    public static List<EldritchObject> eldritchObjects = new List<EldritchObject>();

    private static bool _isActive = false;

    public static void Toggle()
    {
        _isActive = !_isActive;
        if (_isActive)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }
    
    private static void Activate()
    {
        foreach (var eo in eldritchObjects)
        {
            eo.SetVisible();
        }
    }

    private static void Deactivate()
    {
        foreach (var eo in eldritchObjects)
        {
            eo.SetInvisible();
        }
    }
}
