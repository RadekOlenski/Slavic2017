using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour
{
    public Texture2D FadeOutTexture2D;
    public float baseFadeSpeed = 0.8f;

    private float fadeSpeed;
    private int drawDepth = -1000;  //draw in render hierarchy; minus means on top
    private float alpha = 1.0f;
    private int fadeDir = -1;

    void Awake()
    {
        fadeSpeed = baseFadeSpeed;
    }

    void OnGUI()
    {
        alpha += fadeDir*fadeSpeed*Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), FadeOutTexture2D);
    }

    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return fadeSpeed;
    }

    public float BeginFade(int direction, float speed)
    {
        fadeSpeed = speed;
        fadeDir = direction;
        return fadeSpeed;
    }

    void OnLevelWasLoaded()
    {
        fadeSpeed = baseFadeSpeed;
        alpha = 1;
        BeginFade(-1);
    }

    public bool IsAlphaWholeNumber()
    {
        return alpha <= 0.5 || alpha >= 1;
    }
}
