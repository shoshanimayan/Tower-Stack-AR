using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCubes : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;
    public static MoveCubes CurrentCube{get; private set;}

    internal void Stop()
    {
        moveSpeed=0;
    }

    // Update is called once per frame
    private void OnEnable() {

        CurrentCube=this;
    }
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * moveSpeed;
           
    }
}
