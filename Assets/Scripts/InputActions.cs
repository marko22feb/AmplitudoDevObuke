// GENERATED AUTOMATICALLY FROM 'Assets/InputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""Actions"",
            ""id"": ""c8f2e37d-ba30-4b76-82a9-ca637bfcb00f"",
            ""actions"": [
                {
                    ""name"": ""Debug"",
                    ""type"": ""Button"",
                    ""id"": ""fc4d4bce-5381-43be-b0bd-81bfd0067481"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""4b36d72f-c8d6-4a42-8b57-2d4e91bfa942"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Melee"",
                    ""type"": ""Button"",
                    ""id"": ""0e4872d5-aba5-45d3-b857-d5909452b557"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""0041f736-df31-499b-a469-e2a6db189aeb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""68528c8f-43d6-4d13-ab31-0903e91adc68"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Consumable Left"",
                    ""type"": ""Button"",
                    ""id"": ""88c8c8ac-9bf4-46a6-ab49-0b18de8241b9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Consumable Right"",
                    ""type"": ""Button"",
                    ""id"": ""caada6c0-baee-4d69-8564-fe8b6d5490ea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""4a9591ac-ebae-4a1d-9dcd-e2980c40b2f8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""fa937ad3-c819-4dd9-bc77-2597d8df67f4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""c4a34d24-52fe-4350-95fe-56167299db52"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Debug"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""367867ca-7955-4a27-ad49-47084dd88a64"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Debug"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""c98e5651-a077-4fc2-b99e-d0efa0d9e475"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Debug"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e523f221-766b-4d5b-9cf0-21e0e8020b6d"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b3f18c8e-74ac-4a43-9f28-273d8c1bf100"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ae848a27-0144-4989-998b-ce5ae401f8be"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5efc0a64-866e-4630-84a4-893f8dbdc765"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Melee"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""906be446-02c5-48a7-9ab8-8c82dd5a7899"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""25e4a039-440d-4ca3-9372-3b7ebb8cdca2"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24052a0f-5ab5-4b4e-8167-9245ac4a49c3"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Consumable Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bd6953b1-1fc7-4de9-bd9a-c59051dddb2c"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Consumable Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9a2a8be9-7a85-4cf5-a2f2-80f0701ecf57"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f5e13fe2-866a-4954-bd56-682c06169a80"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Player"",
            ""id"": ""c2112ec5-61c1-42d4-a057-b062ce893792"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""24cd18a5-43c5-4ab1-99b4-283f139f016d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f091ba95-f431-4707-868d-9183389b3e92"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Actions
        m_Actions = asset.FindActionMap("Actions", throwIfNotFound: true);
        m_Actions_Debug = m_Actions.FindAction("Debug", throwIfNotFound: true);
        m_Actions_Shoot = m_Actions.FindAction("Shoot", throwIfNotFound: true);
        m_Actions_Melee = m_Actions.FindAction("Melee", throwIfNotFound: true);
        m_Actions_Interact = m_Actions.FindAction("Interact", throwIfNotFound: true);
        m_Actions_Inventory = m_Actions.FindAction("Inventory", throwIfNotFound: true);
        m_Actions_ConsumableLeft = m_Actions.FindAction("Consumable Left", throwIfNotFound: true);
        m_Actions_ConsumableRight = m_Actions.FindAction("Consumable Right", throwIfNotFound: true);
        m_Actions_Jump = m_Actions.FindAction("Jump", throwIfNotFound: true);
        m_Actions_Crouch = m_Actions.FindAction("Crouch", throwIfNotFound: true);
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Newaction = m_Player.FindAction("New action", throwIfNotFound: true);
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

    // Actions
    private readonly InputActionMap m_Actions;
    private IActionsActions m_ActionsActionsCallbackInterface;
    private readonly InputAction m_Actions_Debug;
    private readonly InputAction m_Actions_Shoot;
    private readonly InputAction m_Actions_Melee;
    private readonly InputAction m_Actions_Interact;
    private readonly InputAction m_Actions_Inventory;
    private readonly InputAction m_Actions_ConsumableLeft;
    private readonly InputAction m_Actions_ConsumableRight;
    private readonly InputAction m_Actions_Jump;
    private readonly InputAction m_Actions_Crouch;
    public struct ActionsActions
    {
        private @InputActions m_Wrapper;
        public ActionsActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Debug => m_Wrapper.m_Actions_Debug;
        public InputAction @Shoot => m_Wrapper.m_Actions_Shoot;
        public InputAction @Melee => m_Wrapper.m_Actions_Melee;
        public InputAction @Interact => m_Wrapper.m_Actions_Interact;
        public InputAction @Inventory => m_Wrapper.m_Actions_Inventory;
        public InputAction @ConsumableLeft => m_Wrapper.m_Actions_ConsumableLeft;
        public InputAction @ConsumableRight => m_Wrapper.m_Actions_ConsumableRight;
        public InputAction @Jump => m_Wrapper.m_Actions_Jump;
        public InputAction @Crouch => m_Wrapper.m_Actions_Crouch;
        public InputActionMap Get() { return m_Wrapper.m_Actions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ActionsActions set) { return set.Get(); }
        public void SetCallbacks(IActionsActions instance)
        {
            if (m_Wrapper.m_ActionsActionsCallbackInterface != null)
            {
                @Debug.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnDebug;
                @Debug.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnDebug;
                @Debug.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnDebug;
                @Shoot.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnShoot;
                @Melee.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnMelee;
                @Melee.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnMelee;
                @Melee.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnMelee;
                @Interact.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnInteract;
                @Inventory.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnInventory;
                @Inventory.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnInventory;
                @Inventory.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnInventory;
                @ConsumableLeft.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnConsumableLeft;
                @ConsumableLeft.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnConsumableLeft;
                @ConsumableLeft.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnConsumableLeft;
                @ConsumableRight.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnConsumableRight;
                @ConsumableRight.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnConsumableRight;
                @ConsumableRight.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnConsumableRight;
                @Jump.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnJump;
                @Crouch.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnCrouch;
            }
            m_Wrapper.m_ActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Debug.started += instance.OnDebug;
                @Debug.performed += instance.OnDebug;
                @Debug.canceled += instance.OnDebug;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @Melee.started += instance.OnMelee;
                @Melee.performed += instance.OnMelee;
                @Melee.canceled += instance.OnMelee;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Inventory.started += instance.OnInventory;
                @Inventory.performed += instance.OnInventory;
                @Inventory.canceled += instance.OnInventory;
                @ConsumableLeft.started += instance.OnConsumableLeft;
                @ConsumableLeft.performed += instance.OnConsumableLeft;
                @ConsumableLeft.canceled += instance.OnConsumableLeft;
                @ConsumableRight.started += instance.OnConsumableRight;
                @ConsumableRight.performed += instance.OnConsumableRight;
                @ConsumableRight.canceled += instance.OnConsumableRight;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
            }
        }
    }
    public ActionsActions @Actions => new ActionsActions(this);

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Newaction;
    public struct PlayerActions
    {
        private @InputActions m_Wrapper;
        public PlayerActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_Player_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Newaction.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNewaction;
                @Newaction.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNewaction;
                @Newaction.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNewaction;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Newaction.started += instance.OnNewaction;
                @Newaction.performed += instance.OnNewaction;
                @Newaction.canceled += instance.OnNewaction;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IActionsActions
    {
        void OnDebug(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnMelee(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnInventory(InputAction.CallbackContext context);
        void OnConsumableLeft(InputAction.CallbackContext context);
        void OnConsumableRight(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
    }
    public interface IPlayerActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
}
