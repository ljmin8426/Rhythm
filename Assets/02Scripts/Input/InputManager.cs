using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private InputMapping inputMapping;
    private Dictionary<string, string> keyBindings;

    // Gameplay callbacks
    public Action<float> OnNote0;
    public Action<float> OnNote1;
    public Action<float> OnNote2;
    public Action<float> OnNote3;
    public Action<float> OnESC;

    // UI callbacks
    public Action<Vector2> OnNavigate;
    public Action<float> OnSubmit;
    public Action<float> OnCancel;

    protected override void DoAwake()
    {
        inputMapping = new InputMapping();
        keyBindings = new Dictionary<string, string>();

        BindGameplayInputs();
        BindUIInputs();

        LoadKeyBindings();

        SwitchToGameplay(); // 기본은 게임 입력 활성화
    }

    #region Bind Inputs
    private void BindGameplayInputs()
    {
        BindAction(inputMapping.Player.NoteKey0, v => OnNote0?.Invoke(v));
        BindAction(inputMapping.Player.NoteKey1, v => OnNote1?.Invoke(v));
        BindAction(inputMapping.Player.NoteKey2, v => OnNote2?.Invoke(v));
        BindAction(inputMapping.Player.NoteKey3, v => OnNote3?.Invoke(v));
        BindAction(inputMapping.Player.ESC, v => OnESC?.Invoke(v));

        inputMapping.Player.Disable();
    }

    private void BindUIInputs()
    {
        var navigate = inputMapping.UI.Newaction; // 실제 UI 액션 이름 맞춰서 수정 가능
        navigate.performed += ctx => OnNavigate?.Invoke(ctx.ReadValue<Vector2>());
        BindAction(navigate, v => OnSubmit?.Invoke(v)); // 예시, 필요에 따라 분리
        BindAction(navigate, v => OnCancel?.Invoke(v));

        inputMapping.UI.Disable();
    }

    private void BindAction(InputAction action, Action<float> callback)
    {
        action.performed += ctx => callback?.Invoke(ctx.ReadValue<float>());
    }
    #endregion

    #region Map Switching
    public void SwitchToGameplay()
    {
        inputMapping.UI.Disable();
        inputMapping.Player.Enable();
    }

    public void SwitchToUI()
    {
        inputMapping.Player.Disable();
        inputMapping.UI.Enable();
    }
    #endregion

    #region Key Rebinding
    private InputAction[] noteActions => new InputAction[]
    {
        inputMapping.Player.NoteKey0,
        inputMapping.Player.NoteKey1,
        inputMapping.Player.NoteKey2,
        inputMapping.Player.NoteKey3
    };

    public InputAction GetInputActionByIndex(int idx)
    {
        if (idx < 0 || idx >= noteActions.Length) return null;
        return noteActions[idx];
    }

    public void SetPlayerKeyBinding(int noteIndex, Key newKey)
    {
        InputAction action = GetInputActionByIndex(noteIndex);
        if (action == null) return;

        action.ApplyBindingOverride(new InputBinding
        {
            overridePath = $"<Keyboard>/{newKey}"
        });

        keyBindings[action.name] = newKey.ToString();
        SaveKeyBindings();
    }

    private void SaveKeyBindings()
    {
        foreach (var kvp in keyBindings)
            PlayerPrefs.SetString(kvp.Key, kvp.Value);

        PlayerPrefs.Save();
    }

    private void LoadKeyBindings()
    {
        foreach (var action in noteActions)
        {
            if (PlayerPrefs.HasKey(action.name))
            {
                string savedKey = PlayerPrefs.GetString(action.name);
                if (Enum.TryParse(savedKey, out Key key))
                {
                    action.ApplyBindingOverride(new InputBinding
                    {
                        overridePath = $"<Keyboard>/{key}"
                    });
                    keyBindings[action.name] = savedKey;
                }
            }
        }
    }
    #endregion
}
