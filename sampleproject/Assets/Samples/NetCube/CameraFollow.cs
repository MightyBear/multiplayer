using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

namespace Samples.NetCube
{
    public class CameraFollow : MonoBehaviour
    {
        public float3 offset = float3.zero;
        private Entity EntityToFollow = Entity.Null;
        private EntityManager manager;

        private void Awake()
        {
            var worlds = World.All;
            foreach (var world in worlds)
            {
                if (world.Name == "ClientWorld0")
                    manager = world.EntityManager;
            }

        }

        private void InitialiseCamera()
        {
            var queryDesc = new EntityQueryDesc
            {
                None = new ComponentType[] {ComponentType.ReadOnly<PatrolComponent>()},
                All = new ComponentType[] {typeof(Translation), ComponentType.ReadOnly<PredictedGhostComponent>()}
            };

            EntityQuery query = manager.CreateEntityQuery(queryDesc);
            if(!query.IsEmpty)
                EntityToFollow = query.GetSingletonEntity();
        }

        // Update is called once per frame
        private void LateUpdate()
        {
            // -- Initialise camera upon first time moving the cube
            if (EntityToFollow == Entity.Null &&
                (Input.GetKey("w") ||
                 Input.GetKey("a") ||
                 Input.GetKey("s") ||
                 Input.GetKey("d")))
            {
                InitialiseCamera();
            }

            if (EntityToFollow == Entity.Null)
                return;

            Translation entPos = manager.GetComponentData<Translation>(EntityToFollow);
            transform.position = entPos.Value + offset;
        }
    }
}
