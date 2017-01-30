using System;
namespace questiongps
{
	public class PosAndDistance
	{
		double x;
		double y;
		double z;
		double d;
		public PosAndDistance(double x, double y, double z, double d)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.d = d;
		}
		public double getX()
		{
			return x;
		}
		public double getY()
		{
			return y;
		}
		public double getZ()
		{
			return z;
		}
		public double getD()
		{
			return d;
		}
	}
}

