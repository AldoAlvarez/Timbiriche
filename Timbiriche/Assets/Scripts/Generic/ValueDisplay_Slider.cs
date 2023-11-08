using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Generic.UI
{
    public class ValueDisplay_Slider : MonoBehaviour
    {
        [SerializeField] private TMP_Text display;
        [SerializeField] private Slider slider;

        public void UpdateValue() {
            if (slider == null || display == null) return;

            display.text = slider.value.ToString(); 
        }
    }
}