using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CoinPickup : MonoBehaviour
{
    public int coinNumber = 0; //Coins the player has collected in one block
    public int totalCoins = 0; 
    public Text coinText; 
    [SerializeField] private string gamifiedScene = "GamifiedScene";

    //Awake is called when the object is loaded
    void Awake()
    {
        coinNumber = 0;
        totalCoins = 0;
        coinText.text = ("Coins: " + coinNumber);
    }

    //When player touches coin, destroy coin and add one to number of coins player has
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Coin")
        {
            if((SceneManager.GetActiveScene().name == gamifiedScene))
            {
                Destroy(other.gameObject);
            }
            
            coinNumber += 1;
            totalCoins += 1;
            coinText.text = ("Coins: " + coinNumber);
        }
    }    
}
