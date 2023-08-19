using UnityEngine;

public class InputService : IInputService
{
    public enum ActionKey { up, down, left, right, escape, cheatGemme }
    public enum ControlMode { azerty, qwerty }

    public ControlMode currentMode = ControlMode.azerty;


    public bool GetKey(ActionKey key)
    {
        KeyCode keyCode = GetKeyByControlMode(key);
        return Input.GetKey(keyCode);
    }

    public bool GetKeyDown(ActionKey key)
    {
        KeyCode keyCode = GetKeyByControlMode(key);
        return Input.GetKeyDown(keyCode);
    }

    public bool GetKeyUp(ActionKey key)
    {
        KeyCode keyCode = GetKeyByControlMode(key);
        return Input.GetKeyUp(keyCode);
    }

    public float GetHorizontalAxis()
    {
        return Input.GetAxis(GetHorizontalAxisName());
    }

    private string GetHorizontalAxisName()
    {
        return currentMode == ControlMode.azerty ? "AzertyHorizontal" : "QwertyHorizontal";
    }

    private KeyCode GetKeyByControlMode(ActionKey key)
    {
        switch (currentMode)
        {
            case ControlMode.azerty:
                switch (key)
                {
                    case ActionKey.up: return KeyCode.Z;
                    case ActionKey.down: return KeyCode.S;
                    case ActionKey.left: return KeyCode.Q;
                    case ActionKey.right: return KeyCode.D;
                    case ActionKey.escape: return KeyCode.Escape;
                    case ActionKey.cheatGemme: return KeyCode.M;
                }
                break;

            case ControlMode.qwerty:
                switch (key)
                {
                    case ActionKey.up: return KeyCode.W;
                    case ActionKey.down: return KeyCode.S;
                    case ActionKey.left: return KeyCode.A;
                    case ActionKey.right: return KeyCode.D;
                    case ActionKey.escape: return KeyCode.Escape;
                    case ActionKey.cheatGemme: return KeyCode.M;
                }
                break;
        }

        return KeyCode.None;
    }


    public void SetAzertyModeTo(bool isAzerty)
    {
        if (isAzerty)
        {
            currentMode = ControlMode.azerty;
        }
        else
        {
            currentMode = ControlMode.qwerty;
        }
    }


}
