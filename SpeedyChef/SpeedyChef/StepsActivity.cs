
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

namespace SpeedyChef
{

	/*[Activity(Label = "PagerDemo", Icon = "@drawable/icon")]
	public class StepsActivity : FragmentActivity
	{
		private ViewPager viewPager;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Walkthrough);

			viewPager = FindViewById<ViewPager> (Resource.Id.walkthrough_pager);
			viewPager.Adapter = new AwesomeFragmentAdapter (SupportFragmentManager);


		}
	} */

	public class AwesomeFragmentAdapter : FragmentPagerAdapter {

		public AwesomeFragmentAdapter(Android.Support.V4.App.FragmentManager fm) : base(fm) {

		}

		public override int Count {
			get {
				return 5;
			}
		}
			

		public override Android.Support.V4.App.Fragment GetItem(int position) {
			return new AwesomeFragment ();
		}
	
	}

	public class AwesomeFragment : Android.Support.V4.App.Fragment {

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
			var view = inflater.Inflate (Resource.Layout.Step, container, false);

			return view;
		}
	}
		
	[Activity (Label = "StepsActivity")]			
	public class StepsActivity : FragmentActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Walkthrough);
			ViewPager vp = FindViewById<ViewPager> (Resource.Id.walkthrough_pager);

			RecipeStep[] steps = new RecipeStep[4];
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

			/*TextView title = FindViewById (Resource.Id.step_title);
			ImageView img = FindViewById (Resource.Id.step_image);
			TextView desc = FindViewById (Resource.Id.step_desc); */
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

			titleTv.Text = this.recipeStep.title;
			descTv.Text = this.recipeStep.desc;

			return rootView;
		}

	}
}

