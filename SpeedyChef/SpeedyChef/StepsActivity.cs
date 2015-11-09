
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
using Java.Lang;

namespace SpeedyChef
{

	[Activity (Theme="@style/MyTheme", Label = "StepsActivity", Icon = "@drawable/icon")]			
	public class StepsActivity : CustomActivity
	{

		RecipeStep[] steps; 
		ViewGroup[] timerFrames;
		TimerPoolHandler timerPoolHandler;
		ViewPager vp;
		int fragmentCount;

		//Called when the page is created
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			int mealId = Intent.GetIntExtra ("mealId", 0);
			Console.WriteLine ("Recipe Id: " + mealId);

			SetContentView (Resource.Layout.Walkthrough);
			vp = FindViewById<ViewPager> (Resource.Id.walkthrough_pager);

			//Store pointers to timer frames to be referenced by the fragments
			timerFrames = new ViewGroup[5];
			timerFrames [0] = (ViewGroup) FindViewById (Resource.Id.walkthrough_frame_1);
			timerFrames [1] = (ViewGroup) FindViewById (Resource.Id.walkthrough_frame_2);
			timerFrames [2] = (ViewGroup) FindViewById (Resource.Id.walkthrough_frame_3);
			timerFrames [3] = (ViewGroup) FindViewById (Resource.Id.walkthrough_frame_4);
			timerFrames [4] = (ViewGroup) FindViewById (Resource.Id.walkthrough_frame_5);

			timerPoolHandler = new TimerPoolHandler (timerFrames);

			//TODO fix
			steps = WebUtils.getRecipeSteps (mealId);
			fragmentCount = steps.Length + 1;

			vp.Adapter = new StepFragmentPagerAdapter (SupportFragmentManager, steps, timerPoolHandler);

			//Set up the progress dots to appear at the bottom of the screen
			ViewGroup pd = (ViewGroup) FindViewById (Resource.Id.walkthrough_progress_dots);
			NavDot[] progressDots = new NavDot[fragmentCount];
			Drawable open = Resources.GetDrawable (Resource.Drawable.circle_open);

			for (int i = 0; i < progressDots.Length; i++) {
				NavDot dot = new NavDot (this);
				dot.SetMaxWidth(30);
				dot.SetImageDrawable (open);
				pd.AddView (dot);
				dot.Num = i;
				dot.Click += delegate {
					Console.WriteLine("Going to page " + dot.Num);
					vp.SetCurrentItem (dot.Num, true);
				};
				progressDots [i] = dot;
			}
			progressDots[0].SetImageDrawable (Resources.GetDrawable(Resource.Drawable.circle_closed));

			ViewGroup pbs = (ViewGroup)FindViewById (Resource.Id.walkthrough_progress_bars);

			vp.AddOnPageChangeListener (new StepChangeListener (progressDots, open, Resources.GetDrawable(Resource.Drawable.circle_closed), pbs));

		}

		protected override void OnResume(){
			base.OnResume ();
			CachedData.Instance.CurrHighLevelType = this.GetType ();
		}

		public override void OnBackPressed(){
			base.OnPause ();
			CachedData.Instance.PreviousActivity = this;
			Finish ();
		}

		public ViewPager GetViewPager() {
			return vp;
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
			if (position < steps.Length)
				return new StepFragment (steps [position], handler);
			return new FinishedFragment ();
		}

		public override int Count {
			get {
				return steps.Length + 1;
			}
		}

	}

	//Fragment to display an individual recipe step
	class StepFragment : Android.Support.V4.App.Fragment {

		private RecipeStep recipeStep;
		private TimerPoolHandler handler;

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
			if (this.recipeStep.timeable) { // Add a timer
				rootView.FindViewById (Resource.Id.step_timer_wrapper).Visibility = ViewStates.Visible;
				timeTv = (TextView) rootView.FindViewById (Resource.Id.step_timer_display);
				Button startButton = rootView.FindViewById<Button> (Resource.Id.step_timer_start_button);
				if (this.recipeStep.timerHandler.IsActive ()) {
					startButton.SetText (Resource.String.pause);
				} else {
					timeTv.Text = (this.recipeStep.time / 60).ToString () + ":00";

				}
				handler.AssignFragView (this.recipeStep.timerHandler, timeTv, startButton, ((StepsActivity)Activity).GetViewPager());

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

	//Fragment to display when the user has gone through all of the steps
	class FinishedFragment : Android.Support.V4.App.Fragment {

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container,
			Bundle savedInstanceState) {
			View view = inflater.Inflate (Resource.Layout.FinalStep, container, false);
			Button finishButton = view.FindViewById<Button> (Resource.Id.walkthrough_finish_button);
			finishButton.Click += delegate {
				CachedData.Instance.PreviousActivity = new StepsActivity();
				Activity.Finish();
			};
			return view;
		}
	}

	class StepChangeListener : ViewPager.SimpleOnPageChangeListener {
		NavDot[] dots;
		Drawable open;
		Drawable closed;
		ViewGroup progressBars;
		int selected;

		public StepChangeListener(NavDot[] dots, Drawable open, Drawable closed, ViewGroup progressBars) : base() {
			this.dots = dots;
			this.open = open;
			this.closed = closed;
			this.progressBars = progressBars;
			this.selected = 0;
		}

		public override void OnPageSelected (int position) {
			dots [selected].SetImageDrawable (open);
			selected = position;
			dots [position].SetImageDrawable (closed);
		}

		/*public void SetUIListPos(int old, int new) {
			dots [pos].SetImageDrawable(closed);
			/*if (pos < dots.Length - 1) {
				dots [pos + 1].SetImageDrawable (open);
			}
		}*/

		/*(private void loadTimers() {
			for (int i = 0; i < timers.Count; i++) {
				RecipeStepTimerHandler t = timers.ElementAt (i);
				if (t.IsActive()) {
					//TODO hardcoded for prototype, fix this later
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
					t.SetTextView (tv); 
				}
			}
		}*/
	}

	class NavDot : ImageView {
		public int Num;

		public NavDot(Context c) : base(c) {

		}
	}
}

