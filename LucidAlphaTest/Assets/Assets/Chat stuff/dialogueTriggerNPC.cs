using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dialogueTriggerNPC : MonoBehaviour
{
    public dialogue Dialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<dialogueManager>().StartDialogue(Dialogue);
    }
}
