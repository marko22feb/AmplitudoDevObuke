using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView graphView;
    public string DialogueName = "New Dialogue";

    private List<Edge> edges;
    private List<DialogueNode> nodes;

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

    private void SaveDialogue(string FileName)
    {
        edges = graphView.edges.ToList();

        if (edges.Any())
        {
            nodes = graphView.nodes.Cast<DialogueNode>().ToList();

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            DialogueData data = new DialogueData();

            if (!AssetDatabase.IsValidFolder("Assets/Resources/Dialogues"))
                AssetDatabase.CreateFolder("Assets/Resources", "Dialogues");

            FileStream file = File.Create(Application.dataPath + "/Resources/Dialogues/" + FileName + ".bytes");

            Edge[] connectedPorts = edges.ToArray();

            for (int i = 0; i < connectedPorts.Count(); i++)
            {
                DialogueNode outputNode = (connectedPorts[i].output.node as DialogueNode);
                DialogueNode inputNode = (connectedPorts[i].input.node as DialogueNode);

                data.NodeEdges.Add(new NodeEdgeData
                {
                    CurrentNodeGUID = outputNode.GUID,
                    NextNodeGUID = inputNode.GUID,
                    PortName = connectedPorts[i].output.portName
                }) ;
            }

            foreach (DialogueNode node in nodes)
            {
                data.NodeData.Add(new NodeData
                {
                    EntryNode = node.EntryNode,
                    GUID = node.GUID,
                    DialogueText = node.DialogueText,
                    PositionX = node.GetPosition().position.x,
                    PositionY = node.GetPosition().position.y
                });
            }

            AssignVariables();

            List<string> npc_Dialogues = new List<string>();
            List<string> player_Choices = new List<string>();

            List<int> choice_Leades_To = new List<int>();
            List<IntList> dialogue_Leades_ToChoices = new List<IntList>();

            List<int> choice_Conditions = new List<int>();
            List<int> choice_Event = new List<int>();

            CreateDialogueSave(out npc_Dialogues, out player_Choices, out choice_Leades_To, out dialogue_Leades_ToChoices, out choice_Conditions, out choice_Event);

            data.save = new DialogueSave
            {
                NPC_Dialogues = npc_Dialogues,
                Player_Choices = player_Choices,
                Choice_Leades_To = choice_Leades_To,
                Dialogue_Leades_ToChoices = dialogue_Leades_ToChoices,
                Choice_Conditions = choice_Conditions,
                Choice_Event = choice_Event
            };

            binaryFormatter.Serialize(file, data);
            file.Close();

            AssetDatabase.Refresh();
        }
    }

    public void LoadDialogue(string FileName)
    {
        if (File.Exists(Application.dataPath + "/Resources/Dialogues/" + FileName + ".bytes"))
        {

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/Resources/Dialogues/" + FileName + ".bytes", FileMode.Open);
            DialogueData data = (DialogueData)binaryFormatter.Deserialize(file);

            Dialogue dial = GameObject.Find("DialogueCanvas").GetComponent<Dialogue>();

            edges = graphView.edges.ToList();
            nodes = graphView.nodes.Cast<DialogueNode>().ToList();

            ClearGraph(data);
            GenerateDialogueNodes(data);
            ConnectDialogueNodes(data);

            dial.NPC_Dialogues = data.save.NPC_Dialogues;
            dial.Player_Choices = data.save.Player_Choices;
            dial.Choice_Leades_To = data.save.Choice_Leades_To;
            dial.Dialogue_Leades_ToChoices = data.save.Dialogue_Leades_ToChoices;
            dial.Choice_Conditions = data.save.Choice_Conditions;
            dial.Choice_Event = data.save.Choice_Event;

            file.Close();

        } else
        {
            EditorUtility.DisplayDialog("File Not Found", "You have tried to load a dialogue that does not exist.", "Ok");
        }
    }

    private void AssignVariables()
    {
        nodes = graphView.nodes.ToList().Cast<DialogueNode>().ToList();

        int j = 0;
        int k = 0;

        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].ChoiceIDs.Clear();
            nodes[i].ChoiceLeadsTo.Clear();
            //nodes[i].Dialogue_Leades_ToChoices.Clear();

            if (!nodes[i].EntryNode)
            {
                if (k == 0) k++;
                nodes[i].DialogueID = k;
                k++;
            }
            else
            {
                nodes[i].DialogueID = 0;
            }
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].outputContainer.childCount > 0)
            {
                for (int a = 0; a < nodes[i].outputContainer.childCount; a++)
                {
                    nodes[i].ChoiceIDs.Add(j);
                    j++;

                    edges = graphView.edges.ToList();
                    var connectedEdges = edges.ToArray();

                    for (int b = 0; b < connectedEdges.Count(); b++)
                    {
                        var outputNode = (connectedEdges[b].output.node as DialogueNode);

                        if (outputNode == nodes[i])
                        {
                            var inputNode = (connectedEdges[b].input.node as DialogueNode);
                            outputNode.ChoiceLeadsTo.Add(inputNode.DialogueID);
                        }
                    }
                }
            }
        }
    }

    private void CreateDialogueSave(out List<string> npc_Dialogues, out List<string> Player_Choices, out List<int> choice_Leades_To, out List<IntList> Dialogue_Leades_ToChoices, out List<int> Choice_Conditions, out List<int> Choice_Event)
    {
        npc_Dialogues = new List<string>();
        Player_Choices = new List<string>();

        choice_Leades_To = new List<int>();
        Dialogue_Leades_ToChoices = new List<IntList>();

        Choice_Conditions = new List<int>();
        Choice_Event = new List<int>();

        edges = graphView.edges.ToList();

        for (int i = 0; i < nodes.Count; i++)
        {
            Dialogue_Leades_ToChoices.Add(new IntList());
            npc_Dialogues.Add("");

            for (int a = 0; a < nodes[i].outputContainer.childCount; a++)
            {
                Player_Choices.Add("");
                choice_Leades_To.Add(-1);
                Choice_Conditions.Add(0);
                Choice_Event.Add(0);
            }
        }

        int d = 0;
        for (int i = 0; i < nodes.Count; i++)
        {
            Dialogue_Leades_ToChoices[i] = new IntList(nodes[i].ChoiceIDs);

            Debug.Log(nodes[i].DialogueID);
            npc_Dialogues[nodes[i].DialogueID] = (nodes[i].DialogueText);
            for (int a = 0; a < nodes[i].outputContainer.childCount; a++)
            {
                foreach (var edge in edges)
                {
                    if (edge.output == nodes[i].outputContainer[a])
                    {
                        Player_Choices[d] = edge.output.portName;
                        choice_Leades_To[d] = nodes[i].ChoiceLeadsTo[a];
                        d++;
                    }
                }
            }
        }
    }

    private void ClearGraph(DialogueData data)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            edges.Where(x => x.input.node == nodes[i]).ToList().ForEach(edge => graphView.RemoveElement(edge));
            graphView.RemoveElement(nodes[i]);
        }
    }

    private void GenerateDialogueNodes(DialogueData data)
    {
        nodes = graphView.nodes.Cast<DialogueNode>().ToList();
        foreach (NodeData nodeToAdd in data.NodeData)
        {
            DialogueNode tempNode = new DialogueNode();
            if (!nodeToAdd.EntryNode) tempNode = graphView.CreateNewNode(nodeToAdd.DialogueText, nodeToAdd.GUID, new Vector2(nodeToAdd.PositionX, nodeToAdd.PositionY));
            else tempNode = graphView.CreateEntryNode(nodeToAdd.DialogueText, nodeToAdd.GUID, new Vector2(nodeToAdd.PositionX, nodeToAdd.PositionY));

            graphView.AddElement(tempNode);

            List<NodeEdgeData> nodePorts = data.NodeEdges.Where(x => x.CurrentNodeGUID == nodeToAdd.GUID).ToList();
            nodePorts.ForEach(x => graphView.AddChoice(tempNode, x.PortName));
        }
    }

    private void ConnectDialogueNodes(DialogueData data)
    {
        nodes = graphView.nodes.Cast<DialogueNode>().ToList();
        for (int i = 0; i < nodes.Count; i++)
        {
            List<NodeEdgeData> connections = data.NodeEdges.Where(x => x.CurrentNodeGUID == nodes[i].GUID).ToList();

            for (int j = 0; j < connections.Count; j++)
            {
                string nextNodeGUID = connections[j].NextNodeGUID;
                DialogueNode nextNode = new DialogueNode();

                foreach ( DialogueNode node in nodes)
                {
                    if (node.GUID == nextNodeGUID)
                    {
                        nextNode = node;
                        break;
                    }
                }

                LinkNodes(nodes[i].outputContainer[j].Q<Port>(), (Port)nextNode.inputContainer[0]);
                nextNode.SetPosition(new Rect(new Vector2(data.NodeData.First(x => x.GUID == nextNodeGUID).PositionX, data.NodeData.First(x => x.GUID == nextNodeGUID).PositionY), new Vector2(200, 300)));
            }
        }
    }

    private void LinkNodes(Port Output, Port Input)
    {
        Edge tempEdge = new Edge(){
            output = Output,
            input = Input
        };

        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);
        graphView.Add(tempEdge);
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

[System.Serializable]
public class NodeEdgeData
{
    public string CurrentNodeGUID;
    public string NextNodeGUID;
    public string PortName;
}

[System.Serializable]
public class NodeData
{
    public bool EntryNode;
    public string GUID;
    public string DialogueText;
    public float PositionX;
    public float PositionY;
}

[System.Serializable]
public class DialogueData
{
    public List<NodeEdgeData> NodeEdges = new List<NodeEdgeData>();
    public List<NodeData> NodeData = new List<NodeData>();

    public DialogueSave save = new DialogueSave();
}