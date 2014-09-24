using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reactive.Linq;

namespace ReactiveExtensionsWpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		RandomNumberDataSource ds;
		bool isSubscribing;
		public MainWindow()
		{
			InitializeComponent();
			lblCounter1.Content = "Number of Ticks";
			lblCounter2.Content = "Current Value";
			lblCounter3.Content = "Max Value";
			lblCounter4.Content = "Min Value";
			lblCounter5.Content = "Average Value";
			lblCounter6.Content = "Values Above 7500";
			lblCounter7.Content = "Delayed by 5 seconds";
			lblCounter8.Content = "Running Total";
			lblCounter9.Content = "";
			ds = new RandomNumberDataSource(2000);
		}
		
		private void btnSubscribe_Click(object sender, RoutedEventArgs e)
		{
			if (!isSubscribing)
			{
				Subscribe();
				isSubscribing = true;
				btnSubscribe.Content = "Stop Subscription";
			}
			else 
			{
				Unsubscribe();
				isSubscribing = false;
				btnSubscribe.Content = "Begin Subscription";
			}
		}

		private void Subscribe() 
		{
			// Number of Ticks
			ds.ContinuousCount()
				.Subscribe(new TextBoxObserver<int>(txtCounter1));

			// Current Value
			ds.Subscribe(new TextBoxObserver<int>(txtCounter2));

			// Max Value
			ds.ContinuousMax()
				.Subscribe(new TextBoxObserver<int>(txtCounter3));

			// Min Value
			ds.ContinuousMin()
				.Subscribe(new TextBoxObserver<int>(txtCounter4));

			// Average Value
			ds.Select(i => (double)i)
				.ContinuousAverage()
				.Subscribe(new TextBoxObserver<double>(txtCounter5));

			// Values above 7500
			ds.Where(i => i > 7500)
				.Subscribe(new TextBoxObserver<int>(txtCounter6));

			// Delay by 5 seconds
			ds.Delay(TimeSpan.FromSeconds(5))
				.Subscribe(new TextBoxObserver<int>(txtCounter7));

			// Running Total
			ds.Scan((i, j) => i + j)
				.Subscribe(new TextBoxObserver<int>(txtCounter8));

			// Groups Results in 5 seconds chunks
			//ds.Buffer(TimeSpan.FromSeconds(5))
			//	.Select(l => l.FirstOrDefault())
			//	.Subscribe(new TextBoxObserver<int>(txtCounter4));
		}

		private void Unsubscribe() {
			// Completes all observations
			ds.Complete();
		}
	}
}
