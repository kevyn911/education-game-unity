using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GameScenes
{
    [CreateAssetMenu]
    public class Rules : ScriptableObject
    {
        [SerializeField] private Sprite room;
        [SerializeField] private uint timer;
        [SerializeField] private Thing[] things;

        public Sprite Room => room;

        public uint Timer => timer;

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
