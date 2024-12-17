using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    public int Money;
    public Checkpoint Checkpoint;
}