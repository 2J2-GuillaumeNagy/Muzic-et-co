using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GestionBal : MonoBehaviour
{
    float vitesseRotation;
    float vitesseX;
    float vitesseXMax = 4f;
    float vitesseY;

    int numPiste;
    int numMesure;
    int nbBlocsActifs;

    bool sautPossible;
    bool crouchActif;
    bool pisteJoue;

    public AudioClip noteBlues1;
    public AudioClip noteBlues2;
    public AudioClip noteBlues3;
    public AudioClip noteBlues4;
    public AudioClip noteBlues5;
    public AudioClip noteBlues6;
    public AudioClip noteBlues7;

    public TextMeshProUGUI texteMort;

    public GameObject balVictoire;


    // Au départ, enlever la contrainte de mouvement du personnage (Bal) après la transition
    void Start()
    {
        Invoke("ActiverBal", 1f);
    }

    void ActiverBal()
    {
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
    }

    void Update()
    {
        if (!GetComponent<Animator>().GetBool("mort"))
        {
            // Mouvement
            // Vers la droite
            if (Input.GetKey(KeyCode.D))
            {
                vitesseRotation = -500f;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    vitesseX += 0.3f;
                    vitesseXMax = 9f;
                }
                else
                {
                    vitesseX += 0.5f;
                    vitesseXMax = 4f;
                }

                if (Mathf.Abs(vitesseX) >= vitesseXMax)
                {
                    vitesseX = vitesseXMax;
                }

                GetComponent<SpriteRenderer>().flipX = false;
            }
            // Vers la gauche
            else if (Input.GetKey(KeyCode.A))
            {
                vitesseRotation = 500f;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    vitesseX += -0.3f;
                    vitesseXMax = 9f;
                }
                else
                {
                    vitesseX += -0.5f;
                    vitesseXMax = 4f;
                }

                if (Mathf.Abs(vitesseX) >= vitesseXMax)
                {
                    vitesseX = -vitesseXMax;
                }

                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                vitesseRotation = GetComponent<Rigidbody2D>().angularVelocity;
                vitesseX = GetComponent<Rigidbody2D>().velocity.x;
            }
        
            // Animation de course
            if (Input.GetKey(KeyCode.LeftShift))
            {
                GetComponent<Animator>().SetBool("course", true);

                // Priorité à l'animation "crouch"
                if (crouchActif)
                {
                    GetComponent<Animator>().SetBool("course", false);
                }
            }
            else
            {
                GetComponent<Animator>().SetBool("course", false);
            }

            // Saut
            if (Input.GetKeyDown(KeyCode.W) && Physics2D.OverlapCircle(transform.position, 0.45f) && sautPossible)
            {
                vitesseY = 12f;
            }
            else
            {
                vitesseY = GetComponent<Rigidbody2D>().velocity.y;
                if (!Physics2D.OverlapCircle(transform.position, 0.45f))
                {
                    sautPossible = false;
                }
            }

            // Descendre plus vite lorsque S est enfoncé
            if (Input.GetKeyDown(KeyCode.S))
            {
                vitesseY -= 5f;
                if (vitesseY <= -20f)
                {
                    vitesseY = -20f;
                }
            }
        
            // Animation du mouvement de baisse (crouch) lorsque S est appuyé au sol
            if (Input.GetKey(KeyCode.S) && Physics2D.OverlapCircle(transform.position, 0.45f))
            {
                // Arrêter tout mouvement
                vitesseX = 0;
                vitesseRotation = 0;
                transform.Rotate(new Vector3(0f, 0f, -transform.rotation.eulerAngles.z));

                // Activer l'animation "crouch"
                if (!crouchActif)
                {
                    ActivationCrouch();
                }
            }
            // Fin de l'animation "crouch" lorsque S est relâché
            if (Input.GetKeyUp(KeyCode.S))
            {
                GetComponent<Animator>().SetBool("crouchFin", true);
                Invoke("GestionCrouch", 0.25f);
            }
        }
        else
        {
            // Comportement de Bal lorsque mort
            vitesseRotation = GetComponent<Rigidbody2D>().angularVelocity;
            vitesseX = GetComponent<Rigidbody2D>().velocity.x;
            vitesseY = GetComponent<Rigidbody2D>().velocity.y;

            GetComponent<Animator>().SetBool("course", false);
        }

        // Appliquer les mouvement du personnage
        GetComponent<Rigidbody2D>().angularVelocity = vitesseRotation;

        GetComponent<Rigidbody2D>().velocity = new Vector2(vitesseX, vitesseY);

        // Le niveau recommence lorsque R est enfoncé
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Niveau1_Muz");
        }
    }

    // Activation de l'animation du mouvement de baisse (crouch)
    void ActivationCrouch()
    {
        crouchActif = true;
        GetComponent<Animator>().SetBool("crouchDebut", true);
        Invoke("GestionCrouch", 0.25f);
    }

    // Gestion du mouvement de baisse (crouch)
    void GestionCrouch()
    {
        if (GetComponent<Animator>().GetBool("crouchDebut"))
        {
            GetComponent<Animator>().SetBool("crouchDebut", false);
        }
        else
        {
            crouchActif = false;
            GetComponent<Animator>().SetBool("crouchFin", false);
        }
    }

    // Gestion de collisions de type Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Lorsque que Bal tombe trop bas, il est mort
        if (collision.gameObject.name == "Chute")
        {
            if (!GetComponent<Animator>().GetBool("mort"))
            {
                // Faire jouer le son de mort
                SonMort();
            }

            // Activer l'animation de mort
            GetComponent<Animator>().SetBool("mort", true);

            // Empêcher tout mouvement
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

            // Faire apparaître le texte pour recommencer
            Invoke("TexteMortApparition", 2f);
        }

        // Gestion de la fin au contact de l'énergie colorée
        if (collision.gameObject.name == "EnergieCouleur")
        {
            // L'énergie disparait
            collision.gameObject.GetComponent<Animator>().SetBool("fin", true);

            // L'animation de victoire de Bal s'active
            balVictoire.SetActive(true);
            GetComponent<Animator>().SetBool("gagne", true);

            // Après un moment, charger le menu
            Invoke("RetourMenu", 6.6f);

            // Empêcher Bal de bouger
            transform.position = new Vector2(63.2f, -1.36f);
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    // Gestion de collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "surfaceSaut")
        {
            sautPossible = true;
        }

        // Faire tomber la plateforme tombante au touché
        if (collision.gameObject.name == "PlateformeTombante")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }

        // Collision avec un ennemi
        if (collision.gameObject.tag == "ennemi" && !GetComponent<Animator>().GetBool("mort"))
        {
            // Activer l'animation de mort
            GetComponent<Animator>().SetBool("mort", true);

            // Faire jouer le son de mort
            SonMort();

            // Faire apparaître le texte pour recommencer
            Invoke("TexteMortApparition", 2f);
        }

        // Collision avec la porte
        if (collision.gameObject.name == "PorteAudio")
        {
            // La porte s'ouvre si tous les blocs ont été activés
            if (nbBlocsActifs == 3)
            {
                collision.gameObject.GetComponent <Animator>().SetBool("ouverte", true);
                collision.gameObject.GetComponent<Animator>().SetBool("fermee", false);
            }
            // La porte reste fermée s'il manque un bloc à activer
            else if (nbBlocsActifs <= 2)
            {
                collision.gameObject.GetComponent <Animator>().SetBool("fermee", true);
            }
        }

        if (collision.gameObject.name == "BlocAudio")
        {
            // Incrémenter la variable du nombre de blocs actifs à la première collision
            if (!collision.gameObject.GetComponent<Animator>().enabled)
            {
                nbBlocsActifs += 1;
            }

            // Activer l'animation du bloc
            collision.gameObject.GetComponent<Animator>().enabled = true;

            // Gestion sonore
            if (!pisteJoue)
            {
                // Piste choisie aléatoirement
                float nbAlea = Random.Range(0f, 1f);
                if (nbAlea < 0.85f)
                {
                    numPiste = Random.Range(1, 3);
                }
                else
                {
                    numPiste = 3;
                }
            }

            // Sélection de piste
            // Piste #1
            if (numPiste == 1)
            {
                // Sélection de la mesure
                if (numMesure == 0)
                {
                    pisteJoue = true;
                    numMesure++;

                    JouerNoteBlues3();
                    Invoke("JouerNoteBlues2", 0.1f);
                    Invoke("JouerNoteBlues3", 0.2f);
                }
                else if (numMesure == 1)
                {
                    numMesure++;

                    JouerNoteBlues7();
                    Invoke("JouerNoteBlues6", 0.15f);
                }
                else if (numMesure == 2)
                {
                    numMesure++;

                    JouerNoteBlues5();
                }
                else if (numMesure == 3)
                {
                    numMesure++;

                    JouerNoteBlues6();
                    Invoke("JouerNoteBlues5", 0.2f);
                }
                else if (numMesure == 4)
                {
                    numMesure++;

                    JouerNoteBlues2();
                    Invoke("JouerNoteBlues3", 0.15f);
                }
                else if (numMesure == 5)
                {
                    numMesure++;

                    JouerNoteBlues2();
                    Invoke("JouerNoteBlues3", 0.05f);
                    Invoke("JouerNoteBlues4", 0.1f);
                    Invoke("JouerNoteBlues3", 0.3f);
                }
                else if (numMesure == 6)
                {
                    numMesure++;

                    JouerNoteBlues2();
                }
                else if (numMesure == 7)
                {
                    pisteJoue = false;
                    numMesure = 0;

                    JouerNoteBlues1();
                }
            }
            // Piste #2
            else if (numPiste == 2)
            {
                // Sélection de la mesure
                if (numMesure == 0)
                {
                    pisteJoue = true;
                    numMesure++;

                    JouerNoteBlues3();
                    Invoke("JouerNoteBlues4", 0.05f);
                    Invoke("JouerNoteBlues5", 0.1f);
                }
                else if (numMesure == 1)
                {
                    numMesure++;

                    JouerNoteBlues3();
                }
                else if (numMesure == 2)
                {
                    numMesure++;

                    JouerNoteBlues2();
                }
                else if (numMesure == 3)
                {
                    numMesure++;

                    JouerNoteBlues1();
                }
                else if (numMesure == 4)
                {
                    numMesure++;

                    JouerNoteBlues5();
                    Invoke("JouerNoteBlues6", 0.05f);
                    Invoke("JouerNoteBlues7", 0.1f);
                }
                else if (numMesure == 5)
                {
                    numMesure++;

                    JouerNoteBlues5();
                }
                else if (numMesure == 6)
                {
                    numMesure++;

                    JouerNoteBlues3();
                }
                else if (numMesure == 7)
                {
                    numMesure++;

                    JouerNoteBlues2();
                }
                else if (numMesure == 8)
                {
                    numMesure++;

                    JouerNoteBlues4();
                    Invoke("JouerNoteBlues5", 0.05f);
                    Invoke("JouerNoteBlues6", 0.1f);
                }
                else if (numMesure == 9)
                {
                    numMesure++;

                    JouerNoteBlues5();
                }
                else if (numMesure == 10)
                {
                    numMesure++;

                    JouerNoteBlues6();
                }
                else if (numMesure == 11)
                {
                    numMesure++;

                    JouerNoteBlues7();
                }
                else if (numMesure == 12)
                {
                    numMesure++;

                    JouerNoteBlues2();
                    Invoke("JouerNoteBlues3", 0.05f);
                    Invoke("JouerNoteBlues4", 0.1f);
                }
                else if (numMesure == 13)
                {
                    numMesure++;

                    JouerNoteBlues3();
                }
                else if (numMesure == 14)
                {
                    numMesure++;

                    JouerNoteBlues2();
                }
                else if (numMesure == 15)
                {
                    pisteJoue = false;
                    numMesure = 0;

                    JouerNoteBlues1();
                }
            }
            // Piste #3
            else if (numPiste == 3)
            {
                // Sélection de la mesure
                if (numMesure == 0)
                {
                    pisteJoue = true;
                    numMesure++;

                    JouerNoteBlues1();
                    Invoke("JouerNoteBlues1", 0.15f);
                    Invoke("JouerNoteBlues7", 0.3f);
                }
                else if (numMesure == 1)
                {
                    numMesure++;

                    JouerNoteBlues5();
                }
                else if (numMesure == 2)
                {
                    numMesure++;

                    JouerNoteBlues4();
                    Invoke("JouerNoteBlues3", 0.3f);
                    Invoke("JouerNoteBlues2", 0.6f);
                }
                else if (numMesure == 3)
                {
                    pisteJoue = false;
                    numMesure = 0;

                    JouerNoteBlues1();
                    Invoke("JouerNoteBlues2", 0.15f);
                    Invoke("JouerNoteBlues3", 0.3f);
                }
            }
        }
    }

    // Notes à faire jouer
    void JouerNoteBlues1()
    {
        GetComponent<AudioSource>().PlayOneShot(noteBlues1);
    }
    void JouerNoteBlues2()
    {
        GetComponent<AudioSource>().PlayOneShot(noteBlues2);
    }
    void JouerNoteBlues3()
    {
        GetComponent<AudioSource>().PlayOneShot(noteBlues3);
    }
    void JouerNoteBlues4()
    {
        GetComponent<AudioSource>().PlayOneShot(noteBlues4);
    }
    void JouerNoteBlues5()
    {
        GetComponent<AudioSource>().PlayOneShot(noteBlues5);
    }
    void JouerNoteBlues6()
    {
        GetComponent<AudioSource>().PlayOneShot(noteBlues6);
    }
    void JouerNoteBlues7()
    {
        GetComponent<AudioSource>().PlayOneShot(noteBlues7);
    }

    // Son jouant lorsque Bal est mort
    void SonMort()
    {
        JouerNoteBlues7();
        Invoke("JouerNoteBlues4", 0.2f);
        Invoke("JouerNoteBlues3", 0.3f);
        Invoke("JouerNoteBlues1", 0.55f);
    }

    // Apparition du texte indiquant comment recommencer
    void TexteMortApparition()
    {
        texteMort.GetComponent<Animator>().enabled = true;
    }

    // Retour au menu après une victoire
    void RetourMenu()
    {
        SceneManager.LoadScene("Introduction_Muz");
    }
}
