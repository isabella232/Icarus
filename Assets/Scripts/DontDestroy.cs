using System;
using UnityEngine;
using UnityEngine.UI;

public class DontDestroy : MonoBehaviour {

    GameObject sliderObject;

    private static DontDestroy instance = null;
    public static DontDestroy Instance
    {
        get { return (instance);  }
    }

    void Awake () {
        if (instance != null && instance != this)
        {
            Debug.Log(string.Format("new instance attempting to take control. Current clip name: {0} new clip name: {1}", this.GetComponent<AudioSource>().clip.name, instance.GetComponent<AudioSource>().clip.name));
            if (this.GetComponent<AudioSource>().clip.name != instance.GetComponent<AudioSource>().clip.name)
            {
                Debug.Log(string.Format("new song detected. changing song"));
                instance.GetComponent<AudioSource>().clip = this.GetComponent<AudioSource>().clip;
                instance.GetComponent<AudioSource>().Play();
                //Destroy(instance.gameObject);
                //Debug.Log(string.Format("redefining instance as new song"));
                //instance = this;
                Destroy(this.gameObject);
            }
            else
            {
                Debug.Log(string.Format("same song detected. detroying new instance"));
                Destroy(this.gameObject);
                return;
            }
        }
        else
        {
            //Debug.Log(string.Format("no previous song detected, starting new instance"));

            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
	}

    void Update()
    {
        sliderObject = GameObject.Find("Volume Slider");
        try
        {
            Slider slider = sliderObject.GetComponent<Slider>();
            if (slider != null)
            {
                instance.GetComponent<AudioSource>().volume = slider.value;
            }
        }
        catch (NullReferenceException e)
        {
            var a = true;
            if (a)
            {

            }
            //nothing
        }
    }
}
