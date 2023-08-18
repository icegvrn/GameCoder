using UnityEngine;

public class ToggleInt : MonoBehaviour
{
    [SerializeField] bool isOn;
    public bool IsOn { get { return isOn; } }
    [SerializeField] int value;
    public int Value { get { return value; } }

    public void Toggle()
    {
        isOn = !isOn;
    }
}
