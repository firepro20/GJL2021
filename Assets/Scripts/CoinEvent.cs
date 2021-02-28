using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinEvent : MonoBehaviour
{
    public UnityEvent paidEvent;

    public int coinAmount;
    // void Start()
    // {
    //     
    // }
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     
    // }

    public void Pay(Player player)
    {
        if (player.GetNumOfCoins() >= coinAmount)
        {
            paidEvent.Invoke();
            player.RemoveNumOfCoins(coinAmount);
            //GameManager.Instance.gameUIController.ShowDialogue(coinAmount + " coin(s) used for unlocking door.");
        }
        else
        {
            GameManager.Instance.gameUIController.ShowDialogue("Not enough coin! Need " + coinAmount + " coin(s) to unlock.");
        }
    }
}
