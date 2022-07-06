using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    public void ToggleAudio()
    {
        AudioListener.pause = !AudioListener.pause;
    }

}
