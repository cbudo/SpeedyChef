
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

namespace SpeedyChef
{
		
	[Activity (Label = "StepsActivity")]			
	public class StepsActivity : FragmentActivity
	{

		RecipeStep[] steps; 

		//Called when the page is created
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Walkthrough);
			ViewPager vp = FindViewById<ViewPager> (Resource.Id.walkthrough_pager);

			//TODO replace dummy content with actual input
			steps = new RecipeStep[4];
			RecipeStep s1 = new RecipeStep();
			s1.title = "Step 1";
			s1.desc = "DESCRIPTIVE CONTENT 11111";
			RecipeStep s2 = new RecipeStep();
			s2.title = "Step 2";
			s2.desc = "DESCRIPTIVE CONTENT 22222";
			RecipeStep s3 = new RecipeStep();
			s3.title = "Step 3";
			s3.desc = "DESCRIPTIVE CONTENT 33333";
			RecipeStep s4 = new RecipeStep();
			s4.title = "Step 4";
			s4.desc = "DESCRIPTIVE CONTENT 44444";

			steps [0] = s1;
			steps [1] = s2;
			steps [2] = s3;
			steps [3] = s4;

			vp.Adapter = new StepFragmentPagerAdapter (SupportFragmentManager, steps);

			//This sets up the progress dots to appear at the bottom of the screen
			ViewGroup pd = (ViewGroup) FindViewById (Resource.Id.progress_dots);
			ImageView[] progressDots = new ImageView[steps.Length];
			Drawable open = Resources.GetDrawable (Resource.Drawable.circle_open);

			for (int i = 0; i < progressDots.Length; i++) {
				progressDots [i] = new ImageView (this);
				progressDots [i].SetImageDrawable (open);
				pd.AddView (progressDots [i]);
			}
			progressDots[0].SetImageDrawable (Resources.GetDrawable(Resource.Drawable.circle_closed));

			vp.AddOnPageChangeListener (new StepChangeListener (progressDots, open, Resources.GetDrawable(Resource.Drawable.circle_closed)));
		}
			
	}

	/*class ProgressDots
	{
		ImageView[] dots;
		Drawable open;
		Drawable closed;

		public ProgressDots (ViewGroup view, int num, Drawable open, Drawable closed)
		{
			
		}

		//Updates the progress bar at the bottom to show the currently selected page
		public void SetUIListPos(int pos) {
			dots [pos].SetImageResource (closed);
			if (pos < dots.Length - 1) {
				dots [pos + 1].SetImageResource (open);
			}
		}
	}*/

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

			titleTv.Text = this.recipeStep.title;
			descTv.Text = this.recipeStep.desc;

			return rootView;
		}

	}

	class StepChangeListener : ViewPager.SimpleOnPageChangeListener {
		ImageView[] dots;
		Drawable open;
		Drawable closed;

		public StepChangeListener(ImageView[] dots, Drawable open, Drawable closed) : base() {
			this.dots = dots;
			this.open = open;
			this.closed = closed;
		}

		public override void OnPageSelected (int position) {
			SetUIListPos (position);
		}

		public void SetUIListPos(int pos) {
			dots [pos].SetImageDrawable(closed);
			if (pos < dots.Length - 1) {
				dots [pos + 1].SetImageDrawable (open);
			}
		}
	}
}

