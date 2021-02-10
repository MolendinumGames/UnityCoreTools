namespace EasyAudioSystem.Core
{
    /* This enum holds the different states of which a clip can be selected.
     * 'Random' will select a random non-null clip.
     * 'Next' take into account a counter and play the next clip in line.
     * The counter starts at 0 and resets everytime it reaches the end.
     * It can also be manually reset.
     * 'Specific' will try to play the clip at a specific index in audioClips (EasyAudioObj)
     * 'AllInRow' will start a Coroutine and play each clip after the previous is done. Starts at 0.
     * To stop the Coroutine from playing new clips call EasyAudio.Stop instead of AudioSource.Stop.
     * 'AllInRowShuffled' same as AllInRow but previously shuffles the list.
     * This will not change the order of clips in the EasyAudioObj.
     * 'AllOverlapped' will play all the clips from the EasyAudioObj on top of each other.
     *
     * For more Information on how SelectState works in the EasyAudio workflow check out the EasyAudio Documentation:
     * 
     */
    /// <summary>
    /// Defines the different ways how AudioCLips to play can be selected from an EasyAudioObj.
    /// </summary>
    public enum SelectState
    {
        Random,
        Next,
        Specific,
        AllInRow,
        AllInRowShuffled,
        AllOverlapped
    }
}
