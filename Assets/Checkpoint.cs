using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Checkpoint : MonoBehaviour
{
    public bool refresh;
    public int CheckpointID;

    private void OnValidate()
    {
        CheckpointID = Mathf.RoundToInt(transform.position.x);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (GameController.control.LastCheckpoint != CheckpointID)
            {
                GameController.control.LastCheckpoint = CheckpointID;
                GameController.control.SaveGame();
            }
        }
    }
}
