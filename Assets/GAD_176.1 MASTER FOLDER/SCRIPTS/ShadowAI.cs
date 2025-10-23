using UnityEngine;

public class ShadowAI : EnemyAIScript
{
    private Transform player;
    private float teleportDistance = 10f; // Distance of teleporting 
    private float teleportCooldown = 5f; // Time between teleporting 
    private float rotationSpeed = 5f; // Speed of AI when locking onto the player
    private float teleportTimer;

    void Start()
    {
        teleportTimer = teleportCooldown;
        
    }


    // Update is called once per frame
    private void Update()
    {

        if (player == null) // If the player is not present in the scene, do nothing
        {
            return;
        }

        teleportCooldown -= Time.deltaTime;

    }
}
