public interface ICharacter
{
    public enum STATE { IDLE, RUN, LEFT, RIGHT, BOOSTED, JUMP, CROUCH, DEAD };
    public STATE currentState { get; set; }
    public bool isJumpStarted { get; set; }
}
