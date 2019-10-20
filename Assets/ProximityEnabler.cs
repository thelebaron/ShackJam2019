using System;
using System.Collections;
using System.Collections.Generic;
using ShackJam;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class ProximityEnabler : MonoBehaviour
{

    public GameObject Player;

    public bool HasCompletedBehaviour;
    public List<Rigidbody> m_ChildRigidbodies = new List<Rigidbody>();
    public Collider m_Collider;
    
    void Start()
    {

        m_Collider = GetComponent<Collider>();
        m_Collider.isTrigger = true;
        
        var list = GetComponentsInChildren(typeof(Rigidbody));

        foreach (Rigidbody rb in list)
        {
            rb.isKinematic = true;
            m_ChildRigidbodies.Add(rb);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HELLO");
        if (other.gameObject.CompareTag(Tags.Player) && !HasCompletedBehaviour)
        {
            foreach (var rb in m_ChildRigidbodies)
            {
                rb.isKinematic = false;
            }

            HasCompletedBehaviour = true;

        }
    }
    
}
