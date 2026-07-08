using System;
using System.Collections.Generic;
using EnriRanjan.Core.EngineInterfaces;
using UnityEngine;

namespace EnriRanjan.Core.Unity.Adapters
{
    /// <summary>
    /// <see cref="IAudioPlayer"/> adapter wrapping a Unity AudioSource. Attach
    /// to a GameObject alongside an AudioSource and populate the clip list;
    /// clip ids are resolved against that serialized name-to-clip mapping.
    /// </summary>
    public sealed class AudioPlayerAdapter : MonoBehaviour, IAudioPlayer
    {
        [Serializable]
        private struct ClipEntry
        {
            public string Id;
            public AudioClip Clip;
        }

        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private List<ClipEntry> clips = new List<ClipEntry>();

        private Dictionary<string, AudioClip> _clipsById;

        public void Play(string clipId)
        {
            _clipsById ??= BuildClipLookup();

            if (!_clipsById.TryGetValue(clipId, out AudioClip clip))
            {
                throw new InvalidOperationException($"No audio clip registered under id '{clipId}'.");
            }

            audioSource.clip = clip;
            audioSource.Play();
        }

        public void Stop()
        {
            audioSource.Stop();
        }

        public float Volume
        {
            get => audioSource.volume;
            set => audioSource.volume = value;
        }

        private Dictionary<string, AudioClip> BuildClipLookup()
        {
            var lookup = new Dictionary<string, AudioClip>(clips.Count);
            foreach (ClipEntry entry in clips)
            {
                lookup[entry.Id] = entry.Clip;
            }

            return lookup;
        }
    }
}
