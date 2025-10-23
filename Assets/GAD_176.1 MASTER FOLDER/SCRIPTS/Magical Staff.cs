using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class MagicalStaff : WeaponScript
{
    public float range = 100f;
    public Camera fpsCam;




    // Update is called once per frame
    void Update()
    {
        {
            if (Input.GetButtonDown("Fire1")) // Fire1 is CTRL button

            {
                Shoot(); // Once the player has pressed the required button, the shoot function is called
            }
        }



            void Shoot()
            {
                RaycastHit hit;
                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
                {
                    Debug.Log(hit.transform.name); // Sends a message to the console showing which object has been hit, purely aesthetical, not functional. This makes sure I am actually hitting the object

                    EnemyAIScript enemy = hit.transform.GetComponent<EnemyAIScript>(); // The actual code that determines what object is being hit, which will refer back to the EnemyAIScript
                    if (enemy != null) // If enemy exists
                    {
                        enemy.TakeDamage(damage);
                    }
                }

            }
    }
}
