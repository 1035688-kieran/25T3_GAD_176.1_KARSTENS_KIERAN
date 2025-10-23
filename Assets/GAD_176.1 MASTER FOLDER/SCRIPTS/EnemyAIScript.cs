using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIScript : MonoBehaviour
{

    [SerializeField] private float health = 50f;
    [SerializeField] private Vector3 initialLocation; // This is where the AI will spawn originally

    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Transform player;
    [SerializeField] protected LayerMask whatIsPlayer, whatIsGround; // custom collision detectors

    [SerializeField] protected Vector3 walkPoint; // First part of the patrol phase, this is where the AI will be walking to
    [SerializeField] bool walkPointSet; // bool condition to figure /// This bool has been serialised as to check within the time that the code is running as to see if its being triggered
    [SerializeField] protected float walkPointRange;

    [SerializeField] protected float timeBetweenAttacks; // time between each attack
    [SerializeField] bool alreadyAttacked; // bool statement to check if AI has attacked as to not attack in multiple succession /// This bool has been serialised as to check within the time that the code is running as to see if its being triggered

    [SerializeField] protected float sightRange, attackRange; // Floats for when the player is in a certain visible range or physical range of the AI
    [SerializeField] protected bool playerInSightRange, playerInAttackRange; // Bool checks if the player is in sight of the enemy or if it is the attack range of the enemy, these are different triggers with different ranges /// This bool has been serialised as to check within the time that the code is running as to see if its being triggered

    [SerializeField] protected GameObject projectile; 

    protected void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    protected void Update() // checks every frame
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer); // Checks for sight range every update frame
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer); // Checks for attack range every update frame

        if (!playerInSightRange && !playerInAttackRange) Patrolling();// If player is NOT in sight and/or attack range, continue function of patrolling
        if (playerInSightRange && !playerInAttackRange) ChasePlayer(); // If player is in sight and not attack range, start function of chase
        if (playerInSightRange && playerInAttackRange) AttackPlayer(); // If player is in sight and attack range, start function of attack
    }

    protected void Patrolling() // This is the function that controls the AI looking for player movement
    {
        Debug.Log("Player can not be seen or attack by the Enemy");
        if (!walkPointSet) SearchWalkPoint(); // If there is not point set for the AI to walk, start SearchWalkPoint function to find one

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f) // Defined walk point has been reached
            walkPointSet = false; // Searching for a new point to walk to
    }

    protected void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) // The walk point that the AI is going to must never go outside of what is designated as "Ground"
            walkPointSet = true;
    }
    protected void ChasePlayer() // This is the function for the AI to chase the player
    {
        Debug.Log("Player can be seen but not attacked by the Enemy");
        agent.SetDestination(player.position); // The AI will try to set its position to the player position by moving on the xy axis, essentially chasing it
    }

    protected void AttackPlayer() //This is the function for the AI to attack the player
    {
        Debug.Log("Player can be seen and attacked by the Enemy");
        agent.SetDestination(transform.position);

        transform.LookAt(player); // EnemyAI will lock on with the player and not move while attacking the player

        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 32f, ForceMode.Impulse); // This gives the AI force, so when impact is made, the player will be sent slightly back
            rb.AddForce(transform.up * 32f, ForceMode.Impulse); // This gives the AI force, so when impact is made, the player will be sent slightly back

            alreadyAttacked = true; // The AI has attacked therefore this boolean is true
            Invoke(nameof(ResetAttack), timeBetweenAttacks); // If the AI has attacked then call function ResetAttack, after whatever time is specified by float "timebetweenattacks"
        }
    }

    protected void ResetAttack() // This is the function that will reset the attack and make sure that attacks dont stack
    {
        alreadyAttacked = false; // This function simply just sets alreadyattacked to false, rendering the AttackPlayer function null until the boolean is altered
    }
    public void TakeDamage(float amount) // This is the function for the AI to take damage
    {
            
            health -= amount;
            if (health < 0f)
            {
                Die(); // This starts the Die function, where the GameObject is deleted
            }
        
    }

 public void Die () // This function is kept public as it is applicable to both its ShadowAI and BlightAI children
    {

        Destroy(gameObject); // This destroys the GameObject aka deletes it from the scene
    }

    protected void OnDrawGizmosSelected() // Gizmos is a debugging element in Unity which showcases a visual element that can be controlled by vectors, I will be using this as a debugging element but also a visual element to show where the orbs of light are being shot.
    {
        Gizmos.color = Color.yellow; // Yellow is the color of the wireframe below, because yellow fits the enemys theme
        Gizmos.DrawWireSphere(transform.position, attackRange); // Renders a wireframe sphere in correlation to the vector and attackrange float
        Gizmos.color = Color.white; // White is the color of the wireframe below, because white is a complementary colour to yellow
        Gizmos.DrawWireSphere (transform.position, sightRange); // Renders a wireframe sphere in correlation to the vector and sightrange float
    }
}
