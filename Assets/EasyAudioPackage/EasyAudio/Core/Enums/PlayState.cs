namespace EasyAudioSystem.Core
{
    /* This enum holds the options of how a selected audioclip is being played.
     * 'Normal' will only apply the settings form the EasyAudioObj and simplay play the audioclip.
     * 'AsOneshot' will play the clip on top of what is currently being played by the
     * Audiosource. Note that the new settings will also change what was currently played.
     * 'Delayed' will wait X seconds before playing the clip.
     * 'FadeInOut' will also manipulate the Volume level based on an animation curve set
     * in the EasyAudioObj inspector.
     * For more information on how each PlayState modifies the audio and Usage check out the EasyAudio Documentation:
     * 
     */
    /// <summary>
    /// Defines the way the audio will be played.
    /// </summary>
    public enum PlayState
    {
        Normal,
        AsOneshot,
        Delayed,
        FadeInOut
    }
}