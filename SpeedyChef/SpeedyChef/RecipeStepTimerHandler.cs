using System;
using Android.Widget;
using Android.Views;

namespace SpeedyChef
{
	public class RecipeStepTimerHandler
	{
		private int time;
		private RecipeStepTimer recipeStepTimer;
		private ViewGroup timerFrame;
		private int timerIndex;
		private string timerName;

		public RecipeStepTimerHandler (int t, string s)
		{
			time = t;
			timerName = s;
			recipeStepTimer = new RecipeStepTimer (t);
		}

		public void SetViews(TextView stepView, ViewGroup timerFrame) {
			recipeStepTimer.SetStepTextView (stepView);
			setTimerFrame (timerFrame);
			}

		public void SetStepView(TextView stepView) {
			recipeStepTimer.SetStepTextView (stepView);
		}

		public ViewGroup getTimerFrame() {
			return this.timerFrame;
		}

		public void setTimerFrame(ViewGroup timerFrame) {
			this.timerFrame = timerFrame;
			recipeStepTimer.SetBarTextView (timerFrame.FindViewById<TextView> (Resource.Id.walkthrough_time));
			recipeStepTimer.SetProgressBar (timerFrame.FindViewById<ProgressBar> (Resource.Id.walkthrough_bar));
			timerFrame.FindViewById<TextView> (Resource.Id.walkthrough_text).Text = this.timerName;
			recipeStepTimer.TimeUpdate ();
		}

		public int getTimerIndex() {
			return this.timerIndex;
		}

		public void setTimerIndex(int x) {
			this.timerIndex = x;
		}

		public string getTimerName() {
			return this.timerName;;
		}

		public void setTimerName(string s) {
			this.timerName = s;
		}

		public void StartTimer() {
			recipeStepTimer.activate ();
		}
			
		public void PauseTimer() {
			time = recipeStepTimer.getSecondsLeft();
			recipeStepTimer.deactivate ();
			RecipeStepTimer newTimer = new RecipeStepTimer (time);
			newTimer.SetStepTextView (recipeStepTimer.GetStepTextView ());
			newTimer.SetBarTextView (recipeStepTimer.GetBarTextView ());
			newTimer.SetProgressBar (recipeStepTimer.GetProgressBar ());
			recipeStepTimer = newTimer;
		}

		public bool IsActive() {
			return recipeStepTimer.IsActive ();
		}

	}
}

