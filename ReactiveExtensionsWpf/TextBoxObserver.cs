using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ReactiveExtensionsWpf
{
	public class TextBoxObserver<T> : IObserver<T>
	{
		private TextBox textBox;
		public TextBoxObserver(TextBox textBox)
		{
			this.textBox = textBox;
		}
		#region IObserver<T> Members

		public void OnCompleted() 
		{
			if (textBox != null) {
				textBox.Dispatcher.Invoke(() =>
				{
					textBox.Text = "Completed";
				});
			}
		}

		public void OnError(Exception error) 
		{
			if (textBox != null)
			{
				textBox.Dispatcher.Invoke(() =>
				{
					textBox.Text = "Error Occurred";
				});
			}
		}

		public void OnNext(T value)
		{
			if (textBox != null)
			{
				textBox.Dispatcher.Invoke(() =>
				{
					textBox.Text = value.ToString();
				});
			}
		}

		#endregion
	}
}
