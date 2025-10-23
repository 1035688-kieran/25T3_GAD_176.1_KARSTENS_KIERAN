using UnityEngine;

public class WeaponScript : PickUp
{
    [SerializeField] protected int damage = 20;
    [SerializeField] protected float cooldown = 2f;
    [SerializeField] protected Vector3 spawn;

    


    private void BadStatusEffect()
    {
        
    }
}
