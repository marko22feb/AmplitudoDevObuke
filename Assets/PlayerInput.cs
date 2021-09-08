using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private InteractableObject objectToInteractWith;
    public GameObject InteractCanvasPrefab;
    private GameObject spawnedCanvas;

    public void ActivateInteract(InteractableObject ob)
    {
        if (objectToInteractWith == ob) return;
        objectToInteractWith = ob;
        if (ob != null)
            spawnedCanvas = Instantiate(InteractCanvasPrefab, objectToInteractWith.transform);
        else Destroy(spawnedCanvas);
    }

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
