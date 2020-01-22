using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GameManager : MonoBehaviour
{
    public GameObject currentCube;
    public GameObject lastCube;
    public Text t;
    public int level;
    public bool done;
    public bool placed;
    public ARRaycastManager m_RaycastManager;
    private Pose placementPose;

    private void Start()
    {
        placed = false;
        done = false;
       // NewBlock(0,0);
    }

    public void NewBlock(float x, float z) {
        if (lastCube != null) {
            currentCube.transform.position = new Vector3(Mathf.Round(currentCube.transform.position.x), currentCube.transform.position.y, Mathf.Round(currentCube.transform.position.z));
            
            currentCube.transform.localScale = new Vector3((lastCube.transform.localScale.x-x),lastCube.transform.localScale.y, (lastCube.transform.localScale.z - z));
            currentCube.transform.position = Vector3.Lerp(currentCube.transform.position,lastCube.transform.position,0.005f)+Vector3.up*.005f;
            if (currentCube.transform.localScale.x <= 0f || currentCube.transform.localScale.z <= 0f) {
                done = true;
                t.gameObject.SetActive(true);
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
        if (placed)
        {
            t.text = "tap the screen to stop the block ontop of the tower, don't miss!";
            float xx = Mathf.Abs(currentCube.transform.position.x);
            float zz = (Mathf.Abs(currentCube.transform.position.z));
            if (done)
            {
                t.text = "Score: " + level + "\n tap to start again";

                if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0)
                {

                    SceneManager.LoadScene(0);

                }
                return;
            }
            float time = Mathf.Abs(Time.realtimeSinceStartup % 2f - 1f);
            Vector3 pos1 = lastCube.transform.position + Vector3.up * .1f;
            Vector3 pos2 = pos1 + (level % 2 == 0 ? Vector3.forward : Vector3.left) * 1.20f;
            if (level % 2 == 0)
                currentCube.transform.position = Vector3.Lerp(pos2, pos1, time);
            else
                currentCube.transform.position = Vector3.Lerp(pos1, pos2, time);
            if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0)
            {
                NewBlock(xx, zz);
            }
        }
        else {
            t.text = "point the phone to choose where to play, tap the screen to start";
            UpdatePlacementPose();
        }
    }


    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        m_RaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
        
        if (hits.Count > 0)
        {

            placementPose = hits[0].pose;
            var camForward = Camera.main.transform.forward;
            Vector3 cameraBearing = new Vector3(camForward.x, 0, camForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            currentCube.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);

        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0)
        {
            placed = true;
            NewBlock(0, 0);
        }

    }
    
}
