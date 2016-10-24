using UnityEngine;
using UnityEngine.UI;

public class sliderStart : MonoBehaviour {

    void OnEnable () {
        GameObject sourceOfAudio = GameObject.Find("music player");
        AudioSource sound = sourceOfAudio.GetComponent<AudioSource>();
        Slider slider = GetComponent<Slider>();
        slider.value = sound.volume;
    }

}
