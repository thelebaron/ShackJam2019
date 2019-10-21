using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonBehaviour : MonoBehaviour
{

    private Rigidbody m_Rigidbody;

    public float increasingForce;
    // Start is called before the first frame update

    private float timer;
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_LineRenderer = GetComponent<LineRenderer>();
        m_LineRenderer.SetPosition(0, transform.position);
        m_LineRenderer.SetPosition(1, transform.position -  (2 * Vector3.up));
        
    }

    void FixedUpdate()
    {
        if(m_Rigidbody.isKinematic)
            return;
        else
        {
            m_Rigidbody.useGravity = false;

            increasingForce += Time.fixedTime * 0.05f * Random.Range(0.1f, 1f);
            var randomup = increasingForce * new Vector3(UnityEngine.Random.Range(-5f, 5f), 1, UnityEngine.Random.Range(-5f, 5f));
            
            m_Rigidbody.AddForce(randomup);
        }
        
    }
    
    private LineRenderer m_LineRenderer;
    // Update is called once per frame
    void Update()
    {
        
        if(m_Rigidbody.isKinematic)
            return;
        
        m_LineRenderer.SetPosition(0, transform.position);
        m_LineRenderer.SetPosition(1, transform.position - (2 * Vector3.up) + new Vector3(Random.Range(-0.1f,0.1f),Random.Range(-0.1f,0.1f),Random.Range(-0.1f,0.1f)));

        timer += Time.deltaTime;
        if(timer>50)
            Destroy(gameObject);

    }
}
