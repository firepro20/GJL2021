using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyNPC : MonoBehaviour
{
    public Power obtainablePower;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Player player = col.gameObject.GetComponent<Player>();
            Debug.Log("[PartyNPC] Power obtained: " + obtainablePower);
            Destroy(gameObject);
        }
    }
}
