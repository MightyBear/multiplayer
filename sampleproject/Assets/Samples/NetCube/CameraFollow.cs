using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.NetCode;

public class CameraFollow : MonoBehaviour
{
    public Entity entityToFollow;
    public float3 offset = float3.zero;
    private bool lateInit = true;
    private EntityManager manager;
    private int frameDelay = 10;
    private int frameCount = 0;

    private void Awake()
    {
        var worlds = World.All;
        foreach (var world in worlds)
        {
            Debug.Log($"Checking in {world.Name}");
            if (world.Name == "ClientWorld0")
                manager = world.EntityManager;
        }

    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (lateInit && frameCount >= frameDelay)
        {
            var worlds = World.All;
            foreach (var world in worlds)
            {
                if (world.Name == "ClientWorld0")
                    manager = world.EntityManager;
            }

            var query = new EntityQueryDesc
            {
                None = new ComponentType[] {ComponentType.ReadOnly<PatrolComponent>()},
                All = new ComponentType[] {typeof(Translation), ComponentType.ReadOnly<PredictedGhostComponent>()}
            };

            entityToFollow = manager.CreateEntityQuery(query).GetSingletonEntity();
            lateInit = false;
        }
        else if(lateInit)
            ++frameCount;

        if(entityToFollow == Entity.Null)
            return;
        Translation entPos = manager.GetComponentData<Translation>(entityToFollow);
        transform.position = entPos.Value + offset;
    }
}
