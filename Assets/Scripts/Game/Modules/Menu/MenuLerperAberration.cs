using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class MenuLerperAberration : MonoBehaviour
{
    public VignetteAndChromaticAberration VignetteAndChromaticAberration;
    public float blurIncrement = 0.02f;
    public float2 chromaticMinMax;
    public bool increaseBlur;
    public float timer;
    public float timerMax = 1;
    void Update()
    {
        
        //blurValue = System.Math.Round(VignetteAndChromaticAberration.blur, 2);
        if (increaseBlur)
        {
            VignetteAndChromaticAberration.chromaticAberration += blurIncrement;
            if (VignetteAndChromaticAberration.blur >=chromaticMinMax.y )
                timer = timerMax;
            timer += Time.deltaTime;
        }

        if (!increaseBlur)
        {
            VignetteAndChromaticAberration.chromaticAberration -= blurIncrement;
            if (VignetteAndChromaticAberration.blur <= chromaticMinMax.x)
                timer = timerMax;
            
            timer += Time.deltaTime;
        }

        if (timer >= 1)
        {
            timer = 0;
            increaseBlur = !increaseBlur;
        }
    }
}