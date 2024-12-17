using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool Triggered = false;

    private Ghost ghost;
    private PlayerStats playerStats;
    
    void Awake()
    {
        ghost = FindAnyObjectByType<Ghost>();
        playerStats = FindAnyObjectByType<PlayerStats>();
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Player") TriggerCheckpoint();
    }

    public void TriggerCheckpoint()
    {
        if(Triggered) return;
        Triggered = true;

        CheckpointManager checkpointManager = GetComponentInParent<CheckpointManager>();

        ghost.TeleportNext();
        playerStats.playerData.Checkpoint = this;
        playerStats.playerData.Money = playerStats.Money;

        checkpointManager.checkpoints.Remove(this);
    }
}
