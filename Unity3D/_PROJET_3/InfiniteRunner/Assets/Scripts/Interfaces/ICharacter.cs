public interface ICharacter
{
    public enum STATE { IDLE, RUN, LEFT, RIGHT, BOOSTED, JUMP, CROUCH, DEAD };
    public STATE CurrentState { get; set; }
}
