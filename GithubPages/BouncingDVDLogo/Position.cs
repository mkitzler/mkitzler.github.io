namespace BouncingDVDLogo
{
    public class Position
    {
        public double X { get; set; }
        public double Y { get; set; }

		public Position(double x, double y)
		{
			X = x;
			Y = y;
		}

        /// <summary>
        /// Calculates the angle between two points.
        /// </summary>
        /// <param name="pos">The other position</param>
        /// <returns>An angle, θ, measured in radians, such that 0 ≤ θ ≤ 2π</returns>
        public double AngleTo(Position pos)
		{
            double angle = Math.Atan2(-pos.Y + Y, pos.X - X);
            return angle < 0 ? angle + 2 * Math.PI : angle;
        }

		public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }
    }
}
