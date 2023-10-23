namespace TopologyCardRegister
{
    /// <summary>
    /// グリッド上の2D座標を表すクラスです。
    /// </summary>
    public class Pos
    {
        /// <summary> X座標の値を取得または設定します。 </summary>
        public int X { get; set; }

        /// <summary> Y座標の値を取得または設定します。 </summary>
        public int Y { get; set; }

        /// <summary>
        /// デフォルトの座標 (0,0) を持つPosのインスタンスを初期化します。
        /// </summary>
        public Pos()
        {
            this.X = 0;
            this.Y = 0;
        }

        /// <summary>
        /// 指定されたX座標とY座標を持つPosのインスタンスを初期化します。
        /// </summary>
        /// <param name="x">X座標の値。</param>
        /// <param name="y">Y座標の値。</param>
        public Pos(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// 二つのPos座標を加算します。
        /// </summary>
        public static Pos operator +(Pos a, Pos b)
        {
            checked
            {
                return new Pos(a.X + b.X, a.Y + b.Y);
            }
        }

        /// <summary>
        /// 二つのPos座標を減算します。
        /// </summary>
        public static Pos operator -(Pos a, Pos b)
        {
            checked
            {
                return new Pos(a.X - b.X, a.Y - b.Y);
            }
        }

        /// <summary> 上方向の単位ベクトル </summary>
        public static readonly Pos UP = new Pos(0, 1);

        /// <summary> 右方向の単位ベクトル </summary>
        public static readonly Pos RIGHT = new Pos(1, 0);

        /// <summary> 下方向の単位ベクトル </summary>
        public static readonly Pos DOWN = new Pos(0, -1);

        /// <summary> 左方向の単位ベクトル </summary>
        public static readonly Pos LEFT = new Pos(-1, 0);

        public override int GetHashCode()
        {
            return unchecked((this.X * 33) ^ this.Y);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Pos other)
            {
                return false;
            }

            return this.X == other.X && this.Y == other.Y;
        }
    }
}
