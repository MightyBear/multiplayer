// Copyright © 2017 - 2020 Mighty Bear Games. All rights reserved.

using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

public class ClientServerTickRateAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    [SerializeField] private int networkTickRate = 30;
    [SerializeField] private int simulationTickRate = 30;
    [SerializeField] private int maxSimulationStepsPerFrame = 8;


    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(
            entity,
            new ClientServerTickRate
            {
                NetworkTickRate = networkTickRate,
                SimulationTickRate = simulationTickRate,
                MaxSimulationStepsPerFrame = maxSimulationStepsPerFrame
            }
        );
    }
}

