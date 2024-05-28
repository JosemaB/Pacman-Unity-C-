using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovimientoGhost : MonoBehaviour
{
    public Transform pacMan;
    private float speed;
    private List<Vector3> lastPacManWaypoints = new List<Vector3>();

    // Enumeración para definir las acciones posibles
    private enum Action { Home, Walk }

    // Variable para almacenar la acción actual
    private Action currentAction;

    // Transform del objetivo actual y del último objetivo
    public Transform target;
    public Transform lastTarget;
    public Vector3 direction { get; private set; }

    // Dirección de movimiento actual del fantasma
    private Vector3 currentDirection;

    public Animator animator; // Asigna el Animator específico de este fantasma en el Inspector

    //si matan al fantasma lo manda a la base
    public GameObject portalSalida;

    //Para saber si el fantasma puede salir de la celda
    bool canLeaveGhost = true;

    //Puntaje al comer fantasma
    public Puntaje puntaje;

    //Para el audio
    public GameManager gameManager;
    // Método llamado al inicio del juego
    private void Start()
    {
        Waypoints waypointsScript = FindObjectOfType<Waypoints>();
        if (waypointsScript != null)
        {
            lastPacManWaypoints = waypointsScript.GetLastWaypoints();
        }

        // Inicializa la acción actual como 'Walk' al inicio
        currentAction = Action.Walk;
    }

    // Método llamado en cada frame
    private void Update()
    {
        if (!gameManager.IsPlayingClip())
        {
            Velocity();
            HandleWalk();
            TargetDirecion();
        } 
    }

    // Método para obtener la dirección de movimiento actual
    public Vector3 GetDirection()
    {
        return currentDirection;
    }

    //Para actualizar los movimientos de los ojos
    public void TargetDirecion()
    {
        if (target != null)
        {

            // Aquí se asume que tu lógica de movimiento actualiza la dirección del movimiento.
            Vector3 targetDirection = target.position - transform.position;
            if (targetDirection.magnitude > 0.1f)
            {
                direction = targetDirection.normalized;
            }
        }
    }
    // Método para manejar la acción 'Walk'
    private void HandleWalk()
    {
        // Obtiene los puntos cercanos del objetivo
        Transform[] points = target.GetComponent<Waypoints>().nearbyPoints;

        if ((CompareTag("Blinky") || CompareTag("Inky") && Puntaje.GetPuntaje() > 40 || CompareTag("Clyde") && Puntaje.GetPuntaje() > 500  || CompareTag("Pinky") && Puntaje.GetPuntaje() > 250) && canLeaveGhost)
        {
            // Comprueba si el objeto está cerca del objetivo
            if (transform.position == target.position)
            {
                // Selecciona un punto aleatorio
                int rand = Random.Range(0, points.Length);

                //Vemos si hay un punto cercano que paso pacman o esta
                bool puntoCercano = false;
                for (int i = 0; i < points.Length && !puntoCercano; i++)
                {
                    // Obtiene la posición del punto cercano
                    Vector3 pointPosition = points[i].position;

                    if (lastPacManWaypoints.Contains(pointPosition))
                    {
                        puntoCercano = true;
                        target = points[rand];
                    }
                }

                if (!puntoCercano)
                {
                    if (points[rand] != lastTarget || target.GetComponent<Waypoints>().myLayer == 3)
                    {
                        // Actualiza el último objetivo
                        lastTarget = target;

                        // Establece el nuevo objetivo
                        target = points[rand];
                    }
                    else
                    {
                        // Si el punto aleatorio es el mismo que el último, selecciona otro
                        do
                        {
                            rand = Random.Range(0, points.Length);
                        } while (points[rand] == lastTarget);

                        // Actualiza el último objetivo
                        lastTarget = target;

                        // Establece el nuevo objetivo
                        target = points[rand];
                    }
                }

            }
        }
        else
        {
            int valorMyLayer = 0; 
            if (CompareTag("Inky"))
            {
                valorMyLayer = 1;
            }
            if (transform.position == target.position)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    if (points[i].GetComponent<Waypoints>().myLayer == valorMyLayer)
                    {
                        // Actualiza el último objetivo
                        lastTarget = target;

                        // Establece el nuevo objetivo
                        target = points[i];
                    }
                }
            }
             
        }
        // Mueve el objeto hacia el objetivo
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }


    //Paramos todos los pacman si tocan a pacman
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Pacman") && !animator.GetBool("isBlue"))
        {
            // Detén todos los fantasmas
            MovimientoGhost[] ghosts = FindObjectsOfType<MovimientoGhost>();
            foreach (MovimientoGhost ghost in ghosts)
            {
                ghost.animator.SetBool("isBlue", false);
                ghost.enabled = false;
            }
        }
        else if(collision.CompareTag("Pacman") && animator.GetBool("isBlue"))
        {
            gameManager.muerteGhost.Play();
            StartCoroutine(FreezeGameCoroutine(0.5f));
            DisableGhostMovement(5f);
            puntaje.SumarPuntos(1000);
        }    
    }
    private void Velocity()
    {
        if (animator.GetBool("isBlue"))
        {
            speed = 2.5f;
        }
        else
        {
            speed = 4f;
        }
    }

    // Método para desactivar el movimiento del fantasma y reactivarlo después de una duración específica
    public void DisableGhostMovement(float duration)
    {
        StartCoroutine(DisableAndEnableMovement(duration));
    }

    private IEnumerator DisableAndEnableMovement(float duration)
    {
        // Desactivar el movimiento del fantasma
        canLeaveGhost = false;

        // Esperar el tiempo especificado
        yield return new WaitForSeconds(duration);

        // Reactivar el movimiento del fantasma
        canLeaveGhost = true;
    }

    //Congelar pantalla cuando pacman coma el fantasma
    private IEnumerator FreezeGameCoroutine(float duration)
    {
        animator.SetBool("isDead", true);

        // Esperar un pequeño tiempo para que la animación se vea
        yield return new WaitForSeconds(0.15f);
        // Congelar el juego
        Time.timeScale = 0;
        // Esperar el tiempo especificado
        yield return new WaitForSecondsRealtime(duration);
        animator.SetBool("isDead", false);
        animator.SetBool("isBlue", false);
        // Reanudar el juego
        Time.timeScale = 1;
        transform.position = portalSalida.transform.position;
        target = portalSalida.transform;
    }
}

