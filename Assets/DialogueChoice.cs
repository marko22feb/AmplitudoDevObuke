
using UnityEngine;
using UnityEngine.UI;

public class DialogueChoice : MonoBehaviour
{
    public int ChoiceID;
    private Text ChoiceText;
    private Dialogue dialogue;
    private bool conditionMet;

    private void Awake()
    {
        ChoiceText = transform.Find("ChoiceText").GetComponent<Text>(); 
    }

    public void InstantiateText(int Index, string choice_Text, Dialogue dial, bool Condition)
    {
        ChoiceID = Index;
        ChoiceText.text = choice_Text;
        dialogue = dial;
        conditionMet = Condition;

        if (conditionMet)
        {
            ChoiceText.color = Color.white;
        }
        else
        {
            ChoiceText.color = Color.grey;
        }
    }

    public void OnClick()
    {
        if (conditionMet)
        {
            dialogue.OnChoiceClick(ChoiceID);
        }
    }
}
