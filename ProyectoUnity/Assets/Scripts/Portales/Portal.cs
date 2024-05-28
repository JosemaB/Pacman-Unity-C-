using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject portalSalida;
    private static bool objetoEntro = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!objetoEntro)
        { 
               collision.transform.position = portalSalida.transform.position;

            if (collision.TryGetComponent(out MovimientoGhost movimiento))
            {
                if (CompareTag("Portal"))
                {
                    // Encuentra el objeto con la etiqueta especificada
                    GameObject targetObject = GameObject.FindWithTag("SalidaPortal1");
                    movimiento.target = targetObject.transform;
                    Debug.Log("El fantasma ha colisionado con Portal 1");
                }
                else if (CompareTag("Portal2"))
                {
                   // Encuentra el objeto con la etiqueta especificada
                        GameObject targetObject = GameObject.FindWithTag("SalidaPortal2");
                    if (targetObject != null)
                    {
                        movimiento.target = targetObject.transform;
                        Debug.Log("El fantasma ha colisionado con Portal 2 y se dirige a SalidaPortal2");
                    }
                    else
                    {
                        Debug.LogWarning("No se encontró ningún objeto con la etiqueta 'SalidaPortal2'");
                    }
                }
            }
            objetoEntro = true;
        }
        else
        {
            objetoEntro = false;
        }

    }
}
