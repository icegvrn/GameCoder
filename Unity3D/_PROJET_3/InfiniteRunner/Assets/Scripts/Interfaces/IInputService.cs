
    public interface IInputService
    {
        bool GetKey(InputService.ActionKey key);
        bool GetKeyDown(InputService.ActionKey key);
        bool GetKeyUp(InputService.ActionKey key);
        float GetHorizontalAxis();
        void SetAzertyModeTo(bool isAzerty);
}

