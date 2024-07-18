using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenShinImpactMovementSystem
{
    [Serializable]
    public class PlayerFallData
    {
        [field: SerializeField] [field:Range(1f, 15f)] public float FallSpeedLimit { get; private set; } = 15f;
        [field: SerializeField] [field:Range(0f, 100f)] public float MinimumDistanceToBeConsiderHardFall { get; private set; } = 3f;
    }
}
