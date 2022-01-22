using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateSetter : MonoBehaviour
{
    public int frameRate = 60;
    
    private void Start()
    {
        Application.targetFrameRate = frameRate;
    }
}
