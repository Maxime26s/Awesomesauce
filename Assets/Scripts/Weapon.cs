using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public PlayerActions playerActions;

    // Start is called before the first frame update
    void Awake()
    {
        playerActions = gameObject.GetComponentInParent<PlayerActions>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "EntityParts" || other.tag == "Entity")
        {
            GameObject hit = other.gameObject;
            while (hit != null && !playerActions.entityHit.Contains(hit))
            {
                if (hit.tag == "Entity")
                {
                    hit.GetComponent<Entity>().TakeDamage(5);
                    playerActions.entityHit.Add(hit);
                    break;
                }
                hit = hit.transform.parent.gameObject;
            }
        }
    }
}
