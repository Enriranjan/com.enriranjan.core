namespace EnriRanjan.Core.EngineInterfaces
{
    /// <summary>
    /// Engine-agnostic audio playback surface. Implemented in Unity by an
    /// adapter wrapping an AudioSource, resolving clip ids against a
    /// name-to-clip mapping owned by the adapter.
    /// </summary>
    public interface IAudioPlayer
    {
        /// <summary>Plays the clip registered under <paramref name="clipId"/>.</summary>
        void Play(string clipId);

        /// <summary>Stops playback, if any is in progress.</summary>
        void Stop();

        /// <summary>Playback volume, expected in the 0..1 range.</summary>
        float Volume { get; set; }
    }
}
