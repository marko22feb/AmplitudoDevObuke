using UnityEditor;
using UnityEngine;

public class NavGridEditorFunctions : EditorWindow
{
   [MenuItem("Window/Custom Functions/Nav Grid")]
   public static void ShowWindow()
    {
        GetWindow<NavGridEditorFunctions>("Nav Grid");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Create"))
        {

        }

        if (GUILayout.Button("Destroy"))
        {

        }
    }
}
