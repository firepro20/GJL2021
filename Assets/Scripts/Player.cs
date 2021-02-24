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
    Vector3 boxPositionCheck;

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
            OperateOnBox(); // use similar functionality for moving to check if there are boxes in four directions with arrow keys
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
                // check for door and box
                RaycastHit2D hit = Physics2D.Raycast(moveToPosition, Vector2.up, 0f);
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Box"))
                    {
                        Debug.Log("[Player] Found box next to player", hit.collider.gameObject);
                        if (hit.collider.GetComponent<NumberBox>().MoveBox(movement, wallObstacles))
                        {
                            StartCoroutine(Move(moveToPosition));
                        }
                    }
                    else if (hit.collider.CompareTag("Door"))
                    {
                        Debug.Log("[Player] Found door next to player", hit.collider.gameObject);
                        if (hit.collider.GetComponent<Door>().GetUnlocked())
                        {
                            StartCoroutine(Move(moveToPosition));
                        }
                    }
                    else if (hit.collider.CompareTag("Slot") && !hit.collider.GetComponent<BoxSlot>().GetBoxOnTop())
                    {
                        StartCoroutine(Move(moveToPosition));
                    }
                }
                else
                {
                    // update anim sprites
                    StartCoroutine(Move(moveToPosition));
                }
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

    void OperateOnBox()
    {
        boxPositionCheck = transform.position;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            boxPositionCheck = transform.position + new Vector3(0, 1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            boxPositionCheck = transform.position + new Vector3(0, -1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            boxPositionCheck = transform.position + new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            boxPositionCheck = transform.position + new Vector3(1, 0, 0);
        }

        // check for box
        RaycastHit2D hit = Physics2D.Raycast(boxPositionCheck, Vector2.up, 0f);
        if (hit.collider != null && hit.collider.CompareTag("Box"))
        {
            NumberBox current = hit.collider.GetComponent<NumberBox>();
            current.SetNumber(99);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(moveToPosition, 0.2f);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(moveToPosition, Vector2.up);
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(boxPositionCheck, Vector2.up);
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
