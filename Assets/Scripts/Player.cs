using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Power
{
    ADD,
    MULTIPLY,
    POWER,
    DIVIDE
}

public class Player : MonoBehaviour
{
    // Character Power
    int[] enabledPowers = { 0, 0, 0, 0 };
    Power myPower = Power.ADD;

    // Snap Tile Correction
    Vector2 tileOffset = new Vector2(0.5f, 0.5f);

    // Start is called before the first frame update
    void Start()
    {
        UpdatePower(Power.ADD, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetGameState() == GameState.Playing)
        {
            UpdateMovement();
        }
    }

    void UpdateMovement()
    {

    }

    /// <summary>
    /// Set Allowed Power once discovered
    /// </summary>
    /// <param name="p">Power to allow</param>
    /// <param name="state"></param>
    void UpdatePower(Power p, bool state)
    {
        int s = state ? 1 : 0;
        enabledPowers[(int)p] = s;
    }

    public int[] GetAllowedPowers()
    {
        return enabledPowers;
    }

}
