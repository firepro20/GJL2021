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
        "\"There's never two without three!\"\nSumboi has joined your party! This lad can ADD + 3 to the boxes.",
        "\"How did I end up here? I can’t remember… But I can help you escape.\"\nMull T. Plyer has joined your party! This man can MULTIPLY x2.",
        "\"Looks like we can help each other… I’m Powermann. Nice to meet ya\"\nPowermann has joined your party! This dude can SQUARE the box numbers!",
        "DIVISION",
        "\"Hey… are you stuck here, too? Wanna join forces?\"\nRay Seth has joined your party! This guy can RESET the value on the boxes. Press SPACE to switch party members!"
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
            Player player = col.gameObject.GetComponent<Player>();
            Debug.Log("[PartyNPC] Power obtained: " + obtainablePower);
            GameManager.Instance.gameUIController.ShowDialogue(obtainedDialogue[(int)obtainablePower]);
            player.UpdatePower(obtainablePower, true);
            Destroy(gameObject);
        }
    }

}
