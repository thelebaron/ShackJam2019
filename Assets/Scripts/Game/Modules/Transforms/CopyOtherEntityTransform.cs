using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace Game.Modules.Transforms
{
    [UpdateInGroup(typeof(TransformSystemGroup))]
    //[UpdateAfter(typeof(EndFrameLocalToParentSystem))]
    public class MirrorTransformSystem : JobComponentSystem
    {
        
        [BurstCompile]
        private struct MirrorLocalToWorldJob : IJobForEachWithEntity<MirrorLocalToWorld>
        {
            [NativeDisableParallelForRestriction] public ComponentDataFromEntity<LocalToWorld> LocalToWorldData;

            public void Execute(Entity entity, int index, [ReadOnly] ref MirrorLocalToWorld mirror)
            {
                if (LocalToWorldData.Exists(mirror.Target))
                    LocalToWorldData[entity] = new LocalToWorld{ Value = LocalToWorldData[mirror.Target].Value };
            }
        }
        
        [BurstCompile]
        private struct MirrorTranslationsJob : IJobForEachWithEntity<MirrorTranslation>
        {
            [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Translation> TranslationsDataFromEntity;

            public void Execute(Entity entity, int index, [ReadOnly] ref MirrorTranslation mirror)
            {
                if (TranslationsDataFromEntity.Exists(mirror.Target))
                    TranslationsDataFromEntity[entity] = new Translation{ Value = TranslationsDataFromEntity[mirror.Target].Value };
            }
        }
        
        [BurstCompile]
        private struct MirrorRotationsJob : IJobForEachWithEntity<MirrorRotation>
        {
            [NativeDisableParallelForRestriction] public ComponentDataFromEntity<Rotation> RotationsDataFromEntity;

            public void Execute(Entity entity, int index, [ReadOnly] ref MirrorRotation mirror)
            {
                if (RotationsDataFromEntity.Exists(mirror.Target))
                    RotationsDataFromEntity[entity] = new Rotation{ Value = RotationsDataFromEntity[mirror.Target].Value };

            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var mirrorLocaltoWorldJob = new MirrorLocalToWorldJob
            {
                LocalToWorldData = GetComponentDataFromEntity<LocalToWorld>()
            };
            var mirrorLocaltoWorldHandle = mirrorLocaltoWorldJob.Schedule(this, inputDeps);
            
            
            var mirrorTranslationsJob = new MirrorTranslationsJob
            {
                TranslationsDataFromEntity = GetComponentDataFromEntity<Translation>()
            };
            var mirrorTranslationsHandle = mirrorTranslationsJob.Schedule(this, mirrorLocaltoWorldHandle);
            
            var mirrorRotationsJob = new MirrorRotationsJob
            {
                RotationsDataFromEntity = GetComponentDataFromEntity<Rotation>()
            };

            return mirrorRotationsJob.Schedule(this, mirrorTranslationsHandle);
        }
    }
}