using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PacMan : MonoBehaviour
{

    public float velocidad; // Velocidad de movimiento del personaje
    private Vector3 direccion; //Dejamos la posicion 0 por defecto
    public Puntaje puntaje;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isDead = false;
    private bool isGhostBlue = false;
    Coroutine myCoroutineBlinky;
    Coroutine myCoroutineInky;
    Coroutine myCoroutinePinky;
    Coroutine myCoroutineClyde;
    //Sprite blue
    public Animator animatorBlinky;
    public Animator animatorInky;
    public Animator animatorClyde;
    public Animator animatorPinky;

    //para reinciar punto pacman y ghosts
    public GameObject reiniciarPuntoPacman;
    public GameObject restarInky;
    public GameObject restarBlinky;
    public GameObject restarPinky;
    public GameObject restarClyde;

    //Para el audio
    public GameManager gameManager;

    private void Start()
    {
        direccion = Vector3.zero;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if(!gameManager.IsPlayingClip()){

            SpritePersonajeDireccion();
            ProcesamientoMovimiento();
            RestartGame();
        }
        
    }

    void ProcesamientoMovimiento()
    {
        if (!animator.GetBool("isDead"))
        {
            // Detectar la entrada del teclado
            if (Input.GetKeyDown(KeyCode.D))
            {
                direccion = Vector3.right;
                animator.SetBool("isEat", true);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                direccion = Vector3.left;
                animator.SetBool("isEat", true);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                direccion = Vector3.up;
                animator.SetBool("isEat", true);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                direccion = Vector3.down;
                animator.SetBool("isEat", true);
            }

            // Mover el Pacman en la dirección actual
            rb.velocity = direccion * velocidad;
        }
    }

    private void SpritePersonajeDireccion()
    {
        if (!animator.GetBool("isDead"))
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }

            // Mover hacia abajo (-90 grados alrededor del eje Z)
            if (Input.GetKeyDown(KeyCode.S))
            {
                transform.rotation = Quaternion.Euler(0, 0, -90);
            }

            // Mover hacia la izquierda (180 grados alrededor del eje Z)
            if (Input.GetKeyDown(KeyCode.A))
            {
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }

            // Mover hacia la derecha (0 grados alrededor del eje Z)
            if (Input.GetKeyDown(KeyCode.D))
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
           
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Pacman muriendo que no corte la animacion de muerte
        bool esPacman = false;
        
        if ((collision.CompareTag("Pinky") && !animatorPinky.GetBool("isBlue") || collision.CompareTag("Blinky") && !animatorBlinky.GetBool("isBlue") 
            || collision.CompareTag("Inky") && !animatorInky.GetBool("isBlue") || collision.CompareTag("Clyde")) && !animatorClyde.GetBool("isBlue"))
        {            
            direccion = Vector3.zero;
            animator.SetBool("isEat", false);
            rb.velocity = Vector3.zero;
            esPacman = true;
            StartCoroutine(WaitForAnimationAndDestroy(animator, "PacmanMuriendo", "isDead", esPacman));
        }
        else if (collision.CompareTag("PanSimple"))
        {
            puntaje.SumarPuntos(10);
        }
        else if (collision.CompareTag("PanRicolino"))
        {
            puntaje.SumarPuntos(50);

            if (myCoroutineBlinky == null)
            {
                myCoroutineBlinky = StartCoroutine(WaitForAnimationAndDestroy(animatorBlinky, "BlueGhost", "isBlue", esPacman));
                myCoroutineInky = StartCoroutine(WaitForAnimationAndDestroy(animatorInky, "BlueGhost", "isBlue", esPacman));
                myCoroutinePinky = StartCoroutine(WaitForAnimationAndDestroy(animatorPinky, "BlueGhost", "isBlue", esPacman));
                myCoroutineClyde = StartCoroutine(WaitForAnimationAndDestroy(animatorClyde, "BlueGhost", "isBlue", esPacman));
            }
            else
            {
                //Blinky
                StopCoroutine(myCoroutineBlinky);
                animatorBlinky.SetBool("isBlue", false);
                myCoroutineBlinky = StartCoroutine(WaitForAnimationAndDestroy(animatorBlinky, "BlueGhost", "isBlue", esPacman));

                //Inky
                StopCoroutine(myCoroutineInky);
                animatorInky.SetBool("isBlue", false);
                myCoroutineInky = StartCoroutine(WaitForAnimationAndDestroy(animatorInky, "BlueGhost", "isBlue", esPacman));

                //Pinky
                StopCoroutine(myCoroutinePinky);
                animatorPinky.SetBool("isBlue", false);
                myCoroutinePinky = StartCoroutine(WaitForAnimationAndDestroy(animatorPinky, "BlueGhost", "isBlue", esPacman));

                //Clyde
                StopCoroutine(myCoroutineClyde);
                animatorClyde.SetBool("isBlue", false);
                myCoroutineClyde = StartCoroutine(WaitForAnimationAndDestroy(animatorClyde, "BlueGhost", "isBlue", esPacman));
            }   
        }
    }
    //Muerte Pacman
    private IEnumerator WaitForAnimationAndDestroy(Animator animator,string animationName, string nombreBool, bool esPacman)
    {

        //Ponemos a true
        animator.SetBool(nombreBool, true);
        // Espera un frame para asegurarse de que la animación ha tenido tiempo de cambiar
        yield return null;

        // Espera hasta que el estado actual del Animator sea el deseado
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            yield return null;
        }

        // Una vez que la animación ha comenzado, obtén su duración
        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;

        // Espera la duración de la animación
        yield return new WaitForSeconds(animationDuration);
        if (!esPacman)
        {
            animator.SetBool(nombreBool, false);
        }
        isGhostBlue = false;
    }

    // Método para guardar la escena temporal
    private void RestartGame()
    {
        if (animator.GetBool("isDead"))
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // Verificar si la animación actual es "PacmanMuriendo" y si ha terminado de reproducirse
            if (stateInfo.IsName("PacmanMuriendo") && stateInfo.normalizedTime >= 1.0f)
            {
                // Iniciar la corrutina para esperar la animación completa y permitir la tecla
                StartCoroutine(WaitForAnimationToEnd());
            }
        }
    }
    public Animator PacmanAnimator()
    {
        return animator;
    }
    private IEnumerator WaitForAnimationToEnd()
    {
        // Esperar a que la animación termine completamente
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        // Esperar hasta que se presione una tecla
        while (!Input.anyKeyDown)
        {
            yield return null;
        }

        // Reiniciar la posición y estado de Pacman
        animator.SetBool("isDead", false);
        transform.position = reiniciarPuntoPacman.transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);

        //Reiniciar los puntos de los fantasmas
        // Detén todos los fantasmas
        MovimientoGhost[] ghosts = FindObjectsOfType<MovimientoGhost>();
        foreach (MovimientoGhost ghost in ghosts)
        {
            if (ghost.CompareTag("Inky"))
            {
                ghost.transform.position = restarInky.transform.position;
                ghost.target = restarInky.transform;
                ghost.enabled = true;
            }
            if (ghost.CompareTag("Blinky"))
            {
                ghost.transform.position = restarBlinky.transform.position;
                ghost.target = restarBlinky.transform;
                ghost.enabled = true;
            }
            if (ghost.CompareTag("Pinky"))
            {
                ghost.transform.position = restarPinky.transform.position;
                ghost.target = restarPinky.transform;
                ghost.enabled = true;
            }
            if (ghost.CompareTag("Clyde"))
            {
                ghost.transform.position = restarClyde.transform.position;
                ghost.target = restarClyde.transform;
                ghost.enabled = true;
            }
        }

    }
}

