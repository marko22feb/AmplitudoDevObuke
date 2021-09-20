using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test33 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float z = transform.localRotation.z + 1;
        if (z >= 180) z = 0;
        transform.localRotation = new Quaternion(transform.localRotation.x, transform.localRotation.y, z, transform.localRotation.w);
    }
}
