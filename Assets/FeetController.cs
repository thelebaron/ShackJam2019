using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
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

    public float3 m_WorldPositionLeftFoot;
    public float3 m_LeftFootPositionLerped;
    public float3 RightFootPositionLerped;

    public NavMeshAgent NavMeshAgent;
    public bool working;
    public float dist;

    private float timer = 0.3f;
    private const float maxTime = 0.15f;
    private bool switchFoot;

    public float m_Velocity;
    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        
        if(!Application.isPlaying)
            return;
        //unparent
        LeftFoot.transform.SetParent(null);
        RightFoot.transform.SetParent(null);
        
    }

    // Returns position + y offset
    private float3 Position()
    {
        float3 oldpos = transform.position;
        var newpos = new float3(0,PositionOffset,0);
        return  oldpos + newpos;
    }


    
    void Update()
    {
        LeftLegLineRenderer.SetPosition(0, LeftHipSocket.transform.position);
        LeftLegLineRenderer.SetPosition(1, LeftFoot.transform.position);
        RightLegLineRenderer.SetPosition(0, RightHipSocket.transform.position);
        RightLegLineRenderer.SetPosition(1, RightFoot.transform.position);
        
        if(!Application.isPlaying)
            return;

        timer += Time.deltaTime;

        m_Velocity = NavMeshAgent.velocity.magnitude;

        var dista = math.distance((float3)TargetLeft.position/* + Position()*/, LeftFoot.transform.position);
        if (dista > 0.45f || NavMeshAgent.velocity.magnitude <= 1f)
        {
            if (timer >maxTime && switchFoot)
                LeftFoot.transform.position = (float3) TargetLeft.position;// + Position();
        }
        var distb = math.distance((float3)TargetRight.position/* + Position()*/, RightFoot.transform.position);
        if (distb > 0.45f || NavMeshAgent.velocity.magnitude <= 1f)
        {
            if (timer > maxTime && !switchFoot)
                RightFoot.transform.position = (float3) TargetRight.position;// + Position();
        }

        if (timer > maxTime)
        {
            timer = 0;
            switchFoot = !switchFoot;
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(LeftFootPositionA + Position(), 0.1f);
        Gizmos.DrawSphere(LeftFootPositionB + Position(), 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(RightFootPositionA + Position(), 0.1f);
        Gizmos.DrawSphere(RightFootPositionB + Position(), 0.1f);
        
    }
    
    
}
