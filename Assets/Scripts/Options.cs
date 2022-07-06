using UnityEngine;

public class Options : MonoBehaviour
{
    public void ToggleAudio()
    {
        AudioListener.pause = !AudioListener.pause;
    }

    public void CloseOptions()
    {
        gameObject.SetActive(false);
    }
}
