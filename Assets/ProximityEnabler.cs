using System;
using System.Collections;
using System.Collections.Generic;
using ShackJam;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class ProximityEnabler : MonoBehaviour
{

    public StressBehaviour StressReceiver;

    [SerializeField] private bool HasCompletedBehaviour;
    [SerializeField] private List<Rigidbody> m_ChildRigidbodies = new List<Rigidbody>();
    [SerializeField]private BoxCollider m_Collider;
    
    void Start()
    {

        m_Collider = GetComponent<BoxCollider>();
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
        
        if (other.gameObject.CompareTag(Tags.Player) && !HasCompletedBehaviour)
        {
            foreach (var rb in m_ChildRigidbodies)
            {
                rb.isKinematic = false;
            }

            StressReceiver.Stress();
            
            HasCompletedBehaviour = true;

        }
    }

    private void OnDrawGizmos()
    {
        if(m_Collider==null)
            m_Collider = GetComponent<BoxCollider>();
        
        Gizmos.color = new Color(1f, 0f, 0.04f, 0.42f);
        Gizmos.DrawCube(m_Collider.center + transform.position, m_Collider.size);
    }
}
