using System.Threading;

using UnityEngine;




public class chainspawn : MonoBehaviour
{

    public GameObject chain;
    public Transform Spawnpoint;
    public float Spawntimer;
    public float TimeToSpawn;

    public float minX = -5f;          
    public float maxX = 5f; 

    void Awake()
    {

    }

   
    void FixedUpdate()
    {
        TimeToSpawn -= Time.deltaTime;

        float randomX = Random.Range(minX, maxX);

        Vector3 randomSpawnPosition = new Vector3(randomX, Spawnpoint.position.y, Spawnpoint.position.z);

        if(Spawntimer >= TimeToSpawn)
        {
        
        Instantiate(chain, randomSpawnPosition , Quaternion.Euler(0, 180, 0));
        TimeToSpawn = 2;
        }

    }
}
