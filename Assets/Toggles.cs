using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityStandardAssets.ImageEffects;
using Utility;

using UnityEngine.SceneManagement;
public class Toggles : MonoBehaviour
{
    public PixelBoy PixelBoy;
    public PostProcessLayer layer;

    public float timer;
    // Update is called once per frame
    void Update()
    {
        
        
        timer -= Time.deltaTime;
        
        if (timer > 0.2f) 
            return;
        
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            timer = 0.3f;
            PixelBoy.enabled = !PixelBoy.enabled;
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            timer = 0.3f;
            layer.enabled = !layer.enabled;
        }
        
        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        
        //if(Input.GetKey(KeyCode.R))
            //SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}

