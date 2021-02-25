using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class NumberBox : MonoBehaviour
{
    public int initialNumberValue;
    public int maxOperations = 5;
    public float moveSpeed = 3f;
    private int numberValue;
    private int operationsCount = 0;

    public TMP_Text numberText;
    public Sprite[] operationIndicatorSprites;
    public Image[] operationIndicators;

    private BoxSlot slot;
    void Start()
    {
        numberText.text = numberValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        numberText.text = numberValue.ToString();
        for (int i = 0; i < maxOperations; i++)
        {
            if (i < operationsCount)
            {
                operationIndicators[i].sprite = operationIndicatorSprites[1];
            }
            else
            {
                operationIndicators[i].sprite = operationIndicatorSprites[0];
            }
        }
    }

    public void ResetOperations()
    {
        operationsCount = 0;
        numberValue = initialNumberValue;
        slot?.TriggerCalculateResult();
        UpdateUI();
    }

    public void SetNumberValue(int newValue)
    {
        if (operationsCount < maxOperations)
        {
            numberValue = newValue;
            operationsCount++;
        }
        slot?.TriggerCalculateResult();
        UpdateUI();
    }

    public int GetNumberValue()
    {
        return numberValue;
    }

    public bool MoveBox(Vector3 direction, Tilemap wallObstacles)
    {

        Vector3 moveToPosition = transform.position + direction;
        Vector3Int wallMapTile = wallObstacles.WorldToCell(moveToPosition);

        if (wallObstacles.GetTile(wallMapTile) == null)
        {
            RaycastHit2D hit = Physics2D.Raycast(moveToPosition, Vector2.up, 0f);
            if (hit.collider != null && hit.collider.CompareTag("Door"))
            {
                return false;
            }
            else
            {
                StartCoroutine(Move(moveToPosition));
                return true;
            }

        }
        else
        {
            return false;
        }
    }

    IEnumerator Move(Vector3 newPos)
    {
        while ((newPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = newPos;
    }

    public void SetSlot(BoxSlot slot)
    {
        this.slot = slot;
    }
}
