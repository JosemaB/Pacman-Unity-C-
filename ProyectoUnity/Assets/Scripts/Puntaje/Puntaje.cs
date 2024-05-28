using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;

public class Puntaje : MonoBehaviour
{
    public float puntos;
    public static float puntosActuales;
    private TextMeshProUGUI textMesh;
    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }
    private void Update(){
        textMesh.text = puntos.ToString("0");
        puntosActuales = puntos;
        if (puntos > PlayerPrefs.GetInt("PuntajeRecord", 0))
        {
            //Guardamaos prefab
            PlayerPrefs.SetInt("PuntajeRecord", (int)puntos);
        }
    }
    public void SumarPuntos(float puntosEntrada)
    {
        puntos += puntosEntrada;
    }

    public static float GetPuntaje()
    {
        return puntosActuales;
    }
}
