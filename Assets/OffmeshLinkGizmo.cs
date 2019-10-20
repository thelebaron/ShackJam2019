
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(OffMeshLink))]
public class OffmeshLinkGizmo : MonoBehaviour
{
    private OffMeshLink m_Link;
    
    private void OnDrawGizmos()
    {
        if (m_Link == null)
            m_Link = GetComponent<OffMeshLink>();

        if (m_Link.startTransform != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(m_Link.startTransform.position, Vector3.one * 0.2f);
        }
        
        if (m_Link.endTransform != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(m_Link.endTransform.position, Vector3.one * 0.2f);
        }

        if (m_Link.endTransform != null && m_Link.startTransform != null)
        {
            Debug.DrawLine(m_Link.endTransform.position, m_Link.startTransform.position, Color.red);
        }
    }
}
