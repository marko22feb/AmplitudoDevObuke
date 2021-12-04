using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
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
        AddElement(CreateNewNode(Name, Guid.NewGuid().ToString(), PositionOnScreen));
    }

    public DialogueNode CreateNewNode(string dialogueText, string newGUID, Vector2 PositionOnScreen)
    {
        DialogueNode node = new DialogueNode()
        {
            title = dialogueText,
            GUID = newGUID,
            DialogueText = dialogueText,
            EntryNode = false
        };

        Port generatedPort = GetPortInstance(node, UnityEditor.Experimental.GraphView.Direction.Input, Port.Capacity.Multi);
        generatedPort.portName = "Input";
        node.inputContainer.Add(generatedPort);

        node.SetPosition(new Rect(PositionOnScreen.x, PositionOnScreen.y, 100, 200));

        node.RefreshExpandedState();
        node.RefreshPorts();

        GenerateOutPorts(node, dialogueText);

        return node;
    }

    public DialogueNode CreateEntryNode()
    {
        DialogueNode node = new DialogueNode()
        {
            title = "Entry",
            GUID = Guid.NewGuid().ToString(),
            DialogueText = "First NPC Dialogue",
            EntryNode = true
        };

         GenerateOutPorts(node, "First NPC Dialogue");

        node.SetPosition(new Rect(100, 200, 100, 200));

        return node;
    }

    public DialogueNode CreateEntryNode(string dialogueText, string newGUID, Vector2 Position)
    {
        DialogueNode node = new DialogueNode()
        {
            title = dialogueText,
            GUID = newGUID,
            DialogueText = dialogueText,
            EntryNode = true
        };

        GenerateOutPorts(node, dialogueText);

        node.SetPosition(new Rect(Position.x, Position.y, 100, 200));

        return node;
    }


    private void GenerateOutPorts(DialogueNode node, string DialogueText)
    {
        TextField NPCDialogueTextField = new TextField("Dialogue Text : ");
        NPCDialogueTextField.SetValueWithoutNotify(DialogueText);

        NPCDialogueTextField.RegisterValueChangedCallback(evt =>
        {
            node.DialogueText = evt.newValue;
        }
        );

        node.titleButtonContainer.Add(NPCDialogueTextField);

        string ChoiceText = "";

        Button AddNewChoiceButton = new Button(clickEvent: () => { AddChoice(node, ChoiceText); });
        AddNewChoiceButton.text = "New Choice";

        node.titleButtonContainer.Add(AddNewChoiceButton);

        TextField choiceTextField = new TextField("");
        choiceTextField.RegisterValueChangedCallback(evt =>
        {
            ChoiceText = evt.newValue;
        });

        node.titleButtonContainer.Add(choiceTextField);

        node.RefreshExpandedState();
        node.RefreshPorts();
    }

    public void AddChoice(DialogueNode node, string ChoiceText)
    {
        Port generatedPort = GetPortInstance(node, UnityEditor.Experimental.GraphView.Direction.Output, Port.Capacity.Single);

        Label portLabel = generatedPort.contentContainer.Q<Label>("type");
        generatedPort.contentContainer.Remove(portLabel);

        int outportCount = node.outputContainer.Query("connector").ToList().Count();
        string outportName = string.IsNullOrEmpty(ChoiceText) ? "Choice " + outportCount : ChoiceText;



        TextField textField = new TextField()
        {
            name = string.Empty,
            value = outportName
        };

        textField.RegisterValueChangedCallback(evt =>
        {
            generatedPort.portName = evt.newValue;
        });

        generatedPort.contentContainer.Add(new Label(" "));
        generatedPort.contentContainer.Add(textField);

        IntegerField conditionField = new IntegerField()
        {
            name = string.Empty,
            value = -1
        };

        IntegerField eventsField = new IntegerField()
        {
            name = string.Empty,
            value = -1
        };

        eventsField.RegisterValueChangedCallback(evt =>
        {
            eventsField.value = evt.newValue;
        }
        );

        conditionField.RegisterValueChangedCallback(evt =>
        {
            conditionField.value = evt.newValue;
        }
        );

        generatedPort.contentContainer.Add(eventsField);
        generatedPort.contentContainer.Add(new Label("Event Index"));


        generatedPort.contentContainer.Add(conditionField);
        generatedPort.contentContainer.Add(new Label("Condition Index"));

        Button DeleteChoiceButton = new Button(clickEvent: () =>{ RemoveChoice(node, generatedPort); }) ;
        DeleteChoiceButton.text = "X";

        generatedPort.contentContainer.Add(DeleteChoiceButton);

        generatedPort.portName = outportName;
        node.outputContainer.Add(generatedPort);

        node.RefreshExpandedState();
        node.RefreshPorts();
    }

    private void RemoveChoice(DialogueNode node, Port portToDelete)
    {
        var targetEdge = edges.ToList().Where(x => x.output.portName == portToDelete.portName && x.output.node == portToDelete.node);

        if (targetEdge.Any())
        {
            Edge edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());
        }

        node.outputContainer.Remove(portToDelete);
        node.RefreshExpandedState();
        node.RefreshPorts();
    }

    private Port GetPortInstance(DialogueNode node, UnityEditor.Experimental.GraphView.Direction nodeDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        Port startPortView = startPort;

        ports.ForEach((port) =>
        {
            Port portView = port;
            if (startPortView != portView && startPortView.node != portView.node) compatiblePorts.Add(port);
        });

        return compatiblePorts;
    }
}
