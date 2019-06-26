using System.Collections;
using System.Collections.Generic;
using Game;
using Game.GameScenes;
using UnityEngine;

public abstract class Panel : MonoBehaviour
{
    public Thing Thing { get; private set; }

    public virtual void Show(Thing thing)
    {
        Thing = thing;
    }
}
