//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.0
//     from Assets/Scripts/InputManagement/GameInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @GameInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInputActions"",
    ""maps"": [
        {
            ""name"": ""PlayerInputs"",
            ""id"": ""65e36f6a-b463-4456-b187-1049039e70ea"",
            ""actions"": [
                {
                    ""name"": ""LeftClick"",
                    ""type"": ""Button"",
                    ""id"": ""ff8e6495-5214-4e36-a76c-5b34e9e47dcb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""Button"",
                    ""id"": ""d53f9279-142f-4b2c-af6a-cabc11a171ab"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""7b68f61d-56ef-4684-9ccc-3ddb1b3fb3a7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""cd210b15-7177-487e-a511-2ba79222315c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d38c87c1-1f13-401d-a7a7-6aa0fc91aa27"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e58b18c4-53e8-4517-a5a7-7286ea0322c7"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerInputsCamera"",
            ""id"": ""39156f0a-847f-4428-96c7-c51a6b1f69e8"",
            ""actions"": [
                {
                    ""name"": ""Drag"",
                    ""type"": ""Button"",
                    ""id"": ""b3a096fe-9dba-429c-8462-31fc7e006942"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d2884454-6861-4dab-a026-6351b4410bbc"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""dacc7290-6947-4d09-96ee-073b2b103d0b"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""db98ce04-b5b3-48d9-8375-9367445beaf6"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drag"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerInputs
        m_PlayerInputs = asset.FindActionMap("PlayerInputs", throwIfNotFound: true);
        m_PlayerInputs_LeftClick = m_PlayerInputs.FindAction("LeftClick", throwIfNotFound: true);
        m_PlayerInputs_RightClick = m_PlayerInputs.FindAction("RightClick", throwIfNotFound: true);
        m_PlayerInputs_Move = m_PlayerInputs.FindAction("Move", throwIfNotFound: true);
        // PlayerInputsCamera
        m_PlayerInputsCamera = asset.FindActionMap("PlayerInputsCamera", throwIfNotFound: true);
        m_PlayerInputsCamera_Drag = m_PlayerInputsCamera.FindAction("Drag", throwIfNotFound: true);
        m_PlayerInputsCamera_Zoom = m_PlayerInputsCamera.FindAction("Zoom", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerInputs
    private readonly InputActionMap m_PlayerInputs;
    private List<IPlayerInputsActions> m_PlayerInputsActionsCallbackInterfaces = new List<IPlayerInputsActions>();
    private readonly InputAction m_PlayerInputs_LeftClick;
    private readonly InputAction m_PlayerInputs_RightClick;
    private readonly InputAction m_PlayerInputs_Move;
    public struct PlayerInputsActions
    {
        private @GameInputActions m_Wrapper;
        public PlayerInputsActions(@GameInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftClick => m_Wrapper.m_PlayerInputs_LeftClick;
        public InputAction @RightClick => m_Wrapper.m_PlayerInputs_RightClick;
        public InputAction @Move => m_Wrapper.m_PlayerInputs_Move;
        public InputActionMap Get() { return m_Wrapper.m_PlayerInputs; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerInputsActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerInputsActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerInputsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerInputsActionsCallbackInterfaces.Add(instance);
            @LeftClick.started += instance.OnLeftClick;
            @LeftClick.performed += instance.OnLeftClick;
            @LeftClick.canceled += instance.OnLeftClick;
            @RightClick.started += instance.OnRightClick;
            @RightClick.performed += instance.OnRightClick;
            @RightClick.canceled += instance.OnRightClick;
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
        }

        private void UnregisterCallbacks(IPlayerInputsActions instance)
        {
            @LeftClick.started -= instance.OnLeftClick;
            @LeftClick.performed -= instance.OnLeftClick;
            @LeftClick.canceled -= instance.OnLeftClick;
            @RightClick.started -= instance.OnRightClick;
            @RightClick.performed -= instance.OnRightClick;
            @RightClick.canceled -= instance.OnRightClick;
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
        }

        public void RemoveCallbacks(IPlayerInputsActions instance)
        {
            if (m_Wrapper.m_PlayerInputsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerInputsActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerInputsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerInputsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerInputsActions @PlayerInputs => new PlayerInputsActions(this);

    // PlayerInputsCamera
    private readonly InputActionMap m_PlayerInputsCamera;
    private List<IPlayerInputsCameraActions> m_PlayerInputsCameraActionsCallbackInterfaces = new List<IPlayerInputsCameraActions>();
    private readonly InputAction m_PlayerInputsCamera_Drag;
    private readonly InputAction m_PlayerInputsCamera_Zoom;
    public struct PlayerInputsCameraActions
    {
        private @GameInputActions m_Wrapper;
        public PlayerInputsCameraActions(@GameInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Drag => m_Wrapper.m_PlayerInputsCamera_Drag;
        public InputAction @Zoom => m_Wrapper.m_PlayerInputsCamera_Zoom;
        public InputActionMap Get() { return m_Wrapper.m_PlayerInputsCamera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerInputsCameraActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerInputsCameraActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerInputsCameraActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerInputsCameraActionsCallbackInterfaces.Add(instance);
            @Drag.started += instance.OnDrag;
            @Drag.performed += instance.OnDrag;
            @Drag.canceled += instance.OnDrag;
            @Zoom.started += instance.OnZoom;
            @Zoom.performed += instance.OnZoom;
            @Zoom.canceled += instance.OnZoom;
        }

        private void UnregisterCallbacks(IPlayerInputsCameraActions instance)
        {
            @Drag.started -= instance.OnDrag;
            @Drag.performed -= instance.OnDrag;
            @Drag.canceled -= instance.OnDrag;
            @Zoom.started -= instance.OnZoom;
            @Zoom.performed -= instance.OnZoom;
            @Zoom.canceled -= instance.OnZoom;
        }

        public void RemoveCallbacks(IPlayerInputsCameraActions instance)
        {
            if (m_Wrapper.m_PlayerInputsCameraActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerInputsCameraActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerInputsCameraActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerInputsCameraActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerInputsCameraActions @PlayerInputsCamera => new PlayerInputsCameraActions(this);
    public interface IPlayerInputsActions
    {
        void OnLeftClick(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
    }
    public interface IPlayerInputsCameraActions
    {
        void OnDrag(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
    }
}
