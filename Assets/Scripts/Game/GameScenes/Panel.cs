using System.Collections;
using System.Collections.Generic;
using Game;
using Game.GameScenes;
using UnityEngine;

public class Panel : MonoBehaviour
{
    private Thing thing;
    
    public Thing Thing
    {
        get => thing;
        set => thing = value;
    }

    public virtual void Show(Thing thing)
    {
        Thing = thing;
    }
}
