
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
		List<RecipeStepTimer> timers;

		//Called when the page is created
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Walkthrough);
			ViewPager vp = FindViewById<ViewPager> (Resource.Id.walkthrough_pager);

			timers = new List<RecipeStepTimer> ();
			//TODO replace dummy content with actual input
			steps = new RecipeStep[4];
			RecipeStep s1 = new RecipeStep();
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
			s4.timeable = false;

			steps [0] = s1;
			steps [1] = s2;
			steps [2] = s3;
			steps [3] = s4;
			vp.Adapter = new StepFragmentPagerAdapter (SupportFragmentManager, steps);

			//Set up the progress dots to appear at the bottom of the screen
			ViewGroup pd = (ViewGroup) FindViewById (Resource.Id.walkthrough_progress_dots);
			ImageView[] progressDots = new ImageView[steps.Length];
			Drawable open = Resources.GetDrawable (Resource.Drawable.circle_open);

			for (int i = 0; i < progressDots.Length; i++) {
				progressDots [i] = new ImageView (this);
				progressDots [i].SetImageDrawable (open);
				pd.AddView (progressDots [i]);
			}
			progressDots[0].SetImageDrawable (Resources.GetDrawable(Resource.Drawable.circle_closed));

			ViewGroup pbs = (ViewGroup)FindViewById (Resource.Id.walkthrough_progress_bars);

			vp.AddOnPageChangeListener (new StepChangeListener (progressDots, open, Resources.GetDrawable(Resource.Drawable.circle_closed), timers, pbs));

		}
			
	}
		

	class StepFragmentPagerAdapter : Android.Support.V4.App.FragmentStatePagerAdapter {
		private RecipeStep[] steps;
		public StepFragmentPagerAdapter (Android.Support.V4.App.FragmentManager fm, RecipeStep[] steps) : base(fm) {
			this.steps = steps;
		}

		public override Android.Support.V4.App.Fragment GetItem(int position) {
			return new StepFragment (steps [position]);
		}

		public override int Count {
			get {
				return steps.Length;
			}
		}

	}

	class StepFragment : Android.Support.V4.App.Fragment {

		private RecipeStep recipeStep;

		public StepFragment(RecipeStep s) {
			this.recipeStep = s;
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container,
			Bundle savedInstanceState) {
			ViewGroup rootView = (ViewGroup) inflater.Inflate (Resource.Layout.Step, container, false);

			TextView titleTv = (TextView) rootView.FindViewById (Resource.Id.step_title);
			ImageView imgv = (ImageView) rootView.FindViewById (Resource.Id.step_image);
			TextView descTv = (TextView) rootView.FindViewById (Resource.Id.step_desc);
			TextView timeTv;
			if (this.recipeStep.timeable) { // Add a timer
				rootView.FindViewById (Resource.Id.step_timer_wrapper).Visibility = ViewStates.Visible;
				timeTv = (TextView) rootView.FindViewById (Resource.Id.step_timer_display);
				timeTv.Text = this.recipeStep.time.ToString () + ":00";
				this.recipeStep.timer.SetTextView (timeTv);
				Button startButton = rootView.FindViewById<Button> (Resource.Id.step_timer_start_button);
				startButton.Click += delegate {
					this.recipeStep.timer.Start ();
					startButton.SetText(Resource.String.pause);
				};

			}
			else { // Don't add a timer, just display the time estimate
				rootView.FindViewById (Resource.Id.step_static_time).Visibility = ViewStates.Visible;
				timeTv = (TextView) rootView.FindViewById (Resource.Id.step_static_time);
				timeTv.Text = this.recipeStep.time.ToString() + Resources.GetString(Resource.String.minute_short);
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
		List<RecipeStepTimer> timers;
		ViewGroup progressBars;

		public StepChangeListener(ImageView[] dots, Drawable open, Drawable closed, List<RecipeStepTimer> timers, ViewGroup progressBars) : base() {
			this.dots = dots;
			this.open = open;
			this.closed = closed;
			this.timers = timers;
			this.progressBars = progressBars;
		}

		public override void OnPageSelected (int position) {
			SetUIListPos (position);
			//loadTimers ();
		}

		public void SetUIListPos(int pos) {
			dots [pos].SetImageDrawable(closed);
			if (pos < dots.Length - 1) {
				dots [pos + 1].SetImageDrawable (open);
			}
		}

		private void loadTimers() {
			for (int i = 0; i < timers.Count; i++) {
				RecipeStepTimer t = timers.ElementAt (i);
				if (t.active) {
					//TODO hardcoded for prototype, fix this later
					int id;
					if (i == 2) {
						id = Resource.Id.walkthrough_time_2;
						View v = progressBars.FindViewById (Resource.Id.walkthrough_bar_2);
						v.Visibility = ViewStates.Visible;
					} else {
						id = Resource.Id.walkthrough_time_1;
						View v = progressBars.FindViewById (Resource.Id.walkthrough_bar_1);
						v.Visibility = ViewStates.Visible;
					}
					TextView tv = progressBars.FindViewById<TextView> (id);
					t.SetTextView (tv);
				}
			}
		}
	}
}

