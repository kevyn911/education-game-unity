using System.Collections;
using System.Collections.Generic;
using Game.GameScenes;
using UnityEngine;
using UnityEngine.UI;

public class PanelImage : Panel
{
    [SerializeField] private Image image;

    public override void Show(Thing thing) 
    {
        base.Show(thing);
        image.sprite = thing.sprite;
    }
}
