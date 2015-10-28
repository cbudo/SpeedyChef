using System;
using Android.Widget;
using Android.Views;

namespace SpeedyChef
{
	public class RecipeStepTimerHandler
	{
		private int time;
		private RecipeStepTimer recipeStepTimer;
		private ViewGroup timerView;
		private TextView original;

		public RecipeStepTimerHandler (int t, ViewGroup v, TextView tv)
		{
			time = t;
			timerView = v;
			original = tv;
			recipeStepTimer = new RecipeStepTimer (time, timerView.FindViewById<TextView> (Resource.Id.walkthrough_time), original, v.FindViewById<ProgressBar> (Resource.Id.walkthrough_bar));
		}

		public void StartTimer() {
			recipeStepTimer.activate ();
		}
			
		public void PauseTimer() {
			time = recipeStepTimer.getSecondsLeft();
			recipeStepTimer.deactivate ();
			RecipeStepTimer newTimer = new RecipeStepTimer (time, recipeStepTimer.getBarTextView (), recipeStepTimer.getStepTextView (), recipeStepTimer.getProgressBar ());
			recipeStepTimer = newTimer;
		}

		public bool IsActive() {
			return recipeStepTimer.IsActive ();
		}

	}
}

