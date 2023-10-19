using System;
using UnityEngine;

namespace Audio
{
    [Serializable]
    public class ClipData
    {
        [field:SerializeField] public ClipId Id { get; private set; }
        [field:SerializeField] public AudioClip Clip { get; private set; }
    }
}