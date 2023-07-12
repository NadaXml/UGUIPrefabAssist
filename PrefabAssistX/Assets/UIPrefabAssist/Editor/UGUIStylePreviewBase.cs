using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

[CustomEditor(typeof(UGUIStyleBase))]
public class UGUIStylePreviewBase : Editor
{
    GameObject templateGameObject;
    PreviewRenderUtility previewRender;

    Preset preset;

    Canvas canvasInstance;
    
    private UnityEditorInternal.ReorderableList presetList;
    
    UGUIStyleBase Target;
    
    void OnEnable()
    {
        presetList = null;
        
        Target = target as UGUIStyleBase;

        string path = @"Assets/UIPrefabAssist/Style/Canvas.prefab";
        templateGameObject = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        previewRender = new PreviewRenderUtility();
        previewRender.camera.backgroundColor = Color.grey;
        previewRender.camera.clearFlags = CameraClearFlags.SolidColor;
        previewRender.camera.cameraType = CameraType.Game;
        previewRender.camera.farClipPlane = 1000f;
        previewRender.camera.nearClipPlane = 0.1f;
        
        canvasInstance = previewRender.InstantiatePrefabInScene(templateGameObject).GetComponent<Canvas>();
        canvasInstance.renderMode = RenderMode.ScreenSpaceCamera;
        canvasInstance.worldCamera = previewRender.camera;

        SetupPresetList();
        
        UpdatePreviewPreset();
    }

    void OnDisable()
    {
        if(previewRender != null)
        {
            previewRender.camera.targetTexture = null;
            previewRender.Cleanup();
            previewRender = null;
        }
    }

    public override bool HasPreviewGUI()
    {
        return true;
    }

    // public override void OnPreviewGUI(Rect r, GUIStyle background)
    // {
    //     if (Event.current.type == EventType.Repaint)
    //     {
    //         previewRender.BeginPreview(r, background);
    //         previewRender.Render();
    //         var texture = previewRender.EndPreview();
    //     }
    // }

    void UpdatePreviewPreset()
    {
        // TODO child override
        // TODO Only single passed or multi
        if (preset != null)
        {
            UnityEngine.Object obj = null;
            var targetFullName = preset.GetTargetFullTypeName();
            if ("UI.RectTransform" == targetFullName)
            {
                obj = canvasInstance.transform.Find("Content").GetComponent<RectTransform>();
            }
            else if (typeof(Image).FullName == targetFullName)
            {
                obj = canvasInstance.transform.Find("Content").GetComponent<Image>();
            }
            
            if (preset.CanBeAppliedTo(obj))
            {
                preset.ApplyTo(obj);
            }
            else
            {
                Debug.LogError($"无法应用Preset");
            }
        }
    }

    public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
        if (Event.current.type == EventType.Repaint)
        {
            previewRender.BeginPreview(r, background);
            previewRender.Render();
            previewRender.EndAndDrawPreview(r);
        }
        
        // child override
        {
            Rect rLabel = new Rect(r.x, -r.height * 0.5f + 100f, r.width, r.height);
            GUI.Label(rLabel, "frame = width = 300 height = 300");
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        presetList.DoLayoutList();

        if (GUILayout.Button("应用到选中的GameObject"))
        {
            if (preset != null)
            {
                var image = Selection.activeGameObject.GetComponent<Image>();
                var rectTransform = Selection.activeGameObject.GetComponent<RectTransform>();
                
                if (preset.CanBeAppliedTo(image) )
                {
                    preset.ApplyTo(image);
                }
                else if (preset.CanBeAppliedTo(rectTransform))
                {
                    preset.ApplyTo(rectTransform);
                }
            }
        }
        serializedObject.ApplyModifiedProperties();
    }

    void SetupPresetList()
    {
        // TODO child override 
        float vSpace = 2;
        presetList = new UnityEditorInternal.ReorderableList(serializedObject, serializedObject.FindProperty("PresetList"), true, true, true, true);

        presetList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "预设列表");
        };

        presetList.onSelectCallback = (UnityEditorInternal.ReorderableList list) =>
        {
            var tempPreset = Target.PresetList[list.index];
            bool dirty = false;
            if (preset == null)
            {
                preset = tempPreset;
                dirty = true;
            }
            else if ( preset != tempPreset)
            {
                preset = tempPreset;
                dirty = true;
            }
            if (dirty)
            {
                UpdatePreviewPreset();
            }
        };

        presetList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            rect.y += vSpace;
            Vector2 pos = rect.position;
            rect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty element
                = presetList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, element, GUIContent.none);
        };
            
        // presetList.onChangedCallback = (UnityEditorInternal.ReorderableList l) =>
        // {
        //     if (l.index < 0 || l.index >= l.serializedProperty.arraySize)
        //         return;
        //     UnityEngine.Object o = l.serializedProperty.GetArrayElementAtIndex(
        //             l.index).objectReferenceValue;
        //     // CinemachineVirtualCameraBase vcam = (o != null)
        //     //     ? (o as CinemachineVirtualCameraBase) : null;
        //     // if (vcam != null)
        //     //     vcam.transform.SetSiblingIndex(l.index);
        // };
        
        // presetList.onAddCallback = (UnityEditorInternal.ReorderableList l) =>
        // {
        //     var index = l.serializedProperty.arraySize;
        //     // var vcam = CinemachineMenu.CreateDefaultVirtualCamera();
        //     // Undo.SetTransformParent(vcam.transform, Target.transform, "");
        //     // vcam.transform.SetSiblingIndex(index);
        // };
        
        // presetList.onRemoveCallback = (UnityEditorInternal.ReorderableList l) =>
        // {
        //     UnityEngine.Object o = l.serializedProperty.GetArrayElementAtIndex(
        //             l.index).objectReferenceValue;
        //     // CinemachineVirtualCameraBase vcam = (o != null)
        //     //     ? (o as CinemachineVirtualCameraBase) : null;
        //     // if (vcam != null)
        //     //     Undo.DestroyObjectImmediate(vcam.gameObject);
        // };
    }
}
