using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    [Header("Properties")]
    public bool Dead = false;
    public int  Money = 0;
[Space(10)]
    [InlineEditor]
    public PlayerData playerData;


    [HideInInspector] public PlayerMovement playerMovement;
    [HideInInspector] public PlayerSFX      playerSFX;



    void Awake()
    {
        //Assign Scripts
        playerMovement = GetComponent<PlayerMovement>();
        playerSFX      = FindAnyObjectByType<PlayerSFX>();
    }


    public void Die()
    {
        Dead = true;
        playerSFX.StopSound(playerSFX.Damage);
        playerSFX.PlaySound(playerSFX.Death);
    }
}
