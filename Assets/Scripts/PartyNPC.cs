using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyNPC : MonoBehaviour
{
    public Power obtainablePower;
    public ColorDatabase characterColorsDatabase;

    private SpriteRenderer sprRenderer;
    void Start()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        sprRenderer.material.SetColor("_RedColorReplace", characterColorsDatabase.colors[(int)obtainablePower]);

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Player player = col.gameObject.GetComponent<Player>();
            Debug.Log("[PartyNPC] Power obtained: " + obtainablePower);
            GameManager.Instance.gameUIController.ShowDialogue("Power " + obtainablePower + " Obtained!");
            player.UpdatePower(obtainablePower, true);
            Destroy(gameObject);
        }
    }

}
