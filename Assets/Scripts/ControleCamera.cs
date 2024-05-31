using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleCamera : MonoBehaviour
{
    public float limiteGauche;
    public float limiteDroite;
    public float limiteHaut;
    public float limiteBas;

    public GameObject cibleASuivre;

    void Update()
    {
        Vector3 camPosition = transform.position;
        // suivi horizontal à l'intérieur des limites
        if (cibleASuivre.transform.position.x >= limiteGauche && cibleASuivre.transform.position.x <= limiteDroite)
        {
            camPosition.x = cibleASuivre.transform.position.x;
        }
        // suivi vertical à l'intérieur des limites
        if (cibleASuivre.transform.position.y >= limiteBas && cibleASuivre.transform.position.y <= limiteHaut)
        {
            camPosition.y = cibleASuivre.transform.position.y;
        }
        transform.position = camPosition;
    }
}
