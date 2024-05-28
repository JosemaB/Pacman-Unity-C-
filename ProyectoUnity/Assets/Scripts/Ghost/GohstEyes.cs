using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEyes : MonoBehaviour
{

    //Eyes movimiento
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;

    //Sprite blue
    public Animator animatorBlue;

    private SpriteRenderer spriteRenderer;
    private MovimientoGhost movement;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponentInParent<MovimientoGhost>(); // Suponiendo que 'MovimientoGhost' está en el padre del objeto de 'GhostEyes'
    }

    private void Update()
    {
        if (!animatorBlue.GetBool("isBlue"))
        {
            // Cambia el orden en la capa a 3
            spriteRenderer.sortingOrder = 3;
            CalcularEyesDireccion();
        }
        else
        {
            // Cambia el orden en la capa a 0
            spriteRenderer.sortingOrder = 0;
        }
       
    }
    private void CalcularEyesDireccion()
    {

        if (movement != null)
        {
            Vector3 direction = movement.direction;

            // Comprueba la dirección y actualiza el sprite en consecuencia
            if (Vector3.Dot(direction, Vector3.up) > 0.5f)
            {
                spriteRenderer.sprite = up;
            }
            else if (Vector3.Dot(direction, Vector3.down) > 0.5f)
            {
                spriteRenderer.sprite = down;
            }
            else if (Vector3.Dot(direction, Vector3.left) > 0.5f)
            {
                spriteRenderer.sprite = left;
            }
            else if (Vector3.Dot(direction, Vector3.right) > 0.5f)
            {
                spriteRenderer.sprite = right;
            }
        }
    }
   
}


