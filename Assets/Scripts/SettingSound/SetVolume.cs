using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

namespace PackageCreator.Sounds
{
    public class SetVolume : MonoBehaviour //Script audiomixer for sound with a slider option
    {
        [HideInInspector]
        public AudioMixer mixer; //Don't forget to make ExposedParameter on AudioMixer by right click on audiomixer group on Volume and expose it !
        [HideInInspector]
        public Slider slider;
        [HideInInspector]
        public Text textObject;
        [HideInInspector]
        public string _nameExposedParameter;

        void Start()
        {
            Init();
        }

        //Function Init to be used by other script for set value of sound 
        public void Init()
        {
            //value save data
            slider.value = PlayerPrefs.GetFloat(_nameExposedParameter, 1f);
            UpdateTextObject();
        }

        //Function to change audiomixer volume with value changed of slider
        public void SetLevel()
        {
            float sliderValue = slider.value;
            //Mixer float form slider value 
            mixer.SetFloat(_nameExposedParameter, Mathf.Log10(sliderValue) * 20);
            //Save Data
            PlayerPrefs.SetFloat(_nameExposedParameter, sliderValue);
            UpdateTextObject();
        }

        //Change text for percent
        public void UpdateTextObject()
        {
            //percent of slider value
            textObject.text = Mathf.Round(slider.value * 100) + "%";
        }

        // ------------------------------------------------------------------------------------------------------------------------------------------- //
    }
}
