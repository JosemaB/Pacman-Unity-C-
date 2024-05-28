using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comida : MonoBehaviour
{
    private bool destroyed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pacman"))
        {
            destroyed = true;
            gameObject.SetActive(false); // Desactiva el objeto en lugar de destruirlo
        }
    }
    public bool IsDestroyed()
    {
        return destroyed;
    }
}
