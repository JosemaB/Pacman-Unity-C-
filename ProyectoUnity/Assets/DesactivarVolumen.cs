using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DesactivarVolumen : MonoBehaviour
{
    public Button audioButton;
    public Sprite audioActivadoSprite;
    public Sprite audioDesactivadoSprite;
    public AudioSource audioSource;

    private bool isAudioActivado = true;

    void Start()
    {
        // Asigna el método ToggleAudio como respuesta al evento Pointer Down del botón
        EventTrigger trigger = audioButton.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { ToggleAudio(); });
        trigger.triggers.Add(entry);

        // Asegura que el audio se encuentre activado al inicio
        audioSource.volume = 1.0f;

        // Configura la imagen inicial del botón según el estado del audio
        UpdateAudioButtonImage();
    }

    public void ToggleAudio()
    {
        // Cambia el estado del audio (activado o desactivado)
        isAudioActivado = !isAudioActivado;

        // Cambia la imagen del botón según el estado del audio
        UpdateAudioButtonImage();

        // Ajusta el volumen del audio según el estado
        audioSource.volume = isAudioActivado ? 1.0f : 0.0f;
    }

    void UpdateAudioButtonImage()
    {
        // Cambia la imagen del botón según el estado del audio
        audioButton.image.sprite = isAudioActivado ? audioActivadoSprite : audioDesactivadoSprite;
    }
}
