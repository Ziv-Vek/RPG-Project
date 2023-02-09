using System;
using UnityEngine;

namespace RPG.Cinematics
{
    public class FakePlayableDirector : MonoBehaviour
    {
        public event Action<float> onFinish;
        
        void Start()
        {
            Invoke("OnFinish", 3f);
        }

        void OnFinish()
        {
            
        }
    }

}
