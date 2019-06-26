using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Game.GameScenes
{
    [CreateAssetMenu]
    public class Rules : ScriptableObject
    {
        [SerializeField] private Sprite room;
        [SerializeField] private uint timer;
        [SerializeField] private Panel panelImage;
        [SerializeField] private Panel panelText;
        [SerializeField] private Thing[] things;

        public Sprite Room => room;
        public uint Timer => timer;
        public Panel PanelImage => panelImage;
        public Panel PanelText => panelText;
        public Thing[] Things => things;
    }
    
    [Serializable]
    public class Thing
    {
        public string thingName;
        public bool correct;
        public Sprite sprite;
        private bool selBool;

        public bool SelBool
        {
            get => selBool;
            set => selBool = value;
        }
    }

    public static class CollectionsExtensions
    {
        public static IEnumerable<TSource> Shuffle<TSource>(this IEnumerable<TSource> source)
        {
            var rng = new Random();
            var buffer = source.ToList();
            for (int i = 0; i < buffer.Count; i++)
            {
                var j = rng.Next(i, buffer.Count);
                yield return buffer[j];
                buffer[j] = buffer[i];
            }
        }
    }
}
