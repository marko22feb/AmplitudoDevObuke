using UnityEngine;

public class Lever : InteractableObject
{
    [SerializeField]
    AIBehavior bossBehavior;

    SpriteRenderer render;
    [HideInInspector]
    public bool IsActive = false;

    [SerializeField]
    Sprite Active;

    [SerializeField]
    Sprite InActive;

     void Awake()
    {
        render = GetComponent<SpriteRenderer>();
       // bossBehavior.lever = this;
        UpdateVisuals();
    }

    public void OnLeverPress()
    {
        if (bossBehavior.canBeStunned)
        {
            bossBehavior.OnNPCStateChange(NPCState.stunned);
            bossBehavior.canBeStunned = false;
            IsActive = false;
            UpdateVisuals();
        }
    }

    public override void OnInteract()
    {
        base.OnInteract();
        OnLeverPress();
    }

    public void UpdateVisuals()
    {
        render.sprite = IsActive ? Active : InActive;
    }
}
