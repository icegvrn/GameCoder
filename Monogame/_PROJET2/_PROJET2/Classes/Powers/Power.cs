namespace BricksGame
{
    public abstract class Power
    {
        public Power() { }

        public abstract bool powerAvailable { get; set; }
        public abstract bool powerCharged { get; set; }
        public abstract bool powerUsed { get; set; }
        public abstract void Activate();
        public abstract void Trigger(Bricks brick, BaseGrid grid);
    }
}
