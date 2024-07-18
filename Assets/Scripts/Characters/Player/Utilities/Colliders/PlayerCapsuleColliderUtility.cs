using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenShinImpactMovementSystem
{
    [Serializable]
    public class PlayerCapsuleColliderUtility : CapsuleColliderUtility
    {
        [field:SerializeField] public PlayerTriggerColliderData TriggerColliderData { get;private set; }
    }
}
