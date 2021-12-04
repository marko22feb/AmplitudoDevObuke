using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DialogueNode : Node
{
    public string DialogueText;
    public string GUID;
    public bool EntryNode = false;

    public int DialogueID;

    public List<int> ChoiceIDs = new List<int>();
    public List<int> ChoiceLeadsTo = new List<int>();

    public List<int> Choice_Conditions = new List<int>();
    public List<int> Choice_Event = new List<int>();
}
