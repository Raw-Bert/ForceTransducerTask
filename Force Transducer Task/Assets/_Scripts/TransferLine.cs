using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferLine : MonoBehaviour
{
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(player.transform.position.x, 0,0);
    }
}
