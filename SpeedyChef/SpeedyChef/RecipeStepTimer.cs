using System;
using Android.OS;
using Android.Widget;

namespace SpeedyChef
{
	public class RecipeStepTimer : CountDownTimer
	{
		private TextView bar_tv;
		private TextView step_tv;
		private ProgressBar pb;

		private bool active;
		private long millisLeft;

		public RecipeStepTimer (int seconds, TextView bartv, TextView steptv, ProgressBar pb) : base (seconds * 1000, 1000)
		{
			this.bar_tv = bartv;
			this.step_tv = steptv;
			this.pb = pb;
			pb.Max = seconds;

			active = false;
			millisLeft = seconds;
		}

		public override void OnTick(long millisUntilFinished) {
			millisLeft = millisUntilFinished;
			long seconds = millisUntilFinished / 1000;
			long mins = seconds / 60;
			seconds = seconds % 60;
			string display;
			if (seconds < 10) {
				display = mins + ":0" + seconds;
			} else {
				display = mins + ":" + seconds;
			}

			UpdateTextView (display);

			if (pb != null) {
				pb.IncrementProgressBy (1);
			}
		}

		public override void OnFinish() {
			UpdateTextView ("00:00");
			active = false;
		}

		public void activate() {
			active = true;
			Start ();
		}

		public void deactivate() {
			active = false;
			Cancel ();
		}

		public bool IsActive() {
			return active;
		}

		public int getSecondsLeft() {
			return (int) this.millisLeft / 1000;
		}

		public TextView getStepTextView() {
			return step_tv;
		}

		public TextView getBarTextView() {
			return bar_tv;
		} 

		public ProgressBar getProgressBar() {
			return pb;
		}

		public void UpdateTextView(String s) {
			if (bar_tv != null) {
				bar_tv.Text = s;
			}
			if (step_tv != null) {
				step_tv.Text = s;
			}
		}
	}
}

