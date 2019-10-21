using System;
using System.Collections;
using System.Collections.Generic;
using Game.Modules.Hybrid;
using ShackJam;
using Unity.Mathematics;
using UnityEngine;

public enum DoorAxis
{
    x = 0,
    y = 1,
    z = 2
}
[SelectionBase]
public class Door : MonoBehaviour
{
    public bool isOpen;

    public bool isOpening;
    public DoorAxis axis;
    public float3 openAngle;
    public float3 closedAngle;
    
    public StressBehaviour StressReceiver;
    
    private void Start()
    {
        //angleOpen = transform.rotation.eulerAngles.y + 90f;
    }

    private void Update()
    {
        
        
        if (isOpening)
        {
            transform.Rotate(Vector3.up * 15);
        }

        var yangle = transform.rotation.eulerAngles.y;
        if (yangle >= openAngle.y)
        {
            isOpening = false;
            
        }

        switch (axis)
        {
            case DoorAxis.x:
            {
                
            }
                break;
            case DoorAxis.y:
            {
                
            }
                break;
            case DoorAxis.z:
            {
                
            }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<EntityBehaviour>())
        {
            var eb = other.transform.GetComponent<EntityBehaviour>();
            if (eb.EntityManager.HasComponent<PlayerControlTag>(eb.Entity))
            {
                //Debug.Log("HasComponent");
                if (isOpen)
                {
                    //Debug.Log("isOpen");
                    isOpen = false;
                    isOpening = true;
                    
                    if(StressReceiver!=null)
                        StressReceiver.Stress();
                }
            }
            
        }
        
    }

    public void SetOpenState()
    {
        openAngle = transform.rotation.eulerAngles;
    }

    public void SetClosedState()
    {
        closedAngle = transform.rotation.eulerAngles;
    }
}
