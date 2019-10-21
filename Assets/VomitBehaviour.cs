using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class VomitBehaviour : MonoBehaviour
{
    private float timer;
    // Start is called before the first frame update
    public Rigidbody rb;

    public Collider Collider;
    // Update is called once per frame
    private bool cleanup;
    
    void Update()
    {
        timer += Time.deltaTime;

        transform.localScale = transform.localScale * new float3(0.99f,0.99f,0.99f);


        if (timer > 7f && !cleanup)
        {
            cleanup = true;
            Destroy(rb);
            Destroy(Collider);
        }
        
        if(timer>15f)
            Destroy(gameObject);
    }
}
