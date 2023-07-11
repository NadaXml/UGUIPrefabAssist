using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;

[CreateAssetMenu(fileName = "UGUIStyleBase", menuName = "UIPrefab/UGUIStyleBase")]
public class UGUIStyleBase : ScriptableObject
{
    public List<Preset> PresetList;
}

