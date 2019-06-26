using System.Collections;
using System.Collections.Generic;
using Game.GameScenes;
using UnityEngine;
using UnityEngine.UI;

public class PanelText : Panel
{
    [SerializeField] private Text thingName;

    public override void Show(Thing thing)
    {
        base.Show(thing);
        thingName.text = thing.thingName;
    }
}
