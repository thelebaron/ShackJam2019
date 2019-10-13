using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float3 recordedScale;

    private void Awake()
    {
        recordedScale = transform.localScale;
    }

    void Update()
    {
        if (!transform.localScale.Equals(recordedScale))
        {
            transform.localScale = math.lerp(transform.localScale, recordedScale, Time.deltaTime * 15f);
        }
    }
}
