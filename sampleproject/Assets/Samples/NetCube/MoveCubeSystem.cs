using Samples.NetCube;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;

[UpdateInGroup(typeof(GhostPredictionSystemGroup))]
public class MoveCubeSystem : SystemBase
{
    GhostPredictionSystemGroup m_GhostPredictionSystemGroup;
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
                trans.Value.x += deltaTime;
            if (input.horizontal < 0)
                trans.Value.x -= deltaTime;
            if (input.vertical > 0)
                trans.Value.z += deltaTime;
            if (input.vertical < 0)
                trans.Value.z -= deltaTime;
        }).ScheduleParallel();


        Entities.ForEach((ref Translation trans, ref PatrolComponent patrol) =>
        {
            if (trans.Value.x > patrol.PosLimit)
                patrol.MovingRight = false;
            if (trans.Value.x < -patrol.PosLimit)
                patrol.MovingRight = true;
            trans.Value.x += patrol.MovingRight ? patrol.Speed * deltaTime : -patrol.Speed * deltaTime;
        }).ScheduleParallel();

    }
}
