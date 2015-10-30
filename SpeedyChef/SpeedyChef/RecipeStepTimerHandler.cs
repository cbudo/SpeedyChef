using System;
using Android.Widget;
using Android.Views;

namespace SpeedyChef
{
	public class RecipeStepTimerHandler
	{
		private int time;
		private RecipeStepTimer recipeStepTimer;
	//	private ViewGroup timerView;
	//	private TextView original;

		public RecipeStepTimerHandler (int t)
		{
			time = t;
			recipeStepTimer = new RecipeStepTimer (t);
			//timerView = v;
			//original = tv;
			//recipeStepTimer = new RecipeStepTimer (time, timerView.FindViewById<TextView> (Resource.Id.walkthrough_time), original, v.FindViewById<ProgressBar> (Resource.Id.walkthrough_bar));
		}

		public void SetViews(TextView stepView, TextView barView, ProgressBar progressBar) {
			recipeStepTimer.SetStepTextView (stepView);
			recipeStepTimer.SetBarTextView (barView);
			recipeStepTimer.SetProgressBar (progressBar);
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

