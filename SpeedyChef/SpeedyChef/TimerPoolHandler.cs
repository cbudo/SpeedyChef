using System;
using Android.Views;
using System.Collections.Generic;
using Android.Widget;

namespace SpeedyChef
{
	public class TimerPoolHandler
	{
		private ViewGroup[] timerFrames;
		private List<RecipeStepTimerHandler> timers;
		private int timerIndex;

		public TimerPoolHandler (ViewGroup[] timerFrames)
		{
			this.timerFrames = timerFrames;
			timers = new List<RecipeStepTimerHandler> ();
			timerIndex = 0;
		}

		public List<RecipeStepTimerHandler> getTimers() {
			return timers;
		}

		public void AddTimer(int seconds, Android.Widget.TextView textView, Button button) {

			//Add timer to list
			RecipeStepTimerHandler t = new RecipeStepTimerHandler (seconds, timerFrames[timerIndex], textView);
			timers.Add (t);
			timerIndex++;
			button.Click += delegate {
				if (t.IsActive ()) {
					t.PauseTimer ();
					button.SetText (Resource.String.start);
					timerFrames [timerIndex].Visibility = ViewStates.Gone;
				} else {
					t.StartTimer ();
					button.SetText (Resource.String.pause);
					timerFrames [timerIndex].Visibility = ViewStates.Visible;
				}
			};
		}

		//TODO allow for removing inactive timers
	}
			
}

