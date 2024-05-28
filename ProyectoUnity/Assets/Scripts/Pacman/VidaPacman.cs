using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class VidaPacman : MonoBehaviour
{
    public int vidaActual = 2;
    public UnityEvent<int> cambioVida;


    // Start is called before the first frame update
    void Start()
    {
        cambioVida.Invoke(vidaActual);
    }

    public void TomarDaño(int cantidadDaño)
    {
        int vidaTemporal = vidaActual - cantidadDaño;

        if (vidaTemporal < 0)
        {
            vidaActual = 0;
        }
        else
        {
            vidaActual = vidaTemporal;
        }
        cambioVida.Invoke(vidaActual);

        if (vidaActual <= 0)
        {
            SceneManager.LoadScene(2);
        }
    }
    /*METODO GAME OVER AQUI O EN CORAZON UI*/
}
