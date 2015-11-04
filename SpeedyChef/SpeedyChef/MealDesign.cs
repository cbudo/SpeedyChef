
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

namespace SpeedyChef
{
	[Activity (Label = "MealDesign")]			
	public class MealDesign : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.MealDesign);
			// Create your application here
			long binaryDate = Intent.GetLongExtra ("Date", 0);
			DateTime date = DateTime.FromBinary (binaryDate);
			string mealname = Intent.GetStringExtra ("Name");
			int mealId = Intent.GetIntExtra ("mealId", -1);
			int mealSize = Intent.GetIntExtra ("Mealsize", 0);
			System.Diagnostics.Debug.WriteLine (mealSize);
			// System.Diagnostics.Debug.WriteLine (mealname + " I am here");

			// Changes meal name to passed in name
			EditText mealName = FindViewById<EditText> (Resource.Id.mealName);
			if (mealname != null) {
				mealName.Text = mealname;
			}

			//MENU VIEW
			Button menu_button = FindViewById<Button> (Resource.Id.menu_button);
			menu_button.Click += (s, arg) => {
				menu_button.SetBackgroundResource (Resource.Drawable.pressed_lines);
				PopupMenu menu = new PopupMenu (this, menu_button);
				menu.Inflate (Resource.Menu.Main_Menu);
				menu.MenuItemClick += (s1, arg1) => {
					// Console.WriteLine ("{0} selected", arg1.Item.TitleFormatted);
					if (arg1.Item.TitleFormatted.ToString () == "Browse") {
						var intent = new Intent (this, typeof(BrowseNationalitiesActivity));
						StartActivity (intent);
					} else if (arg1.Item.TitleFormatted.ToString () == "Plan") {
						var intent = new Intent (this, typeof(MealPlannerCalendar));
						StartActivity (intent);
					} else if (arg1.Item.TitleFormatted.ToString () == "Walkthrough") {
						var intent = new Intent (this, typeof(StepsActivity));
						StartActivity (intent);
					}
				};
				menu.DismissEvent += (s2, arg2) => {
					menu_button.SetBackgroundResource (Resource.Drawable.menu_lines);
					Console.WriteLine ("menu dismissed");
				};
				menu.Show ();
			};
			SeekBar sb = FindViewById<SeekBar> (Resource.Id.dinerCounter);
			TextView sbt = FindViewById<TextView> (Resource.Id.seekBarCount);
			sb.Progress = mealSize;
			sbt.Text = sb.Progress.ToString ();
			sb.ProgressChanged += (sender, e) => {
				if (e.FromUser) {
					sbt.Text = e.Progress.ToString ();
				}
			};
			Window.SetSoftInputMode (SoftInput.StateAlwaysHidden);
			Button returnButton = FindViewById<Button> (Resource.Id.returnButton);
			returnButton.Click += (sender, e) => {
				Intent i = new Intent (this, typeof(MealPlannerCalendar));
				i.PutExtra ("Result", "Passing back works");
				SetResult (Result.Ok, i);
				Finish ();
			};
			Button removeButton = FindViewById<Button> (Resource.Id.removeButton);
			removeButton.Click += (object sender, EventArgs e) => {
				System.Diagnostics.Debug.WriteLine ("ClICKED");
			};
			if (mealId == -1){
				removeButton.Clickable = false;
				
			}
			LinearLayout mealsArea = FindViewById<LinearLayout> (Resource.Id.mealsArea);
			RecipeLayouts rl = new RecipeLayouts (this);
			mealsArea.AddView (rl);


		}
	}

	public class RecipeLayouts : LinearLayout
	{


		private Button removeButton;

		private Button recipeInfo;

		private int recipeId { get; set; }

		private string recipeName { get; set; }


		public RecipeLayouts (Context context) : base (context)
		{
			
			this.removeButton = new Button (context, null, Resource.Style.generalButtonStyle);
			this.recipeInfo = new Button (context, null, Resource.Style.generalButtonStyle);
			CreateLPs ();
			CreateRLPs ();
			SetPropertiesLayout ();
			SetPropertiesRemove ();
			SetPropertiesInfo ();
			this.AddView (this.removeButton);
			this.AddView (this.recipeInfo);
		}

		private void SetPropertiesLayout ()
		{
			this.Orientation = Orientation.Horizontal;
			this.SetMinimumHeight (100);
			this.SetMinimumWidth (25);
			//this.SetBackgroundColor(Android.Graphics.Color.White);
			
		}

		private void SetPropertiesRemove ()
		{
			this.removeButton.SetMinimumHeight (100);
			this.removeButton.SetMinimumWidth(25);
			this.removeButton.Text = "Remove";
			this.removeButton.SetBackgroundResource (Resource.Color.my_blue);
			this.removeButton.Gravity = GravityFlags.Center;
			this.removeButton.SetPadding (5, 5, 5, 5);
		}

		private void SetPropertiesInfo ()
		{
			this.recipeInfo.SetMinimumHeight (100);
			this.recipeInfo.SetMinimumWidth (25);
			this.recipeInfo.Gravity = GravityFlags.Center;
			this.recipeInfo.Text = "EMPTY";
		}

		private void CreateRLPs ()
		{
			LinearLayout.LayoutParams lllp = new 
				LinearLayout.LayoutParams (LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
			lllp.SetMargins (5, 5, 5, 5);
			this.removeButton.LayoutParameters = lllp;
			LinearLayout.LayoutParams llri = new 
				LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
			llri.SetMargins (5, 5, 5, 5);
			this.recipeInfo.LayoutParameters = llri;
		}


		private void CreateLPs ()
		{
			LinearLayout.LayoutParams lllp = new 
				LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
			lllp.SetMargins (5, 5, 5, 5);
			this.LayoutParameters = lllp;
		}
	}
}