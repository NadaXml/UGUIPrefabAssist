using UnityEditor;
using UnityEngine;

public abstract class ComponentMenuContext
{
    [MenuItem("CONTEXT/Component/Push To UGUI Style Preview")]
    public static void PushToUGUIStylePreview(MenuCommand cmd)
    {
        Component component = cmd.context as Component;
        if (component is RectTransform)
        {
            
        }
    }
}

