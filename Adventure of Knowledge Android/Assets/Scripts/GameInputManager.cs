using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class GameInputManager:MonoBehaviour
    {
        public static GameInputManager Instance { get; private set; }

        public event EventHandler<OnScreenTouchedEventArgs> OnScreenTouched;
        public event EventHandler OnScreenTouchedCanceled;

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

            playerInput.Player.MouseLeftButtonPress.performed += TouchScreen_performed;
            playerInput.Player.MouseLeftButtonPress.canceled += TouchScreenPress_canceled;     
        }

        private void TouchScreenPress_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnScreenTouchedCanceled?.Invoke(this, EventArgs.Empty);
        }

        private void TouchScreen_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            OnScreenTouched?.Invoke(this, new OnScreenTouchedEventArgs
            {
                touchPosition = GetControllerPosition()
            });
        }

        public Vector3 GetMousePosition() => playerInput.Player.MousePosition.ReadValue<Vector2>();
        
        public Vector3 GetTouchPosition() => playerInput.Player.TouchScreenPosition.ReadValue<Vector2>();
      
        public Vector2 GetControllerPosition() 
        {
            if (GetMousePosition() != Vector3.zero) return GetMousePosition();
            else if (GetTouchPosition() != Vector3.zero) return GetTouchPosition();
            else return Vector2.zero;  
        } 

        private void OnDestroy()
        {
            playerInput.Player.TouchScreenPress.performed -= TouchScreen_performed;
            playerInput.Player.TouchScreenPress.canceled -= TouchScreenPress_canceled;


            playerInput.Player.MouseLeftButtonPress.performed -= TouchScreen_performed;
            playerInput.Player.MouseLeftButtonPress.canceled -= TouchScreenPress_canceled;

            playerInput.Dispose();
        }

    }
}
