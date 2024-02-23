using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    private void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

    }

    float deltaTime = 0.0f;
    [Tooltip("Shows FPS in top left corner.")]
    public bool showFps = true;
    /*
	* Shows fps if its set to true.
	*/
    void OnGUI()
    {

        if (showFps)
        {
            FPSCounter();
        }

    }
    /*
	* Calculating real fps because unity status tab shows too much fps even when its not that mutch so i made my own.
	*/
    void FPSCounter()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = Color.white;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}
