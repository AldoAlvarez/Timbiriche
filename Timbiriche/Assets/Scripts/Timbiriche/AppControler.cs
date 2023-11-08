using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Timbiriche
{
    public class AppControler : MonoBehaviour
    {
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

        }
    }
}