using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [ToggleLeft]
    public bool ResetCheckpoint;

    public List<Checkpoint> checkpoints;

    private PlayerMovement playerMovement;
    private PlayerStats playerStats;

    void Start()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        playerStats = FindAnyObjectByType<PlayerStats>();

        if(ResetCheckpoint) playerStats.playerData.Checkpoint = null;
        UpdateCheckpoints();
    }

    [Button(DisplayParameters = true, Style = ButtonStyle.FoldoutButton)]
    void UpdateCheckpoints()
    {
        Checkpoint SavedCheckpoint = playerStats.playerData.Checkpoint;
        
        foreach(Checkpoint checkpoint in checkpoints.ToList())
        {
            if(checkpoints.IndexOf(checkpoint) <= checkpoints.IndexOf(SavedCheckpoint))
                checkpoint.TriggerCheckpoint();
        }

        SavedCheckpoint = playerStats.playerData.Checkpoint;
        if(SavedCheckpoint != null) playerMovement.Teleport(SavedCheckpoint.transform);
    }
}
