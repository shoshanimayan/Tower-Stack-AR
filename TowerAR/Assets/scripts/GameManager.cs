using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameObject currentCube;
    public GameObject lastCube;
    public Text t;
    public int level;
    public bool done;

    private void Start()
    {
        done = false;
        NewBlock(0,0);
    }
    public void NewBlock(float x, float z) {
        if (lastCube != null) {
            currentCube.transform.position = new Vector3(Mathf.Round(currentCube.transform.position.x), currentCube.transform.position.y, Mathf.Round(currentCube.transform.position.z));
            
            currentCube.transform.localScale = new Vector3((lastCube.transform.localScale.x-x),lastCube.transform.localScale.y, (lastCube.transform.localScale.z - z));
            currentCube.transform.position = Vector3.Lerp(currentCube.transform.position,lastCube.transform.position,0.005f)+Vector3.up*.005f;
            if (currentCube.transform.localScale.x <= 0f || currentCube.transform.localScale.z <= 0f) {
                done = true;
                t.gameObject.SetActive(true);
                t.text = "Score: " + level;
                return;

            }
        }
        lastCube = currentCube;
        currentCube = Instantiate(lastCube);
        currentCube.GetComponent<MeshRenderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        level++;
    

    }
    void Update()
    {
        float xx=Mathf.Abs(currentCube.transform.position.x);
        float zz=(Mathf.Abs(currentCube.transform.position.z));
        if (done)
            return;
        float time = Mathf.Abs(Time.realtimeSinceStartup%2f-1f);
        Vector3 pos1 = lastCube.transform.position + Vector3.up * .1f;
        Vector3 pos2 = pos1 + (level%2==0 ? Vector3.forward: Vector3.left)*1.20f;
        if (level % 2 == 0)
            currentCube.transform.position = Vector3.Lerp(pos2, pos1, time);
        else
            currentCube.transform.position = Vector3.Lerp(pos1, pos2, time);
        if (Input.GetKeyDown(KeyCode.Space)) {
            NewBlock(xx,zz);
        }
    }
}
