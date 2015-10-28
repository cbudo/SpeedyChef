
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Graphics.Drawables;
using Android.Util;

namespace SpeedyChef
{
		
	[Activity (Label = "StepsActivity")]			
	public class StepsActivity : FragmentActivity
	{

		RecipeStep[] steps; 
		ViewGroup[] timerFrames;
		TimerPoolHandler timerPoolHandler;

		//Called when the page is created
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Walkthrough);
			ViewPager vp = FindViewById<ViewPager> (Resource.Id.walkthrough_pager);

			//Store pointers to timer frames to be referenced by the fragments
			timerFrames = new ViewGroup[5];
			timerFrames [0] = (ViewGroup) FindViewById (Resource.Id.walkthrough_frame_1);
			timerFrames [1] = (ViewGroup) FindViewById (Resource.Id.walkthrough_frame_2);
			timerFrames [2] = (ViewGroup) FindViewById (Resource.Id.walkthrough_frame_3);
			timerFrames [3] = (ViewGroup) FindViewById (Resource.Id.walkthrough_frame_4);
			timerFrames [4] = (ViewGroup) FindViewById (Resource.Id.walkthrough_frame_5);

			timerPoolHandler = new TimerPoolHandler (timerFrames);

			//TODO replace dummy content with actual input
			steps = new RecipeStep[9];
			RecipeStep s1 = new RecipeStep ();
			s1.title = "Form the Corn";
			s1.desc = "And we form the corn, form form the corn, and we form the corn, form form the corn";
			s1.time = 600;
			s1.timeable = true;
			RecipeStep s2 = new RecipeStep ();
			s2.title = "Shuck the Corn";
			s2.desc = "And we shuck the corn, shuck shuck the corn, and we shuck the corn, shuck shuck the corn";
			s2.time = 750;
			RecipeStep s3 = new RecipeStep ();
			s3.title = "Pop the Corn";
			s3.desc = "And we pop the corn, pop pop the corn, and we pop the corn, pop pop the corn";
			s3.time = 300;
			RecipeStep s4 = new RecipeStep ();
			s4.title = "Form Potatoes";
			s4.desc = "And we form potatoes, form form potatoes, and we form potatoes, form form potatoes";
			s4.time = 3600;
			s4.timeable = true;
			RecipeStep s5 = new RecipeStep ();
			s5.title = "Peel Potatoes";
			s5.desc = "And we peel potatoes, peel peel potatoes and we peel potatoes, peel peel potatoes";
			s5.time = 750;
			s5.timeable = true;
			RecipeStep s6 = new RecipeStep ();
			s6.title = "Mash Potatoes";
			s6.desc = "And we mash potatoes, mash mash potatoes and we mash potatoes, mash mash potatoes";
			s6.time = 300;
			RecipeStep s7 = new RecipeStep ();
			s7.title = "Form Bananas";
			s7.desc = "And we form bananas, form form bananas, and we form bananas, form form bananas";
			s7.time = 1800;
			RecipeStep s8 = new RecipeStep ();
			s8.title = "Peel Bananas";
			s8.desc = "And we peel bananas, peel peel bananas and we peel bananas, peel peel bananas";
			s8.time = 750;
			RecipeStep s9 = new RecipeStep ();
			s9.title = "Go Bananas";
			s9.desc = "AND WE GO BANANAS GO GO BANANAS AND WE GO BANANAS GO GO BANANAS";
			s9.time = 300;
			/*RecipeStep s1 = new RecipeStep();
			s1.title = "Boil Water";
			s1.desc = "Fill a large pot with about one quart of water. Put the pot on a burner on high. Check back in about 8 minutes.";
			s1.time = 8;
			s1.timeable = true;
			s1.timer = new RecipeStepTimer (s1.time * 60);
			timers.Add (s1.timer);
			RecipeStep s2 = new RecipeStep();
			s2.title = "Boil Potatoes";
			s2.desc = "Put the potatoes into the boiling water. Allow them to boil for 10 minutes.";
			s2.time = 10;
			s2.timeable = true;
			s2.timer = new RecipeStepTimer (s2.time * 60);
			timers.Add (s2.timer);
			RecipeStep s3 = new RecipeStep();
			s3.title = "Peel Potatoes";
			s3.desc = "Remove the potatoes from the boiling water. Wait for them to cool, or you can run them under cold water. Once they are cool enough to handle, use a peeler to remove the skin.";
			s3.time = 5;
			s3.timeable = false;
			RecipeStep s4 = new RecipeStep();
			s4.title = "Slice Potatoes";
			s4.desc = "Use a large chef's knife to slice the potatoes into thin chunks.";
			s4.time = 3;
			s4.timeable = false; */

			steps [0] = s1;
			steps [1] = s2;
			steps [2] = s3;
			steps [3] = s4;
			steps [4] = s5;
			steps [5] = s6;
			steps [6] = s7;
			steps [7] = s8;
			steps [8] = s9;

			//TODO iterate through returned JSON
			/*for (int i = 0; i < steps.Length; i++) {
				if (steps [i].timeable) {
					timers.Add (new RecipeStepTimerHandler (steps [i]));
				}
			} */

			vp.Adapter = new StepFragmentPagerAdapter (SupportFragmentManager, steps, timerPoolHandler);

			//Set up the progress dots to appear at the bottom of the screen
			ViewGroup pd = (ViewGroup) FindViewById (Resource.Id.walkthrough_progress_dots);
			ImageView[] progressDots = new ImageView[steps.Length];
			Drawable open = Resources.GetDrawable (Resource.Drawable.circle_open);

			for (int i = 0; i < progressDots.Length; i++) {
				progressDots [i] = new ImageView (this);
				progressDots [i].SetMaxWidth(30);
				progressDots [i].SetImageDrawable (open);
				pd.AddView (progressDots [i]);
			}
			progressDots[0].SetImageDrawable (Resources.GetDrawable(Resource.Drawable.circle_closed));

			ViewGroup pbs = (ViewGroup)FindViewById (Resource.Id.walkthrough_progress_bars);

			vp.AddOnPageChangeListener (new StepChangeListener (progressDots, open, Resources.GetDrawable(Resource.Drawable.circle_closed), timerPoolHandler.getTimers(), pbs));

			Console.WriteLine (WebUtils.getRecipeSteps ());

		}


			
	}
		

	class StepFragmentPagerAdapter : Android.Support.V4.App.FragmentStatePagerAdapter {
		private RecipeStep[] steps;
		private TimerPoolHandler handler;

		public StepFragmentPagerAdapter (Android.Support.V4.App.FragmentManager fm, RecipeStep[] steps, TimerPoolHandler handler) : base(fm) {
			this.steps = steps;
			this.handler = handler;
		}

		public override Android.Support.V4.App.Fragment GetItem(int position) {
			return new StepFragment (steps [position], handler);
		}

		public override int Count {
			get {
				return steps.Length;
			}
		}

	}

	class StepFragment : Android.Support.V4.App.Fragment {

		private RecipeStep recipeStep;
		private TimerPoolHandler handler;
		private bool hasTimer;

		public StepFragment(RecipeStep s, TimerPoolHandler h) {
			this.recipeStep = s;
			this.handler = h;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container,
			Bundle savedInstanceState) {
			ViewGroup rootView = (ViewGroup) inflater.Inflate (Resource.Layout.Step, container, false);

			TextView titleTv = (TextView) rootView.FindViewById (Resource.Id.step_title);
			ImageView imgv = (ImageView) rootView.FindViewById (Resource.Id.step_image);
			TextView descTv = (TextView) rootView.FindViewById (Resource.Id.step_desc);
			TextView timeTv;
			if (this.recipeStep.timeable && !hasTimer) { // Add a timer
				rootView.FindViewById (Resource.Id.step_timer_wrapper).Visibility = ViewStates.Visible;
				timeTv = (TextView) rootView.FindViewById (Resource.Id.step_timer_display);
				timeTv.Text = (this.recipeStep.time / 60).ToString () + ":00";
				Button startButton = rootView.FindViewById<Button> (Resource.Id.step_timer_start_button);
				handler.AddTimer (this.recipeStep.time, timeTv, startButton);
				hasTimer = true;
			}
			else { // Don't add a timer, just display the time estimate
				rootView.FindViewById (Resource.Id.step_static_time).Visibility = ViewStates.Visible;
				timeTv = (TextView) rootView.FindViewById (Resource.Id.step_static_time);
				timeTv.Text = (this.recipeStep.time / 60).ToString() + Resources.GetString(Resource.String.minute_short);
			}

			titleTv.Text = this.recipeStep.title;
			descTv.Text = this.recipeStep.desc;

			return rootView;
		}

	}

	class StepChangeListener : ViewPager.SimpleOnPageChangeListener {
		ImageView[] dots;
		Drawable open;
		Drawable closed;
		List<RecipeStepTimerHandler> timers;
		ViewGroup progressBars;

		public StepChangeListener(ImageView[] dots, Drawable open, Drawable closed, List<RecipeStepTimerHandler> timers, ViewGroup progressBars) : base() {
			this.dots = dots;
			this.open = open;
			this.closed = closed;
			this.timers = timers;
			this.progressBars = progressBars;
		}

		public override void OnPageSelected (int position) {
			SetUIListPos (position);
			loadTimers ();
		}

		public void SetUIListPos(int pos) {
			dots [pos].SetImageDrawable(closed);
			if (pos < dots.Length - 1) {
				dots [pos + 1].SetImageDrawable (open);
			}
		}

		private void loadTimers() {
			for (int i = 0; i < timers.Count; i++) {
				RecipeStepTimerHandler t = timers.ElementAt (i);
				if (t.IsActive()) {
					/*//TODO hardcoded for prototype, fix this later
					int id_frame;
					int id_time;
					if (i == 1) {
						id_frame = Resource.Id.walkthrough_frame_2;
						id_time = Resource.Id.walkthrough_time_2;
						View v = progressBars.FindViewById (id_frame);
						v.Visibility = ViewStates.Visible;
					} else {
						id_frame = Resource.Id.walkthrough_frame_1;
						id_time = Resource.Id.walkthrough_time_1;
						View v = progressBars.FindViewById (id_frame);
						v.Visibility = ViewStates.Visible;
					}
					TextView tv = progressBars.FindViewById<TextView> (id_time);
					t.SetTextView (tv); */
				}
			}
		}
	}
}

