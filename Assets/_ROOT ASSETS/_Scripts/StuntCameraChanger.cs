using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuntCameraChanger : MonoBehaviour
{
    public GameObject cameraActive;
    public GameObject cameraDisable;
    public GameObject[] triggerDisable;
    public float cameraDelay;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            cameraActive.SetActive(true);
            cameraDisable.SetActive(false);
            for (int i = 0; i < triggerDisable.Length; i++) 
            {
                if (triggerDisable[i])
                {
                    triggerDisable[i].SetActive(false);
                }
            }
                               
            Time.timeScale = 0.4f;
            StartCoroutine(CameraSwitchDelay(cameraDelay));
        }
    }

    public IEnumerator CameraSwitchDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 1;
        cameraActive.SetActive(false);
        cameraDisable.SetActive(true);
        for (int i = 0; i < triggerDisable.Length; i++) 
        {
            if (triggerDisable[i])
            {
                triggerDisable[i].SetActive(true);
            }
        }
    }
}
