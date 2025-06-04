using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum illusteffects
{
    zoom,vibrate
}
public class DialogueEffects : MonoBehaviour
{
    public DialogueManager dialogueManager;

    void actIllustEffect(illusteffects effect)
    {
        switch (effect)
        {
            case illusteffects.zoom:
                break;
            case illusteffects.vibrate:
                break;
        }
    }
}
