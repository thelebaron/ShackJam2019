using System;
using System.Collections.Generic;
using UnityEngine;
using ShackJam;


[RequireComponent(typeof(BoxCollider))]
public class QueueDisruption : MonoBehaviour
{
    public StressBehaviour StressReceiver;

    [SerializeField] private bool            HasCompletedBehaviour;
    [SerializeField] private List<Rigidbody> m_ChildRigidbodies = new List<Rigidbody>();
    private float timer;
    private BoxCollider m_BoxCollider;
    [SerializeField] private GameObject QueueHolder;
    void Start()
    {
        m_BoxCollider = GetComponent<BoxCollider>();
        m_BoxCollider.isTrigger = true;
        var list = GetComponentsInChildren(typeof(Rigidbody));

        foreach (Rigidbody rb in list)
        {
            m_ChildRigidbodies.Add(rb);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(HasCompletedBehaviour)
            return;
        
        if(!other.gameObject.CompareTag(Tags.Player))
            return;
            
        foreach (var rb in m_ChildRigidbodies)
        {
            //rb.isKinematic = false;
            if (rb.velocity.magnitude > 0.1f)
            {
                Debug.Log("PlayerEntered");
                
                StressReceiver.Stress();
                HasCompletedBehaviour = true;
                enabled               = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        
        Debug.DrawLine(transform.position, StressReceiver.transform.position);
    }
}
