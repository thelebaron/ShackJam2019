using ShackJam;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

public class PlayerPawnController : ComponentSystem
{
    public EntityQuery ControlledPawnQuery;
    private float3 mousepos;
    private Transform clickView;
    protected override void OnCreate()
    {
        base.OnCreate();
        ControlledPawnQuery = GetEntityQuery(typeof(PlayerControlTag), typeof(PawnData), typeof(Agent), typeof(Pawn));
        var clickPosGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        clickView = clickPosGo.transform;
        clickView.localScale = 0.35f * Vector3.one;
        
        var ma = Addressables.LoadAssetAsync<Material>("Assets/Materials/HotPink.mat");
        //var m = Addressables.InstantiateAsync("Assets/Materials/HotPink.mat");

        clickPosGo.GetComponent<MeshRenderer>().material = ma.Result;
        
        

    }

    protected override void OnUpdate()
    {
        
        bool hasSingleton = HasSingleton<PlayerInputData>();
        if (hasSingleton)
        {
            var singletonMousePos = GetSingleton<PlayerInputData>();
            mousepos = singletonMousePos.CurrentMousePosition;
        }

        
        
        Entities.With(ControlledPawnQuery).ForEach((Entity entity, Pawn pawn, ref Agent agent) =>
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                agent.SetDestination(mousepos);
                clickView.position = mousepos;
            }

        });
    }

}