using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastToLine : MonoBehaviour
{
    public LayerMask LineLayer;
    public float timer;
   //public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer < 20.001f && timer > 19.999f)
        {
            //Debug.Log("TIME AT 20: " + timer + ". Player Tranform at 20: " + this.transform.position.x);
        }
        //else Debug.Log("Time: " + timer);
    }

    void FixedUpdate() {
        Vector2 dir = new Vector2(0,-90);
        Vector2 dirUp = new Vector2(0,90);
        Debug.DrawRay(this.transform.position, dir, Color.green, 20.0f);
       
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, dir, 999f, LineLayer);

        if (hit.collider != null)
        {

            //Debug.Log("DOWN: " + hit.distance);
        }

        else if(hit.collider == null)
        {
            Debug.DrawRay(this.transform.position, dirUp, Color.red, 20.0f);
            RaycastHit2D hitUp = Physics2D.Raycast(this.transform.position, dirUp, 999f, LineLayer);

            if (hitUp.collider != null)
            {
                //Debug.Log("UP: " + hitUp.distance);
            }
            //else Debug.Log(0);
        } 
        
        

        
        
    }
}
