using System;
using Android.Views;
using System.Collections.Generic;
using Android.Widget;
using Android.Support.V4.View;

namespace SpeedyChef
{
	public class TimerPoolHandler
	{
		private ViewGroup[] timerFrames;
		private RecipeStepTimerHandler[] timers;
		private int timerIndex;

		public TimerPoolHandler (ViewGroup[] timerFrames)
		{
			this.timerFrames = timerFrames;
			timers = new RecipeStepTimerHandler[timerFrames.Length];
			timerIndex = 0;
		}

		public RecipeStepTimerHandler[] getTimers() {
			return timers;
		}

	/*	public void AddTimer(RecipeStep recipeStep, Android.Widget.TextView textView, Button button) {

			//Add timer to list
			/*if (timers.Add (recipeStep.timerHandler)) {
				AssignTimerView (recipeStep, textView, timerFrames [timerIndex], button);
				timerIndex++;
			}

		}*/

		/*private void AssignTimerView(RecipeStep recipeStep, TextView textView, ViewGroup timerFrame, Button button) {
			RecipeStepTimerHandler t = recipeStep.timerHandler;
			t.SetViews (textView, timerFrame);
			timerFrame.FindViewById<TextView> (Resource.Id.walkthrough_text).Text = recipeStep.title;
			AssignButtonFunction (t, button);
		}*/

		public void AssignFragView(RecipeStepTimerHandler t, TextView textView, Button button, ViewPager vp) {
			t.SetStepView (textView);
			AssignButtonFunction (t, button, vp);
		}

		private void AssignButtonFunction(RecipeStepTimerHandler t, Button button, ViewPager vp) {
			//ViewGroup timerFrame = t.getTimerFrame ();
			button.Click += delegate {
				if (t.IsActive ()) {
					DeactivateTimer(t, button);
				} else {
					ActivateTimer(t, button);
					//ViewPager vp = ((StepsActivity)Activity).GetViewPager();
					int pos = vp.CurrentItem + 1;
					vp.SetCurrentItem (pos, true);
					/*t.StartTimer ();
					button.SetText (Resource.String.pause);
					timerFrame.Visibility = ViewStates.Visible;*/
				}
			};
		}

		private bool ActivateTimer(RecipeStepTimerHandler t, Button button) {
			if (timerIndex >= timerFrames.Length)
				return false;
			t.StartTimer ();
			button.SetText (Resource.String.pause);

			ViewGroup timerFrame = timerFrames [timerIndex];
			timers [timerIndex] = t;
			t.setTimerFrame (timerFrame);
			t.setTimerIndex (timerIndex);
			timerFrame.Visibility = ViewStates.Visible;
			Console.WriteLine ("Activating timer " + timerIndex);
			timerIndex++;
			return true;
		}

		private bool DeactivateTimer(RecipeStepTimerHandler t, Button button) {
			ViewGroup timerFrame = t.getTimerFrame ();
			t.PauseTimer ();
			button.SetText (Resource.String.start);
			Console.WriteLine ("Deactivating timer " + t.getTimerIndex());
			Console.WriteLine ("Total Timer Index: " + timerIndex);

			timerIndex--;

			//Shift all active timers down
			for (int i = t.getTimerIndex (); i < timerIndex; i++) {
				ViewGroup frame = timerFrames [i];
				timers [i] = timers [i + 1];
				timers [i].setTimerIndex (i);
				timers [i].setTimerFrame (frame);
			}
			timerFrames[timerIndex].Visibility = ViewStates.Gone;

			return true;
		}
			

		//TODO allow for removing inactive timers
	}
			
}

