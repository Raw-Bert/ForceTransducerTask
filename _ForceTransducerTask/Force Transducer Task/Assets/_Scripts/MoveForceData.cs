using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForceData : MonoBehaviour
{
    public float force;
    public int blocks;
    void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("NoDestroy");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
