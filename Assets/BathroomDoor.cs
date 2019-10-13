using System;
using System.Collections;
using System.Collections.Generic;
using Game.Modules.Hybrid;
using ShackJam;
using UnityEngine;

[SelectionBase]
public class BathroomDoor : MonoBehaviour
{
    public bool isOpen;

    public bool isOpening;
    private float angleOpen;
    
    private void Start()
    {
        angleOpen = transform.rotation.eulerAngles.y + 90f;
    }

    void Update()
    {
        if (isOpening)
        {
            transform.Rotate(Vector3.up * 15);
        }

        var yangle = transform.rotation.eulerAngles.y;
        if (yangle >= 90f)
        {
            isOpening = false;
            
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<EntityBehaviour>())
        {
            var eb = other.transform.GetComponent<EntityBehaviour>();
            if (eb.EntityManager.HasComponent<DirectControlTag>(eb.Entity))
            {
                //Debug.Log("HasComponent");
                if (isOpen)
                {
                    //Debug.Log("isOpen");
                    isOpen = false;
                    isOpening = true;

                }
            }
            
        }
        
    }
}
