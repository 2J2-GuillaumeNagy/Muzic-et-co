using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Instructions : MonoBehaviour
{
    public GameObject transition;
    public GameObject texteRetour;

    // Revenir au menu principal en appuyant sur Tab ou Escape
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape))
        {
            transition.GetComponent<Animator>().enabled = true;
            texteRetour.GetComponent<Animator>().SetBool("retour", true);

            Invoke("retourMenu", 1f);
        }
    }

    void retourMenu()
    {
        SceneManager.LoadScene(0);
    }
}
