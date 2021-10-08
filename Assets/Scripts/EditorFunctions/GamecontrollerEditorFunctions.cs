using UnityEditor;
using UnityEngine;

public class GamecontrollerEditorFunctions : EditorWindow
{
   [MenuItem("Window/Custom Functions/Game Controller")]
   public static void ShowWindow()
    {
        GetWindow<GamecontrollerEditorFunctions>("Game Controller");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Save Item Data"))
        {
            GameObject.Find("GameController").GetComponent<GameController>().SaveItemsData();
        }

        if (GUILayout.Button("Load Item Data"))
        {
            Debug.Log(System.DateTime.Now);
        }
    }
}
