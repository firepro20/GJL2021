using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Power
{
    ADD,
    MULTIPLY,
    POWER,
    DIVIDE
}

public class Player : MonoBehaviour
{
    // Movement
    Vector3 movement;
    Vector3 moveToPosition;
    bool isWalking = false;
    public Transform spawnPoint;
    public float moveSpeed = 1.5f;

    // Wall Tilemap
    public Tilemap wallObstacles;

    // Character Power
    int[] enabledPowers = { 0, 0, 0, 0 };
    Power myPower = Power.ADD;

    // Snap Tile Correction
    Vector2 tileOffset = new Vector2(0.5f, 0.5f);

    // Start is called before the first frame update
    void Start()
    {
        transform.position = spawnPoint.position;
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
        if (!isWalking)
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");

            //Avoid diagonal movement
            if (movement.x != 0)
            {
                movement.y = 0;
            }

            moveToPosition = transform.position + new Vector3(movement.x, movement.y, 0); //+-1
            Vector3Int wallMapTile = wallObstacles.WorldToCell(moveToPosition - new Vector3(0, 0.5f, 0));

            if (wallObstacles.GetTile(wallMapTile) == null)
            {
                // update anim sprites
                StartCoroutine(Move(moveToPosition));
            }
        }
    }

    IEnumerator Move(Vector3 newPos)
    {
        isWalking = true;

        while ((newPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = newPos;

        isWalking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(moveToPosition - new Vector3(0,0.5f,0), 0.2f);
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
