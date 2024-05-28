using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecordPuntaje : MonoBehaviour
{
    public int puntosRecord;
    private TextMeshProUGUI textMesh;
    public MenuInicial menu;
    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        puntosRecord = PlayerPrefs.GetInt("PuntajeRecord", 0); // Obtener el valor de PlayerPrefs
        textMesh.text = puntosRecord.ToString(); // Convertir puntosRecord a string y asignarlo al textMesh
    }
    private void Update()
    {
        if(menu.datosBorradosRecord()){
            puntosRecord = 0;
            textMesh.text = puntosRecord.ToString();
        }
    }
}
