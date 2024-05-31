using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocAudio : MonoBehaviour
{
    public AudioClip noteBlues1;
    public AudioClip noteBlues2;
    public AudioClip noteBlues3;
    public AudioClip noteBlues4;
    public AudioClip noteBlues5;
    public AudioClip noteBlues6;
    public AudioClip noteBlues7;

    int numPiste;
    int numMesure;

    bool pisteJoue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Bal")
        {
            // Activer l'animation du bloc
            GetComponent<Animator>().enabled = true;

            // Gestion sonore
            if(!pisteJoue)
            {
                numPiste = 1;
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

                    JouerNoteBlues1();
                    Invoke("JouerNoteBlues2", 0.1f);
                    Invoke("JouerNoteBlues3", 0.3f);
                }
                else if (numMesure == 1)
                {
                    numMesure++;

                    JouerNoteBlues7();
                    Invoke("JouerNoteBlues6", 0.2f);
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
                    Invoke("JouerNoteBlues3", 0.2f);
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
        }
    }

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
}
