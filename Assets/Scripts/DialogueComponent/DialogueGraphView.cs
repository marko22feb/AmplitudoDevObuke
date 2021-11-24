using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueGraphView : GraphView
{
    
    public DialogueGraphView()
    {
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new ContentZoomer());

        AddElement(CreateEntryNode());
    }

   public void CreateNewDialogueNode(string Name, Vector2 PositionOnScreen)
    {
        AddElement(CreateNewNode(Name, PositionOnScreen));
    }

    private DialogueNode CreateNewNode(string Name, Vector2 PositionOnScreen)
    {
        return new DialogueNode();
    }

    private DialogueNode CreateEntryNode()
    {
        DialogueNode node = new DialogueNode()
        {
            title = "Entry",
            GUID = Guid.NewGuid().ToString(),
            DialogueText = "First NPC Dialogue",
            EntryNode = true
        };

        Port generatedPort = GetPortInstance(node, UnityEditor.Experimental.GraphView.Direction.Output);
        generatedPort.portName = "Next Dialogue";

        node.outputContainer.Add(generatedPort);

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(100, 200, 100, 200));

        return node;
    }

    private Port GetPortInstance(DialogueNode node, UnityEditor.Experimental.GraphView.Direction nodeDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
    }
}
