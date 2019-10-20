using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ColorIndex
{
    A,
    B,
    C,
    D
}
public class MenuTextMeshColorLerper : MonoBehaviour
{
    public ColorIndex colorIndex;

    public TextMeshPro text;
    public float timer;
    public float timerMax = 1;
    public Color color;
    private Material material;

    public Camera camera;
    private void Start()
    {
        material = text.renderer.material;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timerMax)
        {
            timer = 0;
            color = UnityEngine.Random.ColorHSV(0.0f, 1f, 1f, 1f, 0.75f, 1f);
        }
        camera.backgroundColor = Color.Lerp(camera.backgroundColor, InvertColor(color), Time.deltaTime);
        text.color = Color.Lerp(text.color, color, Time.deltaTime);
    }

    private Color InvertColor ( Color color)
    {
        var inverse = color;
        inverse.r = 1.0f - color.r;
        inverse.g = 1.0f - color.g;
        inverse.b = 1.0f - color.b;
        //inverse.a = 1.0f - color.b;
        return inverse;
    }
}
