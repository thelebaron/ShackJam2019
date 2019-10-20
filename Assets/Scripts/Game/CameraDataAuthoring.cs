using System.Collections;
using System.Collections.Generic;
using ShackJam;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public struct CameraData : IComponentData
{
    public float ScrollSpeed;
    public float ScrollEdge;
    public float PanSpeed;
    public bool lockOn;
}
public class CameraDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public float ScrollSpeed = 15;
    public float ScrollEdge = 0.01f;
    public float PanSpeed = 10f;
    public float maxDistance = 10f;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentObject(entity, transform);
        dstManager.AddComponentData(entity, new CameraData
        {
            ScrollSpeed = ScrollSpeed,
            ScrollEdge = ScrollEdge,
            PanSpeed = PanSpeed
        });
        //dstManager.AddComponentData(entity, new Disabled());
    }
}


[UpdateInGroup(typeof(PresentationSystemGroup))]
public class CameraSystem : ComponentSystem
{
    private EntityQuery CameraQuery;
    private EntityQuery controlledQuery;

    protected override void OnCreate()
    {
        base.OnCreate();
        CameraQuery = GetEntityQuery(new EntityQueryDesc
        {
            All = new ComponentType[]
            {
                ComponentType.ReadWrite<CameraData>(), 
                ComponentType.ReadWrite<Transform>(), 
                ComponentType.ReadOnly<Translation>(), 
            },
            None = new ComponentType[]
            {
                //ComponentType.ReadOnly<DirectControlTag>(),
            }
        });
        
        controlledQuery = GetEntityQuery(typeof(PlayerControlTag), typeof(Transform));

    }

    protected override void OnUpdate()
    {
        var deltaTime = Time.deltaTime;
        // Wont work for mp
        var controlTranslationArray = controlledQuery.ToComponentArray<Transform>();
        var goPos = controlTranslationArray[0];
        
        Entities.With(CameraQuery).ForEach((Entity entity, Transform transform, ref CameraData data) =>
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                data.lockOn = !data.lockOn;
            }

            if (data.lockOn)
            {
                transform.position = goPos.position;
                return;
            }
            /*
            Physics.Raycast(transform.position, transform.forward, out var hit, 100);
            
            Vector3 pos = Camera.main.WorldToViewportPoint (goPos.position);
            if (math.distance(goPos.position, hit.point) > 5)
            {
                var lerpPos = goPos.position;
                lerpPos.y = transform.position.y;
                transform.position = Vector3.Lerp(transform.position, lerpPos, deltaTime * 10f);
            }*/
            var mDelta = 100; // Pixels. The width border at the edge in which the movement work
            var mSpeed = 3.0;
            if (Input.mousePosition.x >= Screen.width - mDelta ||
                Input.mousePosition.x <= 0 + mDelta ||
                Input.mousePosition.y >= Screen.height - mDelta ||
                Input.mousePosition.y <= 0 + mDelta

                )
            {
                
                
                var a = data.PanSpeed * deltaTime * Vector3.right;
                var b = Input.mousePosition.x - Screen.width * 0.5f;
                var c = (Screen.width * 0.5f);
                transform.Translate(a * b / c, Space.World);

                var x = data.PanSpeed * deltaTime * Vector3.forward;
                var y = Input.mousePosition.y - Screen.height * 0.5f;
                var z = Screen.height * 0.5f;
                transform.Translate(x * y / z, Space.World);
            }
            


            //ClampPosition(transform, controlTranslation);
        });
        
    }


    private void ClampPosition(Transform tr, Translation t)
    {
        Vector3 pos = Camera.main.WorldToViewportPoint (t.Value);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        tr.position = Camera.main.ViewportToWorldPoint(pos);
    }
}