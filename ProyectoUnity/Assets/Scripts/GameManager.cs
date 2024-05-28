using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    // Referencias a los AudioSources
    [SerializeField] public AudioSource introAudioSource;
    [SerializeField] public AudioSource sirenaAudioSource;
    [SerializeField] public AudioSource pacmanComiendoAudioSource;
    [SerializeField] public AudioSource sirenaBlueAudioSource;
    [SerializeField] public AudioSource muertePacman;
    [SerializeField] public AudioSource muerteGhost;

    // Referencia al PacMan
    public PacMan pacman;
    private int isDead = 0;
    private void Start()
    {
        introAudioSource.Play();
    }

    private void Update()
    {
        sonarSonidos();
        comoprobarTodosLosPanes();
    }

    // Método para verificar si un AudioSource específico está reproduciendo un clip
    public bool IsPlayingClip()
    {
        return introAudioSource.isPlaying;
    }

    public void sonarSonidos()
    {
        if (!IsPlayingClip() && !sirenaAudioSource.isPlaying && !sirenaBlueAudioSource.isPlaying && !pacman.PacmanAnimator().GetBool("isDead"))
        {
            sirenaAudioSource.Play();
        }

        if (pacman.PacmanAnimator().GetBool("isEat") && !pacmanComiendoAudioSource.isPlaying)
        {
            pacmanComiendoAudioSource.Play();
        }else if (!pacman.PacmanAnimator().GetBool("isEat"))
        {
            pacmanComiendoAudioSource.Stop();
        }

        if ((pacman.animatorBlinky.GetBool("isBlue") || pacman.animatorClyde.GetBool("isBlue") || pacman.animatorInky.GetBool("isBlue") || pacman.animatorPinky.GetBool("isBlue"))
            && !sirenaBlueAudioSource.isPlaying)
        {
            sirenaBlueAudioSource.Play();
            sirenaAudioSource.Stop();
        }else if (!pacman.animatorBlinky.GetBool("isBlue") && !pacman.animatorClyde.GetBool("isBlue") && !pacman.animatorInky.GetBool("isBlue") && !pacman.animatorPinky.GetBool("isBlue"))
        {
            sirenaBlueAudioSource.Stop();
        }
        if (pacman.PacmanAnimator().GetBool("isDead") && !muertePacman.isPlaying)
        {
            if (isDead == 0)
            {
                isDead++;
                StartCoroutine(PlayMuertePacman());
            }
            sirenaBlueAudioSource.Stop();
            pacmanComiendoAudioSource.Stop();
            sirenaAudioSource.Stop();
        }
    }
    private IEnumerator PlayMuertePacman()
    {
        muertePacman.Play();
        yield return new WaitWhile(() => muertePacman.isPlaying);
        // Aquí puedes realizar acciones después de que el audio haya terminado
        Debug.Log("MuertePacman audio finished");
        // Detener el audio (aunque ya debería estar detenido)
        muertePacman.Stop();
    }

    //Comprobacion si estan todos los panes
    public void comoprobarTodosLosPanes()
    {
        GameObject panSimple = GameObject.FindWithTag("PanSimple");
        GameObject panRicolino = GameObject.FindWithTag("PanRicolino");

        if (panSimple == null && panRicolino == null) 
        {
            SceneManager.LoadScene(3);
        }
    }

}
