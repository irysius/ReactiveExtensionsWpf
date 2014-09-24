using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace ReactiveExtensionsWpf
{
	public static class StaticClass
	{
		private class ContinuousAverager
		{
			private double mean;
			private int count;

			public ContinuousAverager()
			{
				mean = 0.0;
				count = 0;
			}

			public double Add(double value)
			{
				double delta = value - mean;
				mean += (delta / (double)(++count));
				return mean;
			}
		}

		public static IObservable<double> ContinuousAverage(this IObservable<double> source)
		{
			var averager = new ContinuousAverager();
			return source.Select(x => averager.Add(x));
		}

		private class ContinuousMinner
		{
			int? min;
			public int Register(int value)
			{
				if (min == null)
					min = value;

				if (value < min)
					min = value;

				return min.Value;
			}
		}

		public static IObservable<int> ContinuousMin(this IObservable<int> source)
		{
			var minner = new ContinuousMinner();
			return source.Select(x => minner.Register(x));
		}

		private class ContinuousMaxer
		{
			int? max;
			public int Register(int value)
			{
				if (max == null)
					max = value;

				if (value > max)
					max = value;

				return max.Value;
			}
		}

		public static IObservable<int> ContinuousMax(this IObservable<int> source)
		{
			var maxer = new ContinuousMaxer();
			return source.Select(x => maxer.Register(x));
		}

		private class ContinuousCounter
		{
			int counter = 0;
			public int Count()
			{
				counter++;
				return counter;
			}
		}

		public static IObservable<int> ContinuousCount(this IObservable<int> source)
		{
			var counter = new ContinuousCounter();
			return source.Select(x => counter.Count());
		}
	}
}
