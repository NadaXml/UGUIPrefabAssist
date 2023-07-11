using System;
using UnityEditor;
using UnityEngine;

public class UGUIStyleGallery : EditorWindow
{
    [MenuItem("UGUIStyleGallery/Gallery")]
    private static void ShowWindow()
    {
        var window = GetWindow<UGUIStyleGallery>();
        window.titleContent = new GUIContent("TITLE");
        window.Show();
    }

    UGUIStyleBase styleBase;
    void OnEnable()
    {
        
    }

    void OnDisable()
    {
    }

    private void OnGUI()
    {
        
    }
}

