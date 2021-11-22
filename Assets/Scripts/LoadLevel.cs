using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : InteractableObject
{
    [SerializeField]
    string LevelName;
    public Scene SceneToLoad;

    public bool Locked;

    public LockedDoor DoorType;
    public int LockID;

    public override void OnTriggerCheck(Collider2D collision)
    {
     if (collision.tag == "Player")
        {
            switch (DoorType)
            {
                case LockedDoor.enemyDead:
                    bool isDead = false;
                    foreach (int item in GameController.control.BossesDead)
                    {
                        if (item == LockID)
                        {
                            isDead = true;
                            break;
                        }
                    }
                    if (isDead) Locked = false;
                    break;
                case LockedDoor.hasItem:
                   if( Inventory.inv.FetchInventoryItem(LockID) > 0)
                    {
                        Locked = false;
                    }
                    break;
                case LockedDoor.needCoins:
                    if (GameController.control.CoinsCount >= LockID)
                    {
                        Locked = false;
                    }
                    break;
                case LockedDoor.custom:
                    break;
                default:
                    break;
            }
        } 
    }

    public override void OnInteract()
    {
        if (!Locked)
        {
            if (DoOnce) return;
            SceneManager.LoadScene(LevelName, LoadSceneMode.Single);
            base.OnInteract();
        } else
        {
            AudioClip swoosh;
            swoosh = Resources.Load("Audio_SFX/SFX_LockedDoors") as AudioClip;
            AudioSource.PlayClipAtPoint(swoosh, new Vector3(0, 0, 0));
        }
    }
}
