using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCursor : MonoBehaviour
{
    public Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        var mp = Input.mousePosition;
        if(mp.x < Screen.width * 0.02f)
        {
            camera.transform.position -= Vector3.right * 10 * Time.deltaTime;
        }
        if(mp.x >  Screen.width * 0.98f)
        {
            camera.transform.position -= Vector3.left * 10 * Time.deltaTime;
        }

        if(mp.y < Screen.height * 0.02f)
        {
            camera.transform.position -= Vector3.forward * 10 * Time.deltaTime;
        }

        if(mp.y >  Screen.height * 0.98f)
        {
            camera.transform.position -= Vector3.back * 10 * Time.deltaTime;
        }

    }
}
