using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenShinImpactMovementSystem
{
    [Serializable]
    public class PlayerRotationData
    {
        [field:SerializeField]public Vector3 TargetRotationTime { get; private set; }

    }
}
