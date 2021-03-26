using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinPickup : MonoBehaviour
{
    public int coinNumber;

    public Text coinText;

    //Awake is called when the object is loaded
    void Awake()
    {
        coinNumber = 0;
        coinText.text = ("Coins: " + coinNumber);
    }

    //When player touches coin, destroy coin and add one to number of coins player has
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Coin")
        {
            Destroy(other.gameObject);
            coinNumber += 1;
            coinText.text = ("Coins: " + coinNumber);
        }
    }    
}
