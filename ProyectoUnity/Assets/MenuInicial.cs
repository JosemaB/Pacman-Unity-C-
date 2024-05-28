using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    bool datosBorrados = false;
   public void Jugar()
    {
        //Para el lugar de nuestra escena mas uno
        SceneManager.LoadScene(1);
    }
    public void MenuPrincipal()
    {
        SceneManager.LoadScene(0);
    }
    public void Salir()
    {
        Debug.Log("Salir");
        Application.Quit();
    }
    public void borrarRecord()
    {
        datosBorrados = true;
        PlayerPrefs.DeleteKey("PuntajeRecord");
    }

    public bool datosBorradosRecord()
    {
        return datosBorrados;
    }
}
