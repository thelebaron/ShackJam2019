using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthController : MonoBehaviour
{
    [SerializeField] private Transform mouth;
    [SerializeField] private Vector3 mouthBig;
    //[SerializeField] private Vector3 mouthSemibig;
    private bool run;
    [SerializeField] private MouthBehaviour MouthBehaviour;
    
    public void OpenSesame()
    {
        if (MouthBehaviour == MouthBehaviour.Open)
        {
            mouth.localScale = mouthBig;
            
        }
        else
        {
            run = true;
        }
        

    }

    public void Update()
    {
        if (run)
        {
            //mouth.localScale = Vector3.Lerp(mouthBig, );
        }
    }
}


public enum MouthBehaviour
{
    Open,
    Lerp
}