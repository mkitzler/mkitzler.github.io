namespace BouncingDVDLogo
{
	public class Dimensions
	{
		public double Width { get; set; }
		public double Height { get; set; }

		public override string ToString()
		{
			return $"(Width: {Width}, Height: {Height})";
		}
	}
}
