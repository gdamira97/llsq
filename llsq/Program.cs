using System;
namespace questiongps
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			PosAndDistance beacon1 = new PosAndDistance(1.0, 1.0, 1.0, 1.0);
			PosAndDistance beacon2 = new PosAndDistance(3.0, 1.0, 2.0, 1.0);
			PosAndDistance beacon3 = new PosAndDistance(2.0, 2.0, 1.0, 2.0);
			PosAndDistance beacon4 = new PosAndDistance(4.0, 3.0, 2.0, 3.0);
			Beacon beacons = new Beacon(4);
			beacons.AddBeacon(beacon1);
			beacons.AddBeacon(beacon2);
			beacons.AddBeacon(beacon3);
			beacons.AddBeacon(beacon4);
			Trilateration tr = new Trilateration();
			tr.Trilaterationfunction(beacons);
		}
	}
}
