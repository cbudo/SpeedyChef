using System;
using Android.OS;
using Android.Widget;

namespace SpeedyChef
{
	public class RecipeStepTimer : CountDownTimer
	{
		private TextView tv;

		public bool active;

		public RecipeStepTimer (int seconds) : base (seconds * 1000, 1000)
		{
		}

		public override void OnTick(long millisUntilFinished) {
			long seconds = millisUntilFinished / 1000;
			long mins = seconds / 60;
			seconds = seconds % 60;
			if (tv != null) {
				if (seconds < 10) {
					tv.Text = (mins + ":0" + seconds);
				} else {
					tv.Text = (mins + ":" + seconds);
				}
			}
		}

		public override void OnFinish() {
			tv.Text = "00:00";
			active = false;
		}

		public void SetTextView(TextView tv) {
			this.tv = tv;
		}
			
	}
}

