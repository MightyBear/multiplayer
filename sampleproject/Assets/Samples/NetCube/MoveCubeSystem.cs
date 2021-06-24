using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(GhostPredictionSystemGroup))]
public class MoveCubeSystem : SystemBase
{
    GhostPredictionSystemGroup m_GhostPredictionSystemGroup;
    float speed = 2;
    protected override void OnCreate()
    {
        m_GhostPredictionSystemGroup = World.GetExistingSystem<GhostPredictionSystemGroup>();
    }

    protected override void OnUpdate()
    {
        var tick = m_GhostPredictionSystemGroup.PredictingTick;
        var deltaTime = Time.DeltaTime;
        Entities.ForEach((DynamicBuffer<CubeInput> inputBuffer, ref Translation trans,
            in PredictedGhostComponent prediction) =>
        {
            if (!GhostPredictionSystemGroup.ShouldPredict(tick, prediction))
                return;
            CubeInput input;
            inputBuffer.GetDataAtTick(tick, out input);
            if (input.horizontal > 0)
                trans.Value.x += 2 * deltaTime;
            if (input.horizontal < 0)
                trans.Value.x -= 2 * deltaTime;
            if (input.vertical > 0)
                trans.Value.z += 2 * deltaTime;
            if (input.vertical < 0)
                trans.Value.z -= 2 * deltaTime;
        }).ScheduleParallel();


        Entities.ForEach((ref Translation trans,in PatrolComponent patrol) =>
        {
            if (trans.Value.x > 5f)
                speed *= -1;
            if (trans.Value.x < -5f)
                speed *= -1;
            trans.Value.x += deltaTime * speed;
        }).WithoutBurst().Run();

    }
}
