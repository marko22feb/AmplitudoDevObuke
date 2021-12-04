using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public string DialogueName;

    public List<string> NPC_Dialogues;
    public List<string> Player_Choices;

    public List<int> Choice_Leades_To;
    public List<IntList> Dialogue_Leades_ToChoices;

    public List<int> Choice_Conditions;
    public List<int> Choice_Event;

    private Text DialogueText;
    private Transform ChoicesPanel;
    public GameObject ChoicePrefab;

    private void Awake()
    {
        DialogueText = transform.Find("DialogueTextPanel").Find("DialogueText").GetComponent<Text>();
        ChoicesPanel = transform.Find("DialogueChoicesPanel").transform;
    }

    private void Start()
    {
        if (File.Exists(Application.dataPath + "/Resources/Dialogues/" + DialogueName + ".bytes"))
        {

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.dataPath + "/Resources/Dialogues/" + DialogueName + ".bytes", FileMode.Open);
            DialogueData data = (DialogueData)binaryFormatter.Deserialize(file);


            NPC_Dialogues = data.save.NPC_Dialogues;
            Player_Choices = data.save.Player_Choices;
            Choice_Leades_To = data.save.Choice_Leades_To;
            Dialogue_Leades_ToChoices = data.save.Dialogue_Leades_ToChoices;
            Choice_Conditions = data.save.Choice_Conditions;
            Choice_Event = data.save.Choice_Event;

            file.Close();

        }
        OnDialogueStart();
    }

    public void OnChoiceClick(int ChoiceID)
    {

        if (Choice_Leades_To.Count > ChoiceID)
        {
            NextDialogue(Choice_Leades_To[ChoiceID]);
        }

        ChoiceEvent(ChoiceID);
    }

    public void OnDialogueStart()
    {
        DialogueText.text = NPC_Dialogues[0];
        ConstructChoices(0);
    }

    public void NextDialogue(int DialogueID)
    {
        if (Dialogue_Leades_ToChoices.Count > DialogueID)
        {
            DialogueText.text = NPC_Dialogues[DialogueID];
            ConstructChoices(DialogueID);
        }
    }

    public void ConstructChoices(int DialogueID)
    {
        int Count = ChoicesPanel.childCount;

        for (int i = 0; i < Count; i++)
        {
            Destroy(ChoicesPanel.GetChild(i).gameObject);
        }

        for (int i = 0; i < Dialogue_Leades_ToChoices[DialogueID].LeadTo.Count; i++)
        {
            GameObject temp = Instantiate(ChoicePrefab, ChoicesPanel);
            int LeadTo = Dialogue_Leades_ToChoices[DialogueID].LeadTo[i];
            temp.GetComponent<DialogueChoice>().InstantiateText(LeadTo, Player_Choices[LeadTo], this, ConditionMet(LeadTo));
        }
    }

    public bool ConditionMet(int ChoiceID)
    {
        bool DidMeet = true;

        switch (Choice_Conditions[ChoiceID])
        {
            case 0:
                DidMeet = true;
                break;
            case 1:
                if (GameController.control.CoinsCount > 50) DidMeet = true; else DidMeet = false;
                break;
            default:
                break;
        }
        return DidMeet;
    }

    public void ChoiceEvent(int ChoiceID)
    {
        switch (Choice_Event[ChoiceID])
        {
            case 0:
                break;
            case -1:
                GetComponent<Canvas>().enabled = false;
                break;
            case 1:
                GetComponent<Canvas>().enabled = false;
                GameController.control.Player.GetComponent<PlayerInput>().ClickedInventory();
                break;
            default:
                break;
        }
    }
}

[System.Serializable]
public class IntList
{
    public List<int> LeadTo;

    public IntList()
    {
        LeadTo = new List<int>();
    }

    public IntList(List<int> newEntry)
    {
        LeadTo = newEntry;
    }
}
