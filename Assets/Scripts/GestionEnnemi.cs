using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionEnnemi : MonoBehaviour
{
    public float limiteGauche;
    public float limiteDroite;
    float vitesse;

    void Start()
    {
        // Saute à interval aléatoire
        InvokeRepeating("Saut", 2f, Random.Range(2f, 6f));
    }

    void Update()
    {
        // Lorsque l'ennemi arrive à une limite, il se retourne et va dans la direction opposée
        if (GetComponent<SpriteRenderer>().flipX)
        {
            vitesse = 150;
            if (transform.position.x < limiteGauche)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        if (!GetComponent<SpriteRenderer>().flipX)
        {
            vitesse = -150;
            if (transform.position.x > limiteDroite)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        // Application de la vitesse de rotation
        GetComponent<Rigidbody2D>().angularVelocity = vitesse;
    }

    // Saut de l'ennemi
    void Saut()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 16f);
    }
}
