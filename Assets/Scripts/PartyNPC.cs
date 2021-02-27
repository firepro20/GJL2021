using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyNPC : MonoBehaviour
{
    public Power obtainablePower;
    public ColorDatabase characterColorsDatabase;

    private SpriteRenderer sprRenderer;
    private string[] obtainedDialogue =
    {
        "MOVE",
        "SIR TROIS: \"There's never two without three!\"\n\nSir Trois has joined your party! This lad can ADD + 3 to the boxes.",
        "DON CINCO: \"Boo-yah! I'm free! Give me five!\"\n\nDon Cinco has joined your party! This man can ADD +5 to the boxes.",
        "SYV: \"You want me to join you? Sure. Just don't get in my way.\"\n\nSyv has joined your party! This dude can ADD +7 to the boxes.",
        "DIVISION",
        "RAY SETH: \"Hey… are you stuck here, too? Wanna join forces?\"\n\nRay Seth has joined your party! This guy can RESET the value on the boxes. Press SPACE to switch party members!"
    };
    void Start()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        sprRenderer.material.SetColor("_RedColorReplace", characterColorsDatabase.colors[(int)obtainablePower]);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            AudioController.Instance.PartyJoined();
            Player player = col.gameObject.GetComponent<Player>();
            Debug.Log("[PartyNPC] Power obtained: " + obtainablePower);
            GameManager.Instance.gameUIController.ShowDialogue(obtainedDialogue[(int)obtainablePower]);
            player.UpdatePower(obtainablePower, true);
            Destroy(gameObject);
        }
    }

}
