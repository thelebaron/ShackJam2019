using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public GameObject MenuHolder;

    public GameObject GameCamera;


    private void Awake()
    {
        GameCamera.SetActive(false);
    }

    void Update()
    {
        if (Input.anyKey)
        {
            GameCamera.SetActive(true);
            MenuHolder.SetActive(false);

        }
    }
}
