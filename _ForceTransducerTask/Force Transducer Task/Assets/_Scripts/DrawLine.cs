using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public GameObject line;
    public GameObject currentLine;

    public LineRenderer lineRenderer;

    public List<Vector2> birdPos;

    public GameObject bird;

    public bool birdStart = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (birdStart == true)
        {
            CreateLine();
            //Debug.Log(bird.transform.position);
            birdStart = false;
        }  
        if(birdStart == false)
        {
            Vector2 birdPosrn = bird.transform.position;
            if(Vector2.Distance(birdPosrn, birdPos[birdPos.Count - 1]) > .1f)
            {
                UpdateLine(birdPosrn);
                //Debug.Log(birdPosrn); <-- BirdPoos.y and .x go to spreadsheet.
            }
        }
        
            
    }


    void CreateLine()
    {
        currentLine = Instantiate(line, Vector3.zero, Quaternion.identity);
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        //edgeCollider = currentLine.GetComponent<EdgeCollider2D>();

        birdPos.Clear();
        birdPos.Add(bird.transform.position);
        birdPos.Add(bird.transform.position);

        lineRenderer.SetPosition(0, birdPos[0]);
        lineRenderer.SetPosition(1,birdPos[1]);
        
    }

    void UpdateLine(Vector2 newPoint)
    {
        birdPos.Add(newPoint);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPoint);

    }

}
