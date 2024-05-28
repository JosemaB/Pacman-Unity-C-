using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoDaño : MonoBehaviour
{
    public int daño;
    public Animator animator;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!animator.GetBool("isBlue"))
        {
            if (other.TryGetComponent(out VidaPacman vidaPacman))
            {
                vidaPacman.TomarDaño(daño);
            }
        }
    }
}
