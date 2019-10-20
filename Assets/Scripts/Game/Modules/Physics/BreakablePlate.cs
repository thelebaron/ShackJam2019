using System.Collections.Generic;
using ShackJam;

namespace Game.Modules.Physics
{
    using Unity.Entities;
    using UnityEngine;

    public class BreakablePlate : MonoBehaviour, IConvertGameObjectToEntity,IDisableable
    {
        private Rigidbody m_Rigidbody;
        [SerializeField] private List<GameObject> breakPrefabs;
        [SerializeField] private float breakMagnitude = 2;
        
        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }
    
        private void OnCollisionEnter(Collision other)
        {
            if(Disabled)
                return;
            
            if (m_Rigidbody.velocity.magnitude > breakMagnitude)
            {
                Spawn();
                Disabled = true;
                enabled  = false;
                Destroy(gameObject);
                
            }
        }

        private void Spawn()
        {
            var pos = transform.position;
            var rot = transform.rotation;
            foreach (var prefab in breakPrefabs)
            {
                var g = Instantiate(prefab);
                g.transform.position = pos;
                g.transform.rotation = rot;
            }
        }

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            //dstManager.AddComponent<PhysicsSleep>(entity);
        }

        public bool Disabled { get; set; }
    }
}