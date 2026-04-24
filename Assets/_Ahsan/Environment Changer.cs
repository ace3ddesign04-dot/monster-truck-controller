using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnvironmentChanger : MonoBehaviour
{
    public GameObject env1, env2;

    public Button changeEnv;
    public Button reload;

    private GameObject activeEnv;

    private void Start() {
        activeEnv = env1;
        reload.onClick.AddListener(() => {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
        changeEnv.onClick.AddListener(() => {
            activeEnv.gameObject.SetActive(false);

            if (activeEnv == env1)
                activeEnv = env2;
            else
                activeEnv = env1;

            activeEnv.gameObject.SetActive(true);
        });
    }
}
