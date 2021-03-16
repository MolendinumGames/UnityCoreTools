using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreTools;

public class DialoguePushText : MonoBehaviour
{
    public DialogueChannelSO channel;
    public Dialogue dialogue;

    public void PushDialogue()
    {
        channel.Raise(dialogue);
    }
    private void Start()
    {
        PushDialogue();
    }
}
