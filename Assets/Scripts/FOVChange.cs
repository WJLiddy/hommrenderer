using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKeyDown(KeyCode.Equals) || Input.mouseScrollDelta.y > 0) && GetComponent<Camera>().fieldOfView >= 35)
        {
            GetComponent<Camera>().fieldOfView -= 10;
        }
        if ((Input.GetKeyDown(KeyCode.Minus) || Input.mouseScrollDelta.y < 0) && GetComponent<Camera>().fieldOfView <= 95)
        {
            GetComponent<Camera>().fieldOfView += 10;
        }
    }
}
