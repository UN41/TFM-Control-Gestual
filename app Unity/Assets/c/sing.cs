using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sing : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;

    private void Start()
    {

    }

    private void Update()
    {
        // Obtener el valor del parámetro "sing" del animator
        int singLevel = animator.GetInteger("sing");

        // Realizar acciones en función del valor de "sing"
        if (singLevel == 0)
        {
            // Detener la reproducción del audio
            audioSource.Stop();
        }
         else if (singLevel == 3)
        {
            // Hacer algo cuando "sing" es 1 (más agudo)
            audioSource.pitch = 1f; // Ajustar el tono del audio
            if (!audioSource.isPlaying)
            {
                audioSource.Play(); // Iniciar la reproducción del audio si no se está reproduciendo
            }
        }
        else if (singLevel == 2)
        {
            // Hacer algo cuando "sing" es 1 (más agudo)
            audioSource.pitch = 1.2f; // Ajustar el tono del audio
            if (!audioSource.isPlaying)
            {
                audioSource.Play(); // Iniciar la reproducción del audio si no se está reproduciendo
            }
        }
        else if (singLevel == 1)
        {
            // Hacer algo cuando "sing" es 2 (más grave)
            audioSource.pitch = 0.8f; // Ajustar el tono del audio
            if (!audioSource.isPlaying)
            {
                audioSource.Play(); // Iniciar la reproducción del audio si no se está reproduciendo
            }
        }
    }
}