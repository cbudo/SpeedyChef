
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
			//TextView text = FindViewById<TextView> (Resource.Id.debug);
			//text.Text = date.ToString ();

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
			sb.ProgressChanged += (sender, e) => {
				if (e.FromUser) {
					sbt.Text = e.Progress.ToString();
				}
			};
			Window.SetSoftInputMode (SoftInput.StateAlwaysHidden);
			Button returnButton = FindViewById<Button> (Resource.Id.returnButton);
			returnButton.Click += (sender, e) => {
				Intent i = new Intent (this, typeof(MealPlannerCalendar));
				i.PutExtra ("Result", "Passing back works");
				SetResult (Result.Ok, i);
				Console.WriteLine ("Made it here");
				Finish ();
			};
		}



	}
}

