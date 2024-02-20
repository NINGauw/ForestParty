using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public void SetVolume(Slider s){
        AudioListener.volume = s.value;
    }
}
