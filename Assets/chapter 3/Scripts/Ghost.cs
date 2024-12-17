using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Transform mainTransform;
    public Transform floatyTransform;
[Space(12)]
    public int ActivePoint = 0;
[PropertySpace(SpaceBefore = 0, SpaceAfter = 20)]
    public List<Transform> Points;


    [ButtonGroup("Teleport")]
    [Button(DisplayParameters = true, Style = ButtonStyle.FoldoutButton)]
    public void TeleportToActive()
    {
        if(ActivePoint >= Points.Count) return;

        mainTransform.position = Points[ActivePoint].position;
        mainTransform.rotation = Points[ActivePoint].rotation;

        // Play Sound
    }

    [ButtonGroup("Teleport")]
    [Button(DisplayParameters = true, Style = ButtonStyle.FoldoutButton)]
    public void TeleportNext()
    {
        ActivePoint++;
        TeleportToActive();
    }
}
