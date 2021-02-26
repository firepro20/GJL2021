using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Dictionary<NumberBox, Vector3> numberBoxInitialPos = new Dictionary<NumberBox, Vector3>();
    public Transform spawnPosition;

    private SlotsObserver SlotsObserver;
    void Start()
    {
        // gathering all boxes
        foreach (NumberBox box in GetComponentsInChildren<NumberBox>())
        {
            numberBoxInitialPos.Add(box, box.transform.position);
        }

        SlotsObserver = GetComponentInChildren<SlotsObserver>();

        if (!spawnPosition)
        {
            Debug.LogWarning("[RoomManager] The spawn position is not assigned!", this);
        }
    }

    public void ResetRoom()
    {
        foreach (KeyValuePair<NumberBox, Vector3> pair in numberBoxInitialPos)
        {
            pair.Key.StopAllCoroutines();
            pair.Key.transform.position = pair.Value;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("[RoomManager] Player entered room: " + this, this);
            col.gameObject.GetComponent<Player>().currentRoom = this;
            GameManager.Instance.gameUIController.ShowEquation();
            SlotsObserver.CalculateResult();
            UpdateEquation();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("[RoomManager] Player exited room: " + this, this);
            col.gameObject.GetComponent<Player>().currentRoom = null;
            GameManager.Instance.gameUIController.HideEquation();
        }
    }

    public void UpdateEquation()
    {
        List<string> numberList = new List<string>();
        List<Color> colorList = new List<Color>();
        foreach (BoxSlot slot in SlotsObserver.boxSlots)
        {
            if (slot.GetBoxOnTop())
            {
                numberList.Add(slot.GetBoxOnTop().GetNumberValue().ToString());
            }
            else
            {
                numberList.Add("X");
            }

            colorList.Add(slot.slotColor);
        }

        GameManager.Instance.gameUIController.UpdateEquation(numberList, colorList, SlotsObserver.expectedResult.ToString());
    }
}