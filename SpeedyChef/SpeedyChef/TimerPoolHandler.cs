using System;
using Android.Views;
using System.Collections.Generic;
using Android.Widget;

namespace SpeedyChef
{
	public class TimerPoolHandler
	{
		private ViewGroup[] timerFrames;
		private HashSet<RecipeStepTimerHandler> timers;
		private int timerIndex;

		public TimerPoolHandler (ViewGroup[] timerFrames)
		{
			this.timerFrames = timerFrames;
			timers = new HashSet<RecipeStepTimerHandler> ();
			timerIndex = 0;
		}

		public HashSet<RecipeStepTimerHandler> getTimers() {
			return timers;
		}

		public void AddTimer(RecipeStep recipeStep, Android.Widget.TextView textView, Button button) {

			//Add timer to list
			if (timers.Add (recipeStep.timerHandler)) {
				assignTimerView (recipeStep, textView, timerFrames [timerIndex], button);
				timerIndex++;
			}

		}

		private void assignTimerView(RecipeStep recipeStep, TextView textView, ViewGroup timerFrame, Button button) {
			RecipeStepTimerHandler t = recipeStep.timerHandler;
			t.SetViews (textView, timerFrame.FindViewById<TextView> (Resource.Id.walkthrough_time),
				timerFrame.FindViewById<ProgressBar> (Resource.Id.walkthrough_bar));
			timerFrame.FindViewById<TextView> (Resource.Id.walkthrough_text).Text = recipeStep.title;
			button.Click += delegate {
				if (t.IsActive ()) {
					t.PauseTimer ();
					button.SetText (Resource.String.start);
					timerFrame.Visibility = ViewStates.Gone;
				} else {
					t.StartTimer ();
					button.SetText (Resource.String.pause);
					timerFrame.Visibility = ViewStates.Visible;
				}
			};
		}

		//TODO allow for removing inactive timers
	}
			
}

