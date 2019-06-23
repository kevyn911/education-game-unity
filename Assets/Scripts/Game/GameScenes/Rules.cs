using System;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GameScenes
{
    [CreateAssetMenu]
    public class Rules : ScriptableObject
    {
        [SerializeField] private Sprite room;
        [SerializeField] private uint timer;
        [SerializeField] private GameObject panelImage;
        [SerializeField] private GameObject panelText;
        [SerializeField] private Thing[] things;

        public Sprite Room => room;
        public uint Timer => timer;
        public GameObject PanelImage => panelImage;

        public GameObject PanelText => panelText;
        public Thing[] Things => things;
    }
    
    [Serializable]
    public class Thing
    {
        public string thingName;
        public bool correct;
        public Sprite sprite;
    }
}
