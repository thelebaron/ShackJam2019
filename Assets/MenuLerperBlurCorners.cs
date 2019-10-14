using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class MenuLerperBlurCorners : MonoBehaviour
{
    public VignetteAndChromaticAberration VignetteAndChromaticAberration;
    public float blurIncrement = 0.02f;

    public bool increaseBlur;
    public float timer;
    public float timerMax = 1;
    void Update()
    {
        
        //blurValue = System.Math.Round(VignetteAndChromaticAberration.blur, 2);
        if (increaseBlur)
        {
            VignetteAndChromaticAberration.blur += blurIncrement;
            if (VignetteAndChromaticAberration.blur >=1 )
                timer = timerMax;
            timer += Time.deltaTime;
        }

        if (!increaseBlur)
        {
            VignetteAndChromaticAberration.blur -= blurIncrement;
            if (VignetteAndChromaticAberration.blur <= 0)
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
