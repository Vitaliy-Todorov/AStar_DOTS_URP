using Assets.Scripts.Services.InputServices;
using System;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class RegisterServices : MonoBehaviour
    {
        private AllServices _containerServices = AllServices.Container;

        private void Awake()
        {
            _containerServices.RegisterSingle(InputKeyboardMouseService());
        }

        private InputKeyboardMouseService InputKeyboardMouseService() => 
            gameObject.AddComponent<InputKeyboardMouseService>();
    }
}