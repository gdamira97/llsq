using System;
using System.Collections.Generic;
namespace questiongps
{
	public class Trilateration
	{
		public bool Trilaterationfunction(Beacon beacons)
		{
			if (beacons.size() < 4)
			{
				return false;
			}
			int rows = 3;
			double[,] m = new double[rows, 3];
			double[] b = new double[rows];

			double rx, ry, rz;
			getMatrixAndVector(beacons,rows, out m, out b);
			JacobiMethod(beacons, m, b, out rx, out ry, out rz);
			Console.WriteLine("First result: ("+rx + "; " + ry + "; " + rz+")");
			double[] coor = new double[3] { rx, ry, rz};
			test(beacons, coor, rows);
			Console.WriteLine("Second result: (" + coor[0] + "; " + coor[1] + "; " + coor[2] + ")");
			return true;
		}

		public double[] test(Beacon beacons, double[] coor,int rows)
		{
			double[] dest = new double[beacons.size()];
			double[] res = new double[beacons.size()];
			Beacon beaconr = beacons;

			int c = 0;
			while (c != 100)
			{
				for (int i = 0; i < beacons.size(); i++)
				{
					dest[i] = Math.Sqrt(Math.Pow(beaconr.xpos(i) - coor[0], 2) + Math.Pow(beaconr.ypos(i) - coor[1], 2) + Math.Pow(beaconr.zpos(i) - coor[2], 2));
					res[i] = Math.Abs(beacons.rpos(i) - dest[i]);
				}
				if (res[0] <= 1E-10 && res[1] <= 1E-10 & res[2] <= 1E-10)
				{
					c=99;
				}
				else 
				{
					for (int i = 0; i < beaconr.size(); i++)
					{
						double[,] m = new double[rows, 3];
						double x, y, z;
						double[] b = new double[rows];
						beaconr.to(i, beaconr.xpos(i), beaconr.ypos(i), beaconr.zpos(i), newr(beaconr.rpos(i), dest[i]));
						getMatrixAndVector(beaconr, rows, out m, out b);
						JacobiMethod(beaconr, m, b, out x, out y, out z);
						coor[0] = x;
						coor[1] = y;
						coor[2] = z;
					}
					Console.WriteLine(c + ": " + res[0] + " " + res[1] + " " + res[2] + " " + res[3]);
				}
				c++;
			}
			Console.WriteLine("Final: " + res[0] + " " + res[1] + " " + res[2] + " " + res[3]);

			return coor;
		}
		private double newr(double a, double b)
		{
			double r=0;
			if (a > 0 && b > 0)
			{
				r = Math.Min(a, b) + (Math.Max(a, b) - Math.Min(a, b)) / 2;
			}
			else if (a < 0 && b < 0)
			{
				r = Math.Max(a, b) + (Math.Min(a, b) - Math.Max(a, b)) / 2;
			}
			else
			{
				r = (Math.Max(a, b) - Math.Min(a, b))/2;
			}
			return r;
		}
		private void getMatrixAndVector(Beacon beacons, int rows, out double[,] m, out double[] b)
		{
			m = new double[rows,3];
			b = new double[rows];
			double x1 = beacons.xpos(0);
			double y1 = beacons.ypos(0);
			double z1 = beacons.zpos(0);
			double r1 = beacons.rpos(0);
			for (int i = 1; i < beacons.size(); i++)
			{
				double x2 = beacons.xpos(i);
				double y2 = beacons.ypos(i);
				double z2 = beacons.zpos(i);
				double r2 = beacons.rpos(i);
				m[i-1, 0] = x2 - x1;
				m[i-1, 1] = y2 - y1;
				m[i-1, 2] = z2 - z1;
				b[i-1] = (Math.Pow(r1, 2) - Math.Pow(r2, 2) + Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2) + Math.Pow(z2 - z1, 2)) / 2;
			}
		}
		private void JacobiMethod(Beacon beacons, double[,] m, double[] b, out double x, out double y, out double z)
		{
			double x1 = beacons.xpos(0);
			double y1 = beacons.ypos(0);
			double z1 = beacons.zpos(0);
			double[,] mt = new double[m.GetLength(1), m.GetLength(0)];
			double[,] multiplied = new double[mt.GetLength(0), m.GetLength(1)];
			mt = transpose(m);
			multiplied = multiply(mt, m);
			double det = determinant(multiplied);
			double[,] A = inverse(multiplied, det);
			double[,] d = multiply(A, mt);
			double[] q = multbyvector(d, b);
			x = q[0] + x1;
			y = q[1] + y1;
			z = q[2] + z1;
		}
		private double[,] transpose(double[,] m)
		{
			double[,] mt = new double[m.GetLength(1), m.GetLength(0)];
			for (int row = 0; row < mt.GetLength(0); row++)
			{
				for (int col = 0; col < mt.GetLength(1); col++)
				{
					mt[row, col] = m[col, row];
				}
			}
			return mt;
		}
		private double[,] multiply(double[,] a, double[,] b)
		{
			double temp = 0;
			double[,] multiplied = new double[a.GetLength(0), b.GetLength(1)];
			if (a.GetLength(0) == b.GetLength(1))
			{
				for (int i = 0; i < a.GetLength(0); i++)
				{
					for (int j = 0; j < b.GetLength(1); j++)
					{
						temp = 0;
						for (int k = 0; k < a.GetLength(1); k++)
						{
							temp += a[i, k] * b[k, j];
						}
						multiplied[i, j] = temp;
					}
				}
			}
			return multiplied;
		}
		private double[] add(double[] a, double[] b)
		{
			double[] added = new double[a.Length];
			if (a.Length == b.Length)
			{
				for (int i = 0; i < a.Length; i++)
				{
						added[i] = a[i]+b[i];
				}
			}
			return added;
		}
		private double[] multbyvector(double[,] m, double[] v)
		{
			double[] r = new double[v.Length];
			double temp = 0;
			for (int i = 0; i < m.GetLength(0); i++)
			{
				temp = 0;
				for (int k = 0; k < m.GetLength(1); k++)
				{
					temp += m[i, k] * v[k];
				}
				r[i] = temp;
			}
			return r;
		}
		private double determinant(double[,] array)
		{
			double[,] res = new double[array.GetLength(0), array.GetLength(1)];
			for (int i = 0; i < array.GetLength(0); i++)
			{
				for (int j = 0; j < array.GetLength(1); j++)
				{
					res[i, j] = array[i, j];
				}
			}

			double det = 0;
			double total = 0;
			double[,] tempArr = new double[array.GetLength(0) - 1, array.GetLength(1) - 1];
			if (array.GetLength(0) == 1)
			{
				det = res[0, 0];
			}
			else if (array.GetLength(0) == 2)
			{
				det = res[0, 0] * res[1, 1] - res[0, 1] * res[1, 0];
			}
			else {
				for (int i = 0; i < 1; i++)
				{
					for (int j = 0; j < res.GetLength(1); j++)
					{
						if (j % 2 != 0) res[i, j] = res[i, j] * -1;
						tempArr = minor(res, i, j);
						det += determinant(tempArr)*res[i,j];
						total = total + (det * res[i, j]);
					}
				}
			}
			return det;
		}
		private double[,] inverse(double[,] m, double d)
		{
			double[,] r = new double[m.GetLength(0), m.GetLength(1)];
			for (int i = 0; i < m.GetLength(0); i++)
			{
				for (int j = 0; j < m.GetLength(1); j++)
				{
					r[i, j] = (determinant(minor(m, i, j)) * Math.Pow(-1, i + j)) / d;
				}
			}
			return r;
		}
		private double[,] minor(double[,] m, int r, int c)
		{
			List<List<double>> arr = new List<List<double>>();
			double[,] array = new double[m.GetLength(0) - 1, m.GetLength(1) - 1];
			for (int i = 0; i < m.GetLength(0); i++)
			{
				List<double> row = new List<double>();
				for (int j = 0; j < m.GetLength(1); j++)
				{
					row.Add(m[i, j]);
				}
				arr.Add(row);
			}
			arr.RemoveAt(r);
			foreach (List<double> row in arr)
			{
				row.RemoveAt(c);
			}
			for (int j = 0; j < array.GetLength(1); j++)
			{
				int k = 0;
				foreach (List<double> row in arr)
				{
					
					array[k, j] = row[j];
					k++;
				}

			}
			return array;
		}
	}
}

