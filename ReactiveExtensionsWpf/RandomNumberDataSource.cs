using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ReactiveExtensionsWpf
{
	public class RandomNumberDataSource: IObservable<int>, IDisposable
	{
		private class Monitor : IDisposable
		{
			private readonly Action dispose;
			public Monitor(Action dispose) {
				this.dispose = dispose;
			}

			public void Dispose() {
				this.dispose();
			}
		}

		Timer timer;
		Random random = new Random(12345);
		public RandomNumberDataSource(int milliseconds = 1000) {
			timer = new Timer(milliseconds);
		}

		private void timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (_observer != null && _observer.Count > 0) {
				var value = random.Next(10000);
				foreach (var obs in _observer) {
					obs.OnNext(value);
				}
			}
		}

		public void Complete() {
			timer.Elapsed -= timer_Elapsed;
			timer.Stop();

			for (int i = _observer.Count - 1; i >= 0; --i) {
				// Interesting note.  With this setup, every OnCompleted call
				// results in the monitor for each observer being disposed,
				// and subsequently removed from the list...
				_observer[i].OnCompleted();
			}
		}

		#region IObservable<int> Members
		private readonly List<IObserver<int>> _observer = new List<IObserver<int>>();
		public IDisposable Subscribe(IObserver<int> observer)
		{
			if (!timer.Enabled) {
				timer.Elapsed += timer_Elapsed;
				timer.Start();
			}
			var subscription = new Monitor(() => _observer.Remove(observer));
			_observer.Add(observer);
			return subscription;
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if (timer != null) {
				Complete();
			}
		}

		#endregion
	}
}
