public interface IRunnerCharacter
{
    public enum STATE { IDLE, RUN, LEFT, RIGHT, BOOSTED, JUMP, CROUCH, DEAD };
    public STATE CurrentState { get; set; }

    public bool IsJumping { get; set; }
}
