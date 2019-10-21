using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MouthController : MonoBehaviour
{
    [SerializeField] private Transform mouth;
    [SerializeField] private Transform mouthSecondary;
    [SerializeField] private Vector3 mouthBig;
    //[SerializeField] private Vector3 mouthSemibig;
    private bool run;
    [SerializeField] private MouthBehaviour MouthBehaviour;
    
    [SerializeField] private GameObject VomitPrefab;

    public bool Vomiting;
    public bool VomitingFromSecondary;

    private const float vomitTimeoutTime = 3;
    private float vomitDuration;

    private int counter;
    public bool slowerinstantiation;
    public bool dontrescale;
    

    public void Start()
    {
        if (mouthSecondary != null)
        {
            VomitingFromSecondary = true;
        }
    }

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

    public void FixedUpdate()
    {
        if (Vomiting)
        {
            vomitDuration += Time.deltaTime;
            
            var randomMax = Random.Range(4, 7);
            if (vomitDuration >= randomMax)
            {
                vomitDuration = -vomitTimeoutTime;
            }
            
            if(vomitDuration<=0)
                return;
            
            Vomit();
        }
    }


    private void Vomit()
    {
        if (slowerinstantiation)
        {
            counter++;
        
            if(counter%2 != 0)
                return;
        }

        
        var vom = Instantiate(VomitPrefab);
        vom.transform.position = mouth.transform.position + mouth.transform.forward * 0.3f;
        vom.transform.rotation = Random.rotation;
        vom.transform.localScale = Random.Range(0.5f, 1) * vom.transform.localScale;

        var vomrb = vom.GetComponent<Rigidbody>();
        vomrb.AddForce(mouth.transform.forward * 0.3f);
        
        if (VomitingFromSecondary)
        {
            vom = Instantiate(VomitPrefab);
            vom.transform.position   = mouthSecondary.transform.position + mouthSecondary.transform.forward * 0.3f;
            vom.transform.rotation   = Random.rotation;
            vom.transform.localScale = Random.Range(0.5f, 1) * vom.transform.localScale;

            vomrb = vom.GetComponent<Rigidbody>();
            vomrb.AddForce(mouthSecondary.transform.forward * 0.3f);
        }

    }
}


public enum MouthBehaviour
{
    Open,
    None
}