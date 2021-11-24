using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView graphView;
    public string DialogueName = "New Dialogue";

    [MenuItem("Graph/Dialogue Graph/Open Dialogue Graph")]
    public static void CreateGraphViewWindow()
    {
        EditorWindow window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Dialogue Graph");
    }

    private void ConstructGraphView()
    {
        graphView = new DialogueGraphView { name = "Dialogue Graph" };
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    private void ConstructMenuToolBar()
    {
        Toolbar toolbar = new Toolbar();

        TextField DialogueNameTextField = new TextField("Dialogue Name : ");
        DialogueNameTextField.SetValueWithoutNotify(DialogueName);

        DialogueNameTextField.RegisterValueChangedCallback(evt =>
        {
            DialogueName = evt.newValue;
        }
        );

        toolbar.Add(DialogueNameTextField);

        Button SaveButton = new Button(clickEvent: () => { SaveDialogue(DialogueName); });
        SaveButton.text = "Save Dialogue";

        toolbar.Add(SaveButton);

        Button LoadButton = new Button(clickEvent: () => { LoadDialogue(DialogueName); });
        LoadButton.text = "Load Dialogue";

        toolbar.Add(LoadButton);

        rootVisualElement.Add(toolbar);
    }

    private void ConstructToolBar()
    {
        Toolbar toolbar = new Toolbar();

        Button CreateNewNodeButton = new Button(clickEvent: () => { graphView.CreateNewDialogueNode("New Dialogue", new Vector2(600, 600)); });
        CreateNewNodeButton.text = "Create New Node";

        toolbar.Add(CreateNewNodeButton);

        rootVisualElement.Add(toolbar);
    }

    private void OnEnable()
    {
        ConstructGraphView();
        ConstructMenuToolBar();
        ConstructToolBar();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }

    public static void SaveDialogue(string FileName)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        DialogueSave save = new DialogueSave();

        FileStream file = File.Create(Application.dataPath + "/Resources/Dialogues/" + FileName + ".bytes");

        Dialogue dial = GameObject.Find("DialogueCanvas").GetComponent<Dialogue>();

        save.NPC_Dialogues = dial.NPC_Dialogues;
        save.Player_Choices = dial.Player_Choices;
        save.Choice_Leades_To = dial.Choice_Leades_To;
        save.Dialogue_Leades_ToChoices = dial.Dialogue_Leades_ToChoices;
        save.Choice_Conditions = dial.Choice_Conditions;
        save.Choice_Event = dial.Choice_Event;

        binaryFormatter.Serialize(file, save);
        file.Close();

        AssetDatabase.Refresh();
    }

    public static void LoadDialogue(string FileName)
    {
        if (File.Exists(Application.dataPath + "/Resources/Dialogues/" + FileName + ".bytes"))
        {

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/Resources/Dialogues/" + "Dialogue1" + ".bytes", FileMode.Open);
            DialogueSave save = (DialogueSave)binaryFormatter.Deserialize(file);

            Dialogue dial = GameObject.Find("DialogueCanvas").GetComponent<Dialogue>();

            dial.NPC_Dialogues = save.NPC_Dialogues;
            dial.Player_Choices = save.Player_Choices;
            dial.Choice_Leades_To = save.Choice_Leades_To;
            dial.Dialogue_Leades_ToChoices = save.Dialogue_Leades_ToChoices;
            dial.Choice_Conditions = save.Choice_Conditions;
            dial.Choice_Event = save.Choice_Event;

            file.Close();

        }
    }
}

[System.Serializable]
public class DialogueSave
{
    public List<string> NPC_Dialogues;
    public List<string> Player_Choices;

    public List<int> Choice_Leades_To;
    public List<IntList> Dialogue_Leades_ToChoices;

    public List<int> Choice_Conditions;
    public List<int> Choice_Event;
}
