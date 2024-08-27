using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            print("youtube");
            Application.OpenURL("https://www.youtube.com/");
        }
    }
}
