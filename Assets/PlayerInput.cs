using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public InteractableObject objectToInteractWith;

    private void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (objectToInteractWith != null)
            {
                objectToInteractWith.OnInteract();
            }
        }
    }
}
