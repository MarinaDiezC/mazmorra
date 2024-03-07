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
        //que estén interesadas en que se necesite iniciar un diálogo.
        OnIniciarDialogo?.Invoke(dialogo);
        //La interrogacion pregunta si alguien está interesado, y si no hay impide que el codigo se rompa

    }
    public void FinInteraccion()
    {
        OnFinInteraccion?.Invoke();
    }
}
