using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AttachToMusic : MonoBehaviour
{

    private GameObject soundSource;
    private AudioSource soundVolume;

    //private GameObject volumeSlider;
    //private Slider sliderScript;

    void Start () {
        //volumeSlider = GameObject.Find("Volume Slider");
        //sliderScript = volumeSlider.GetComponent<Slider>();

        soundSource = GameObject.Find("music player");
        soundVolume = soundSource.GetComponent<AudioSource>();

        //sliderScript.onValueChanged.AddListener(changeVolume);
    }

    float changeVolume()
    {
        return soundVolume.volume;
    }
}
