using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using Utility;

public class MenuLerperPixelizer : MonoBehaviour
{
    public PixelBoy PixelBoy;
    public int blurIncrement = 1;
    public float2 chromaticMinMax = new float2(64,128);
    public bool increaseBlur;
    public float timer;
    public float timerMax = 1;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timerMax)
        {
            //blurValue = System.Math.Round(VignetteAndChromaticAberration.blur, 2);
            if (increaseBlur)
            {
                PixelBoy.h += blurIncrement;
                if (PixelBoy.h >= chromaticMinMax.y)
                    increaseBlur = !increaseBlur;
                //timer += Time.deltaTime;
            }

            if (!increaseBlur)
            {
                PixelBoy.h -= blurIncrement;
                if (PixelBoy.h <= chromaticMinMax.x)
                    increaseBlur = !increaseBlur;
            
                //timer += Time.deltaTime;
            }
            timer = 0;
        }
        

        
    }
}