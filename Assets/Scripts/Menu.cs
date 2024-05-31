using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject titre;
    public GameObject transition;
    public GameObject texteControle;
    public GameObject texteCommence;
    bool boutonAppuye;


    void Update()
    {
        // Commencer le jeu lorsque la barre d'espace est appuyée
        if (Input.GetKeyDown(KeyCode.Space) && !boutonAppuye)
        {
            boutonAppuye = true;

            titre.GetComponent<Animator>().enabled = true;
            transition.GetComponent<Animator>().enabled = true;
            texteControle.GetComponent<Animator>().SetBool("disparait", true);
            texteCommence.GetComponent<Animator>().SetBool("commence", true);

            Invoke("commencerJeu", 1f);
        }

        // Aller voir les instructions en appuyant Tab
        if (Input.GetKeyDown(KeyCode.Tab) && !boutonAppuye)
        {
            boutonAppuye = true;

            titre.GetComponent<Animator>().enabled = true;
            transition.GetComponent<Animator>().enabled = true;
            texteControle.GetComponent<Animator>().SetBool("disparait", true);
            texteCommence.SetActive(false);

            Invoke("voirInstructions", 1f);
        }
    }

    // Charger le premier niveau
    void commencerJeu()
    {
        SceneManager.LoadScene("Niveau1_Muz");
    }

    // Charger les instructions
    void voirInstructions()
    {
        SceneManager.LoadScene("Instructions_Muz");
    }
}
