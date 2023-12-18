using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class GameInputManager:MonoBehaviour
    {
        public static GameInputManager Instance { get; private set; }

        public event EventHandler<OnScreenTouchedEventArgs> OnScreenTouched;
        public event EventHandler<OnScreenTouchedEventArgs> OnScreenTouchedCanceled;

        public class OnScreenTouchedEventArgs : EventArgs { public Vector2 touchPosition; }

        private PlayerInput playerInput;


        private void Awake()
        {
            if (!Instance)
                Instance = this;

            playerInput = new PlayerInput();

            playerInput.Enable();

            playerInput.Player.TouchScreenPress.performed += TouchScreen_performed;
            playerInput.Player.TouchScreenPress.canceled += TouchScreenPress_canceled;    
        }

        private void TouchScreenPress_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnScreenTouchedCanceled?.Invoke(this, new OnScreenTouchedEventArgs
            {
                touchPosition = GetControllerPosition()
            });
        }

        private void TouchScreen_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnScreenTouched?.Invoke(this, new OnScreenTouchedEventArgs
            {
                touchPosition = GetControllerPosition()
            });
        }

    
        public Vector2 GetControllerPosition() 
        {
            return playerInput.Player.TouchScreenPosition.ReadValue<Vector2>();
        } 

        private void OnDestroy()
        {
            playerInput.Player.TouchScreenPress.performed -= TouchScreen_performed;
            playerInput.Player.TouchScreenPress.canceled -= TouchScreenPress_canceled;

            playerInput.Dispose();
        }

    }
}
