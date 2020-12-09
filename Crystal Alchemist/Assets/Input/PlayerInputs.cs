// GENERATED AUTOMATICALLY FROM 'Assets/Input/PlayerInputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputs : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputs"",
    ""maps"": [
        {
            ""name"": ""Controls"",
            ""id"": ""a5b0f353-7533-4396-8360-c766e0cba2f2"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""1be5da47-d7cf-463a-88b4-bea204bb63e7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""51555b6d-4d31-4896-a050-c397efffb385"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""a8c68749-33ab-4651-a5b6-39f0fe2529fc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""3edb0133-2426-4657-9af1-8a1c2fa7c8fd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""a7c7de5c-65c8-4f61-94da-aa0e43a6d936"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AButton"",
                    ""type"": ""Button"",
                    ""id"": ""efd5ec90-2bfb-494b-90a1-cf842de18941"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""BButton"",
                    ""type"": ""Button"",
                    ""id"": ""44742763-def2-4830-a7c5-8a3f48f18d07"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""XButton"",
                    ""type"": ""Button"",
                    ""id"": ""c90c0a6e-2167-4657-a53e-33ad1fe09da5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""YButton"",
                    ""type"": ""Button"",
                    ""id"": ""4da05189-9c7a-45ed-baa8-ab92a648d516"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""LBButton"",
                    ""type"": ""Button"",
                    ""id"": ""0ba21f36-e306-4492-b2f2-112deff40fca"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""RBButton"",
                    ""type"": ""Button"",
                    ""id"": ""3f000d71-59dd-4c65-8c91-0fe2201335c2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Targeting"",
                    ""type"": ""Button"",
                    ""id"": ""4932499c-ad94-4dd2-8c3b-3d2204204ed8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""NextPage"",
                    ""type"": ""Button"",
                    ""id"": ""643332e5-0556-4b5d-b7c2-2c62b42008b3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""PreviousPage"",
                    ""type"": ""Button"",
                    ""id"": ""4442afbf-09df-473d-beb7-f7a78513fa30"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseClick"",
                    ""type"": ""Button"",
                    ""id"": ""8cc5257e-8456-4270-b138-920e8cdce630"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseMovement"",
                    ""type"": ""Value"",
                    ""id"": ""caf983b6-b178-4f0e-ba4d-ef4628235eeb"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Arrows"",
                    ""id"": ""8c734192-3db5-45f4-b8be-ef8442401855"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""aa3db559-6bb9-4933-908a-be37f8249808"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9667d11f-eb2b-4c89-9a58-3f3a907d1e0d"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ccf3fd61-16e9-4fa5-9e4a-e53fd73aada0"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""956a33dd-7bf0-4b25-bf12-70f168e1ea1f"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a5e9f300-8b83-4f0d-83dd-4abcf976cecc"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""027b37a1-9bee-45fb-bd47-069c2355bdf5"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""28936e66-3790-4409-bc6f-06e8534c7e1f"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3cdce0f3-f8cf-4860-9b74-c11cc40c9549"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""864adb11-1cdd-4bde-98a6-64575213b457"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""NormalizeVector2,StickDeadzone"",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""37a2bd1c-e778-44c0-83bf-9cc20685f38b"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7c023b25-833e-49eb-9869-32e2571a7725"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e68e743e-ad72-4e9d-a161-e8ccd3aafa92"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1b813a94-62a5-4183-8d73-528c156836a3"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1c205a16-43de-445a-98a1-b15e37a51f31"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""afb5811e-f4b5-4861-93a0-d954853a77a7"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0f5a4aa0-dc50-4725-93de-f461808bbacb"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a1aed803-ee79-461c-982c-c4a94d1eb6a7"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d71d11c0-3bf7-4d18-a61d-5371a301fb1e"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f867ef5d-7f21-4812-8fe4-f5d903f93000"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""63b35f10-0899-4d4b-9caf-27a7764602cb"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb7eca8f-49c5-4a95-a27f-74cab3a99ff8"",
                    ""path"": ""<Keyboard>/numpad1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""247d4877-f7c8-4a5e-b1de-00cfd11c0094"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""577966a6-5382-47f7-9a9a-b69abb343c46"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb9f9a6a-7dc1-4bfe-b966-5f6090601d89"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""521c66b8-f1dc-4df4-bbab-70262efbe936"",
                    ""path"": ""<Keyboard>/numpad2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d38b63fb-ac83-4bc5-9002-a18798a72ca1"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b566fced-a659-4814-b4ba-0080e1cd7630"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2426d743-3ae0-4a18-a212-a831fe39d78a"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""XButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a93b3f53-47cf-4f98-b5d6-aa829f74ae96"",
                    ""path"": ""<Keyboard>/numpad3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""XButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""01a77d72-07db-428d-99e2-1692e3a1629d"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""XButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dd1f0598-49fb-4751-917f-a693d11edffe"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""XButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6e707d6b-84b2-4606-818e-6fdb246125e8"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""YButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""27ba83c6-f131-44b2-b13b-cbcae8a6ded1"",
                    ""path"": ""<Keyboard>/numpad4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""YButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""60144ade-e6db-44ae-a051-9d7831136334"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""YButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87caa500-7766-46ae-a6a7-24e8c8b5a4a1"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""YButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a4d9a5a9-2b0b-43d9-89c2-9cf78722ad10"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RBButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9d001f64-f31a-462a-8be3-b44ee7ec39c2"",
                    ""path"": ""<Keyboard>/numpad6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RBButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""05ea6491-9773-40cd-9ecc-8709635a22a8"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RBButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4bee3fc6-3c34-426b-9e32-1efa928b8be1"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RBButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8603666e-836a-489b-9c0e-ca012ada72a4"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LBButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""62f04213-ce01-4bef-beb1-0c2481a4265a"",
                    ""path"": ""<Keyboard>/numpad5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LBButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""235730b5-a3e7-4ead-934f-dac0de2c72f9"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LBButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""062bdad5-e17c-4750-b8ed-d2c1ae5d90a5"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LBButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""DPad"",
                    ""id"": ""05eadd25-a423-4053-a773-cbca2ccfe4ea"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Targeting"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""efd73892-6586-4a2b-bfd3-cf653fcafad1"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Targeting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""cf3f63b8-7cd3-4942-91d2-6f1f88726604"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Targeting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ad68db27-ecd5-49f3-b1b1-4dc77f36411c"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Targeting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""98bf0eb0-2575-4570-be7a-2034ffedc354"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Targeting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""2b53c779-e72d-4af0-b1d0-14ca374e853e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Targeting"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""ca9ba5a9-c079-46bc-aad2-9784f5e6a131"",
                    ""path"": ""<Keyboard>/f1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Targeting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b8fca735-aa1a-4354-9c9a-66d15f1afe74"",
                    ""path"": ""<Keyboard>/f2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Targeting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""32ea2181-9399-49a1-995b-0a06ec989d0a"",
                    ""path"": ""<Keyboard>/f3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Targeting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""06ef0fc0-2ee6-4593-ad9f-583dd77974e4"",
                    ""path"": ""<Keyboard>/f4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Targeting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""b2575d50-be56-4258-a3c0-e521e5bc5db3"",
                    ""path"": ""<Keyboard>/pageUp"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NextPage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""698eace5-ab88-4929-aff1-afddee2e0067"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NextPage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2f360212-29cb-4bc8-8143-5f459ba6e6c5"",
                    ""path"": ""<Keyboard>/pageDown"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PreviousPage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f76432eb-233e-49d4-8576-8904dc43f3b3"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PreviousPage"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2d39f9cf-e083-41f9-a883-8f3a6d7a0d8d"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""61d7f9cc-0943-4ce0-b81c-e78388ebe527"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Controls
        m_Controls = asset.FindActionMap("Controls", throwIfNotFound: true);
        m_Controls_Movement = m_Controls.FindAction("Movement", throwIfNotFound: true);
        m_Controls_Pause = m_Controls.FindAction("Pause", throwIfNotFound: true);
        m_Controls_Inventory = m_Controls.FindAction("Inventory", throwIfNotFound: true);
        m_Controls_Submit = m_Controls.FindAction("Submit", throwIfNotFound: true);
        m_Controls_Cancel = m_Controls.FindAction("Cancel", throwIfNotFound: true);
        m_Controls_AButton = m_Controls.FindAction("AButton", throwIfNotFound: true);
        m_Controls_BButton = m_Controls.FindAction("BButton", throwIfNotFound: true);
        m_Controls_XButton = m_Controls.FindAction("XButton", throwIfNotFound: true);
        m_Controls_YButton = m_Controls.FindAction("YButton", throwIfNotFound: true);
        m_Controls_LBButton = m_Controls.FindAction("LBButton", throwIfNotFound: true);
        m_Controls_RBButton = m_Controls.FindAction("RBButton", throwIfNotFound: true);
        m_Controls_Targeting = m_Controls.FindAction("Targeting", throwIfNotFound: true);
        m_Controls_NextPage = m_Controls.FindAction("NextPage", throwIfNotFound: true);
        m_Controls_PreviousPage = m_Controls.FindAction("PreviousPage", throwIfNotFound: true);
        m_Controls_MouseClick = m_Controls.FindAction("MouseClick", throwIfNotFound: true);
        m_Controls_MouseMovement = m_Controls.FindAction("MouseMovement", throwIfNotFound: true);
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

    // Controls
    private readonly InputActionMap m_Controls;
    private IControlsActions m_ControlsActionsCallbackInterface;
    private readonly InputAction m_Controls_Movement;
    private readonly InputAction m_Controls_Pause;
    private readonly InputAction m_Controls_Inventory;
    private readonly InputAction m_Controls_Submit;
    private readonly InputAction m_Controls_Cancel;
    private readonly InputAction m_Controls_AButton;
    private readonly InputAction m_Controls_BButton;
    private readonly InputAction m_Controls_XButton;
    private readonly InputAction m_Controls_YButton;
    private readonly InputAction m_Controls_LBButton;
    private readonly InputAction m_Controls_RBButton;
    private readonly InputAction m_Controls_Targeting;
    private readonly InputAction m_Controls_NextPage;
    private readonly InputAction m_Controls_PreviousPage;
    private readonly InputAction m_Controls_MouseClick;
    private readonly InputAction m_Controls_MouseMovement;
    public struct ControlsActions
    {
        private @PlayerInputs m_Wrapper;
        public ControlsActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Controls_Movement;
        public InputAction @Pause => m_Wrapper.m_Controls_Pause;
        public InputAction @Inventory => m_Wrapper.m_Controls_Inventory;
        public InputAction @Submit => m_Wrapper.m_Controls_Submit;
        public InputAction @Cancel => m_Wrapper.m_Controls_Cancel;
        public InputAction @AButton => m_Wrapper.m_Controls_AButton;
        public InputAction @BButton => m_Wrapper.m_Controls_BButton;
        public InputAction @XButton => m_Wrapper.m_Controls_XButton;
        public InputAction @YButton => m_Wrapper.m_Controls_YButton;
        public InputAction @LBButton => m_Wrapper.m_Controls_LBButton;
        public InputAction @RBButton => m_Wrapper.m_Controls_RBButton;
        public InputAction @Targeting => m_Wrapper.m_Controls_Targeting;
        public InputAction @NextPage => m_Wrapper.m_Controls_NextPage;
        public InputAction @PreviousPage => m_Wrapper.m_Controls_PreviousPage;
        public InputAction @MouseClick => m_Wrapper.m_Controls_MouseClick;
        public InputAction @MouseMovement => m_Wrapper.m_Controls_MouseMovement;
        public InputActionMap Get() { return m_Wrapper.m_Controls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ControlsActions set) { return set.Get(); }
        public void SetCallbacks(IControlsActions instance)
        {
            if (m_Wrapper.m_ControlsActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnMovement;
                @Pause.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPause;
                @Inventory.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnInventory;
                @Inventory.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnInventory;
                @Inventory.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnInventory;
                @Submit.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnSubmit;
                @Cancel.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnCancel;
                @AButton.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnAButton;
                @AButton.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnAButton;
                @AButton.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnAButton;
                @BButton.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnBButton;
                @BButton.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnBButton;
                @BButton.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnBButton;
                @XButton.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnXButton;
                @XButton.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnXButton;
                @XButton.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnXButton;
                @YButton.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnYButton;
                @YButton.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnYButton;
                @YButton.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnYButton;
                @LBButton.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnLBButton;
                @LBButton.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnLBButton;
                @LBButton.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnLBButton;
                @RBButton.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnRBButton;
                @RBButton.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnRBButton;
                @RBButton.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnRBButton;
                @Targeting.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnTargeting;
                @Targeting.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnTargeting;
                @Targeting.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnTargeting;
                @NextPage.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnNextPage;
                @NextPage.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnNextPage;
                @NextPage.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnNextPage;
                @PreviousPage.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPreviousPage;
                @PreviousPage.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPreviousPage;
                @PreviousPage.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnPreviousPage;
                @MouseClick.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnMouseClick;
                @MouseClick.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnMouseClick;
                @MouseClick.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnMouseClick;
                @MouseMovement.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnMouseMovement;
                @MouseMovement.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnMouseMovement;
                @MouseMovement.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnMouseMovement;
            }
            m_Wrapper.m_ControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Inventory.started += instance.OnInventory;
                @Inventory.performed += instance.OnInventory;
                @Inventory.canceled += instance.OnInventory;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @AButton.started += instance.OnAButton;
                @AButton.performed += instance.OnAButton;
                @AButton.canceled += instance.OnAButton;
                @BButton.started += instance.OnBButton;
                @BButton.performed += instance.OnBButton;
                @BButton.canceled += instance.OnBButton;
                @XButton.started += instance.OnXButton;
                @XButton.performed += instance.OnXButton;
                @XButton.canceled += instance.OnXButton;
                @YButton.started += instance.OnYButton;
                @YButton.performed += instance.OnYButton;
                @YButton.canceled += instance.OnYButton;
                @LBButton.started += instance.OnLBButton;
                @LBButton.performed += instance.OnLBButton;
                @LBButton.canceled += instance.OnLBButton;
                @RBButton.started += instance.OnRBButton;
                @RBButton.performed += instance.OnRBButton;
                @RBButton.canceled += instance.OnRBButton;
                @Targeting.started += instance.OnTargeting;
                @Targeting.performed += instance.OnTargeting;
                @Targeting.canceled += instance.OnTargeting;
                @NextPage.started += instance.OnNextPage;
                @NextPage.performed += instance.OnNextPage;
                @NextPage.canceled += instance.OnNextPage;
                @PreviousPage.started += instance.OnPreviousPage;
                @PreviousPage.performed += instance.OnPreviousPage;
                @PreviousPage.canceled += instance.OnPreviousPage;
                @MouseClick.started += instance.OnMouseClick;
                @MouseClick.performed += instance.OnMouseClick;
                @MouseClick.canceled += instance.OnMouseClick;
                @MouseMovement.started += instance.OnMouseMovement;
                @MouseMovement.performed += instance.OnMouseMovement;
                @MouseMovement.canceled += instance.OnMouseMovement;
            }
        }
    }
    public ControlsActions @Controls => new ControlsActions(this);
    public interface IControlsActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnInventory(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnAButton(InputAction.CallbackContext context);
        void OnBButton(InputAction.CallbackContext context);
        void OnXButton(InputAction.CallbackContext context);
        void OnYButton(InputAction.CallbackContext context);
        void OnLBButton(InputAction.CallbackContext context);
        void OnRBButton(InputAction.CallbackContext context);
        void OnTargeting(InputAction.CallbackContext context);
        void OnNextPage(InputAction.CallbackContext context);
        void OnPreviousPage(InputAction.CallbackContext context);
        void OnMouseClick(InputAction.CallbackContext context);
        void OnMouseMovement(InputAction.CallbackContext context);
    }
}
