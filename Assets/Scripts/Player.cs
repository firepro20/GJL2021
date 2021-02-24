using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Power
{
    ADD, // +3
    MULTIPLY, // x2
    POWER, // ^2
    DIVIDE // /4
}

public class Player : MonoBehaviour
{
    // defines function and parameters if required
    public delegate void OnPowerUpdateHandler(Power p, bool state);
    // event to subsbribe to
    public event OnPowerUpdateHandler OnPowerUpdated;

    // Movement
    Vector3 movement;
    Vector3 moveToPosition;
    bool isWalking = false;
    public Transform spawnPoint;
    public float moveSpeed = 1.5f;

    // Wall Tilemap
    public Tilemap wallObstacles;

    // Character Power
    int[] characterPowers = { 0, 0, 0, 0 };
    Power myPower = Power.ADD;

    // Snap Tile Correction
    Vector2 tileOffset = new Vector2(0.5f, 0.5f);

    private void OnEnable()
    {
        OnPowerUpdated += UpdatePower;
    }

    private void OnDisable()
    {
        OnPowerUpdated -= UpdatePower;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = spawnPoint.position;
        OnPowerUpdated?.Invoke(Power.ADD, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetGameState() == GameState.Playing)
        {
            UpdateMovement();
            SwitchCharacter();
            SwitchSelectedBox();
            //Interact(); // probably wont need this as I will only interact with targeted box, call the operation of this box when I calculate it here and update value sent to thanik script
        }
    }

    void UpdateMovement()
    { 
        if (!isWalking)
        {
            // Using Raw for unfiltered input, no smoothing applied
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            
            //Avoid diagonal movement
            if (movement.x != 0)
            {
                movement.y = 0;
            }

            moveToPosition = transform.position + new Vector3(movement.x, movement.y, 0); //+-1
            Vector3Int wallMapTile = wallObstacles.WorldToCell(moveToPosition);

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

    void SwitchCharacter()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }

    void SwitchSelectedBox()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(moveToPosition, 0.2f);
    }

    /// <summary>
    /// Set Allowed Power once discovered
    /// </summary>
    /// <param name="p">Power to allow</param>
    /// <param name="state"></param>
    void UpdatePower(Power p, bool state)
    {
        int s = state ? 1 : 0;
        characterPowers[(int)p] = s;
    }

    public int[] GetAllowedPowers()
    {
        return characterPowers;
    }
}
