using System;
namespace questiongps
{
	public class Beacon
	{
		PosAndDistance[] _beacons;
		int index;
		int n;
		public Beacon(int n)
		{
			this.n = n;
			_beacons = new PosAndDistance[n];
		}
		public bool AddBeacon(PosAndDistance beacon)
		{
			if (index < _beacons.Length)
			{
				_beacons[index++] = beacon;
				return true;
			}
			else
			{
				return false;
			}
		}
		public int size()
		{
			return n;
		}
		public PosAndDistance elOf(int index)
		{
			return _beacons[index];
		}
		public double xpos(int index)
		{
			return _beacons[index].getX();
		}
		public double ypos(int index)
		{
			return _beacons[index].getY();
		}
		public double zpos(int index)
		{
			return _beacons[index].getZ();
		}
		public double rpos(int index)
		{
			return _beacons[index].getD();
		}
		public void to(int k, double rx, double ry, double rz, double rr)
		{
			_beacons[k] = new PosAndDistance(rx, ry, rz, rr);
		}
	}
}

