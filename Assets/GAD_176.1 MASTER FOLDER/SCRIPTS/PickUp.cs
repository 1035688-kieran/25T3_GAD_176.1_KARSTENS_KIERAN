using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] protected GameObject ItemOnPlayer;
    [SerializeField] protected bool willCauseDamage;
    protected void Start()
    {
        willCauseDamage = false;
        ItemOnPlayer.SetActive(false); // On start the players weapon is rendered e
    }

    protected void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                this.gameObject.SetActive(false); // Physical object in the game is rendered invisible
                ItemOnPlayer.SetActive(true); // holdable object in the game is rendered visible
                willCauseDamage = true;
                Debug.Log("Player Has Picked up Weapon");
                
            }
        }
    }
}
