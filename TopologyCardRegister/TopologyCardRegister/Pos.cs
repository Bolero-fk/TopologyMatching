namespace TopologyCardRegister
{
    /// <summary>
    /// グリッドの座標を管理するクラスです
    /// </summary>
    public class Pos
    {
        public int X { get; }
        public int Y { get; }

        public Pos()
        {
            this.X = 0;
            this.Y = 0;
        }

        public Pos(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Pos operator +(Pos a, Pos b)
        {
            return new Pos(a.X + b.X, a.Y + b.Y);
        }

        public static Pos operator -(Pos a, Pos b)
        {
            return new Pos(a.X - b.X, a.Y - b.Y);
        }

        public static readonly Pos UP = new Pos(0, 1);
        public static readonly Pos RIGHT = new Pos(1, 0);
        public static readonly Pos DOWN = new Pos(0, -1);
        public static readonly Pos LEFT = new Pos(-1, 0);
    }
}