using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

//[ExecuteInEditMode]
public class FeetController : MonoBehaviour
{
    public float PositionOffset = 1;
    public LineRenderer LeftLegLineRenderer;
    public LineRenderer RightLegLineRenderer;

    public GameObject LeftHipSocket;
    public GameObject RightHipSocket;

    public Transform TargetLeft;
    public Transform TargetRight;

    public GameObject LeftFoot;
    public GameObject RightFoot;
    
    public float3 LeftFootPositionA;
    public float3 LeftFootPositionB;
    public float3 RightFootPositionA;
    public float3 RightFootPositionB;
    public float minVelocity = 1f;
    public float maxDistance = 0.45f;
    public float timer = 0.3f;
    public float maxTime = 0.15f;
    public bool switchFoot;
    
    //debugging
    public float currentVelocity;
    public float currentDistanceL;
    private NavMeshAgent _agent;
    private void Awake()
    {
        //if(!Application.isPlaying)
            //return;
        //unparent
        LeftFoot.transform.SetParent(null);
        RightFoot.transform.SetParent(null);
        _agent = GetComponent<NavMeshAgent>();
    }
    
    private void Update()
    {
        //if(Application.isPlaying)
            currentVelocity = _agent.velocity.magnitude;
        //if(Application.isPlaying)
            //return;
        LeftLegLineRenderer.SetPosition(0, LeftHipSocket.transform.position);
        LeftLegLineRenderer.SetPosition(1, LeftFoot.transform.position);
        RightLegLineRenderer.SetPosition(0, RightHipSocket.transform.position);
        RightLegLineRenderer.SetPosition(1, RightFoot.transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(LeftFootPositionA + Position(), 0.1f);
        Gizmos.DrawSphere(LeftFootPositionB + Position(), 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(RightFootPositionA + Position(), 0.1f);
        Gizmos.DrawSphere(RightFootPositionB + Position(), 0.1f);
    }
    // Returns position + y offset
    // For gizmos only
    [BurstCompile]
    private float3 Position()
    {
        float3 oldpos = transform.position;
        var newpos = new float3(0,PositionOffset,0);
        return  oldpos + newpos;
    }


    
}
