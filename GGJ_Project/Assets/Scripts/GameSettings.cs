using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Settings", menuName = "ScriptableObjects/GameSettingsScriptableObject", order = 1)]
public class GameSettings : ScriptableObject
{
    public GameObject playerPrefab;
    public GameObject cameraPrefab;
    public GameObject uiPrefab;
}
