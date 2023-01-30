using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

//clase base usada para todo tipo de acciones como mostrar un objeto, disparar una animación etc..
public abstract class GameAction : MonoBehaviour
{
    public abstract void Activated();
}

//Clase Base usada para activar GameAction cuando se realiza una acción específica
public abstract class GameTrigger : MonoBehaviour
{
    public GameAction[] actions;

    public void Trigger()
    {
        foreach (GameAction g in actions)
            g.Activated();
    }
}