using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CorazonesUI : MonoBehaviour
{
    public List<Image> listaCorozanes;
    public GameObject corazonPrefab;
    public VidaPacman vidaJugador;
    public int indexActual;
    public Sprite corazonLleno;
    public Sprite corazonVacio;

    public void Awake()
    {
        vidaJugador.cambioVida.AddListener(CambiarCorazones);
    }

    private void CambiarCorazones(int vidaActual)
    {
        if (!listaCorozanes.Any())
        {
            CrearCorazones(vidaActual);
        }
        else
        {
            CambiarVida(vidaActual);
        }
    }

    private void CambiarVida(int vidaActual)
    {
        if (vidaActual <= indexActual)
        {
            QuitarCorazones(vidaActual);
        }
    }
    public void QuitarCorazones(int vidaActual)
    {
        for (int i = indexActual; i >= vidaActual; i--)
        {
            indexActual = i;
            listaCorozanes[i].sprite = corazonVacio;
        }
    }

    private void CrearCorazones(int cantidadMaximaVida)
    {
        for (int i = 0; i < cantidadMaximaVida; i++)
        {
            GameObject corazon = Instantiate(corazonPrefab, transform);

            listaCorozanes.Add(corazon.GetComponent<Image>());
        }

        indexActual = cantidadMaximaVida - 1;
    }
}
