using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameManager")]
public class GameManagerSO : ScriptableObject
{
    public event Action<DialogoSO> OnIniciarDialogo;
    public event Action OnFinInteraccion;
    public void IniciarDialogo(DialogoSO dialogo)
    {
        //Lanzar un evento a todas aquellas entidades
        //que est�n interesadas en que se necesite iniciar un di�logo.
        OnIniciarDialogo?.Invoke(dialogo);
        //La interrogacion pregunta si alguien est� interesado, y si no hay impide que el codigo se rompa

    }
    public void FinInteraccion()
    {
        OnFinInteraccion?.Invoke();
    }
}
