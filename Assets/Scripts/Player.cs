using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Power
{
    MOVE,
    ADD, // +3
    MULTIPLY, // x2 
    POWER, // ^2
    DIVIDE, // /4
    RESET
}

public class Player : MonoBehaviour
{
    // defines function and parameters if required
    public delegate void OnPowerUpdateHandler(Power p, bool state);
    public delegate void OnPowerUIUpdateHandler(Power p);
    public delegate void OnPartyMembersUpdateHandler(int[] discoveredPowers);
    // event to subsbribe to
    public event OnPowerUpdateHandler OnPowerUpdated;
    public event OnPowerUIUpdateHandler OnPowerUIUpdated;
    public event OnPartyMembersUpdateHandler OnPartyMembersUpdated;

    // Movement
    Vector3 movement;
    private Vector3 oldMovement;
    Vector3 moveToPosition;
    bool isWalking = false;
    public Transform spawnPoint;
    public float moveSpeed = 1.5f;

    // Animation
    public Animator animator;

    // Wall Tilemap
    public Tilemap wallObstacles;

    // Character Power
    int[] characterPowers = { 0, 0, 0, 0, 0, 0 };
    Power myPower;
    Vector3 boxPositionCheck;
    int powerIndex = 0;

    // character color
    public Color[] characterColors;
    private SpriteRenderer sprRenderer;
    public RoomManager currentRoom;

    // coins
    private int numOfCoins = 0;

    // hold time
    private float holdTime = 0;
    
    private void OnDisable()
    {
        OnPowerUpdated -= UpdatePower;
        OnPowerUIUpdated -= GameManager.Instance.gameUIController.UpdateCurrentPower;
        OnPartyMembersUpdated -= GameManager.Instance.gameUIController.UpdatePartyMembersUI;
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = spawnPoint.position;
        sprRenderer = GetComponent<SpriteRenderer>();

        OnPowerUpdated += UpdatePower;
        OnPowerUIUpdated += GameManager.Instance.gameUIController.UpdateCurrentPower;
        OnPartyMembersUpdated += GameManager.Instance.gameUIController.UpdatePartyMembersUI;

        OnPowerUpdated?.Invoke(Power.MOVE, true);
        OnPowerUpdated?.Invoke(Power.ADD, true); // remove before release
        OnPowerUpdated?.Invoke(Power.RESET, true);
        OnPartyMembersUpdated?.Invoke(characterPowers);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetGameState() == GameState.Playing)
        {
            UpdateMovement();
            SwitchPower();
            ListenForResetRoom();
        }
    }

    void UpdateMovement()
    {
        // Using Raw for unfiltered input, no smoothing applied
        movement.x = Mathf.Round(Input.GetAxisRaw("Horizontal"));
        movement.y = Mathf.Round(Input.GetAxisRaw("Vertical"));

        if (movement != Vector3.zero && movement == oldMovement)
        {
            if (holdTime < 0.25f)
            {
                holdTime += Time.deltaTime;
            }
            else
            {
                movement = Vector3.zero;
                holdTime = 0;
            }
        }
        else
        {
            holdTime = 0;
        }

        if (!isWalking && movement != oldMovement)
        {
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
                LayerMask mask = LayerMask.GetMask("RoomObject");
                RaycastHit2D hit = Physics2D.Raycast(moveToPosition, Vector2.up, 0f, mask);
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Box"))
                    {
                        NumberBox box = hit.collider.GetComponent<NumberBox>();
                        Debug.Log("[Player] Found box next to player", hit.collider.gameObject);
                        OperateOnBox(box);
                    }
                    else if (hit.collider.CompareTag("Door"))
                    {
                        Debug.Log("[Player] Found door next to player", hit.collider.gameObject);
                        if (hit.collider.GetComponent<Door>().GetUnlocked())
                        {
                            StartCoroutine(Move(moveToPosition));
                        }
                    }
                    else if (hit.collider.CompareTag("Coin"))
                    {
                        IncrementNumOfCoins();
                        Destroy(hit.collider.gameObject);
                        StartCoroutine(Move(moveToPosition));
                        GameManager.Instance.gameUIController.UpdateCoinUI(numOfCoins);
                    }
                    else if (hit.collider.CompareTag("CoinDoor"))
                    {
                        if (hit.collider.GetComponent<Door>().GetUnlocked())
                        {
                            StartCoroutine(Move(moveToPosition));
                        }
                        else
                        {
                            hit.collider.GetComponent<CoinEvent>().Pay(this);
                            GameManager.Instance.gameUIController.UpdateCoinUI(numOfCoins);
                        }
                    }
                    else if (hit.collider.CompareTag("Slot"))
                    {
                        if (hit.collider.GetComponent<BoxSlot>().GetBoxOnTop())
                        {
                            OperateOnBox(hit.collider.GetComponent<BoxSlot>().GetBoxOnTop());
                        }
                        else
                        {
                            StartCoroutine(Move(moveToPosition));
                        }
                    }
                    else if (hit.collider.CompareTag("ExitHole"))
                    {
                        StartCoroutine(Move(moveToPosition));
                    }
                    else if (hit.collider.gameObject != this.gameObject)
                    {
                        Debug.LogWarning("[Player] Found unknown object: " + hit.collider.gameObject, hit.collider.gameObject);
                    }
                }
                else
                {
                    // update anim sprites
                    StartCoroutine(Move(moveToPosition));
                }
            }

            oldMovement = movement;
        }
    }

    IEnumerator Move(Vector3 newPos)
    { 
        isWalking = true;
        //Debug.Log("Before X " + movement.x + " Y " + movement.y);
        // Animate
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", 1);
        while ((newPos - transform.position).sqrMagnitude > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        //Debug.Log("After X " + movement.x + " Y " + movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        transform.position = newPos;
        currentRoom?.UpdateEquation();
        isWalking = false;
    }

    void SwitchPower()
    {
        if (Input.GetButtonDown("Switch"))
        {
            do
            {
                powerIndex++;
                if (powerIndex >= characterPowers.Length)
                {
                    powerIndex = 0;
                }
            } while (characterPowers[powerIndex] != 1);

            myPower = (Power)powerIndex;
            OnPowerUIUpdated?.Invoke(myPower);
            UpdateColor();
            Debug.Log("My power is - " + myPower);

        }
    }

    void OperateOnBox(NumberBox box)
    {
        switch (myPower)
        {
            case Power.MOVE:
                if (box.MoveBox(movement, wallObstacles))
                {
                    StartCoroutine(Move(moveToPosition));
                }
                break;
            case Power.ADD:
                int currentValue = box.GetNumberValue();
                box.SetNumberValue(currentValue + 3);
                currentRoom.UpdateEquation();
                break;
            case Power.RESET:
                box.ResetOperations();
                currentRoom.UpdateEquation();
                break;
            default:
                break;
        }
    }

    void ListenForResetRoom()
    {
        if (Input.GetButtonDown("Reset"))
        {
            Debug.Log("Reset room");
            if (currentRoom)
            {
                currentRoom.ResetRoom();
                StopAllCoroutines();
                isWalking = false;
                transform.position = currentRoom.spawnPosition.position;
            }
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

    public void SetAllowedPowers(int[] cp)
    {
        characterPowers = cp;
    }

    public Power GetCurrentPower()
    {
        return myPower;
    }

    void UpdateColor()
    {
        sprRenderer.material.SetColor("_RedColorReplace", characterColors[(int)myPower]);
    }

    public int GetNumOfCoins()
    {
        return numOfCoins;
    }

    public void IncrementNumOfCoins()
    {
        numOfCoins++;
    }

    public void RemoveNumOfCoins(int amount)
    {
        numOfCoins -= amount;
        if (numOfCoins < 0)
        {
            numOfCoins = 0;
        }
    }
}
