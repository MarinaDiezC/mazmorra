using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SistemaDialogo : MonoBehaviour
{
    private DialogoSO dialogoActual;
    [SerializeField] private GameManagerSO gM;
    [SerializeField] private GameObject cuadroDialogo;
    [SerializeField] private TMP_Text textoDialogo;

    private int indiceFrase;
    private bool escribiendo;

    private void OnEnable()
    {
        gM.OnIniciarDialogo += LanzarDialogo;
    }

    private void LanzarDialogo(DialogoSO dialogoALanzar)
    {
        if(dialogoActual== null) //Comienzo de un dialogo nuevo
        {
            dialogoActual = dialogoALanzar;
            cuadroDialogo.SetActive(true);
            StartCoroutine(EscribirFrase());
        }
        else if(escribiendo)
        {
            CompletarFrase();
        }
        else
        {
            SiguienteFrase();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    //Completa la frase de golpe.
    private void CompletarFrase()
    {
        StopAllCoroutines();
        textoDialogo.text = dialogoActual.Frases[indiceFrase];
        //Activar un icono de continuar (gameObject)
        escribiendo = false;
    }
    //Saca una nueva frase.
    private void SiguienteFrase()
    {
        //Quitar icono de continuar
        indiceFrase++;
        if (indiceFrase >= dialogoActual.Frases.Length)
        {
            TerminarDialogo();
        }
        else
        {
            StartCoroutine(EscribirFrase());
        }
    }

    private IEnumerator EscribirFrase()
    {
        escribiendo = true;
        
        textoDialogo.text = string.Empty;

        //Obtener una frase y trocearla en caracteres.
        char[] fraseTroceada = dialogoActual.Frases[indiceFrase].ToCharArray();
        foreach (char item in fraseTroceada)
        {
            //Escribir en el texto.
            textoDialogo.text += item;
            //Hacer sonidito.
            //Hacer una pequeña espera.
            yield return new WaitForSeconds(dialogoActual.VelocidadDialogo);
        }

        escribiendo = false;
    }

    private void TerminarDialogo()
    {
        dialogoActual = null;
        indiceFrase = 0;
        textoDialogo.text = string.Empty;
        cuadroDialogo.SetActive(false);

        //NPC vuelve a activarte.
        gM.FinInteraccion();
    }
}

