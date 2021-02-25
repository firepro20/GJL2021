using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Dictionary<NumberBox, Vector3> numberBoxInitialPos = new Dictionary<NumberBox, Vector3>();
    public Transform spawnPosition;
    void Start()
    {
        // gathering all boxes
        foreach (NumberBox box in GetComponentsInChildren<NumberBox>())
        {
            numberBoxInitialPos.Add(box, box.transform.position);
        }

        if (!spawnPosition)
        {
            Debug.LogWarning("[RoomManager] The spawn position is not assigned!", this);
        }
    }

    public void ResetRoom()
    {
        foreach (KeyValuePair<NumberBox, Vector3> pair in numberBoxInitialPos)
        {
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
            col.gameObject.GetComponent<Player>().currentRoom = this;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Player>().currentRoom = null;
        }
    }
}