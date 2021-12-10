using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableConsole : MonoBehaviour
{
    public GameObject console;
   public void onClick(bool enable)
    {
       console = GameObject.Find("Console_Canvas");
       if (console == null)
        {
            console = Instantiate(console);
        }

        console.GetComponent<Canvas>().enabled = enable;
    }
}
