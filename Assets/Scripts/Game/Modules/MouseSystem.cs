using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ShackJam
{
    public struct UserInput : IComponentData
    {
        public bool LeftClick;
        public bool RightClick;
        public float3 MousePosition;
    }
    
    public struct PlayerInputData : IComponentData
    {
        public float3 CurrentMousePosition;
        public float3 ClickPosition;
    }
    
    public class MouseSystem : ComponentSystem
    {
        public EntityQuery MouseInputQuery;

        protected override void OnCreate()
        {
            base.OnCreate();
            MouseInputQuery = GetEntityQuery(typeof(UserInput));
        }

        protected override void OnUpdate()
        {
            Entities.With(MouseInputQuery).ForEach((Entity entity, ref UserInput input, ref PlayerInputData playerInputData) =>
                {
                    input.LeftClick = Input.GetKey(KeyCode.Mouse0);
                    input.RightClick = Input.GetKey(KeyCode.Mouse1);
                    input.MousePosition = Input.mousePosition;

                    var camera = Camera.main;
                    
                    if (camera != null)
                    {
                        var ray = camera.ScreenPointToRay(input.MousePosition);

                        if (Physics.Raycast(ray, out var hit)) 
                        {
                            var objectHit = hit.transform;
                            playerInputData.CurrentMousePosition = hit.point;

                            //Debug.Log(objectHit);
                            // Do something with the object that was hit by the raycast.
                        }
                    }
                });
        }
    }

}
