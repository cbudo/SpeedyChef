
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
	[Activity(Theme="@style/MyTheme",Label = "Allergens")]
    public class Allergens : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
			SetContentView(Resource.Layout.Allergens);


			Button allergens_button = FindViewById<Button> (Resource.Id.allergens_button);
			allergens_button.Click += (s, arg) => {
				var intent = new Intent(this, typeof(Allergens));
				StartActivity(intent);
			};
			Button appliances_button = FindViewById<Button> (Resource.Id.oven_button);
			appliances_button.Click += (s, arg) => {
				var intent = new Intent(this, typeof(Appliances));
				StartActivity(intent);
			};
			Button expertise_button = FindViewById<Button> (Resource.Id.expertise_button);
			expertise_button.Click += (s, arg) => {
				var intent = new Intent(this, typeof(Expertise));
				StartActivity(intent);
			};


			//MENU VIEW
			Button menu_button = FindViewById<Button> (Resource.Id.menu_button);
			menu_button.Click += (s, arg) => {
				menu_button.SetBackgroundResource(Resource.Drawable.pressed_lines);
				PopupMenu menu = new PopupMenu (this, menu_button);
				menu.Inflate (Resource.Menu.Main_Menu);
				menu.MenuItemClick += (s1, arg1) => {
					// Console.WriteLine ("{0} selected", arg1.Item.TitleFormatted);
					if (arg1.Item.TitleFormatted.ToString() == "Browse") {
						var intent = new Intent(this, typeof(BrowseNationalitiesActivity));
						StartActivity(intent);
					}
					else if (arg1.Item.TitleFormatted.ToString() == "Plan") {
						var intent = new Intent(this, typeof(MealPlannerCalendar));
						StartActivity(intent);
					}
					else if (arg1.Item.TitleFormatted.ToString() == "Walkthrough"){
						var intent = new Intent(this, typeof(StepsActivity));
						StartActivity(intent);
					}
					else if (arg1.Item.TitleFormatted.ToString() == "Search"){
						var intent = new Intent(this, typeof(SearchActivity));
						StartActivity(intent);
					}
				};
				menu.DismissEvent += (s2, arg2) => {
					menu_button.SetBackgroundResource(Resource.Drawable.menu_lines);
					Console.WriteLine ("menu dismissed");
				};
				menu.Show ();
			};
        }
    }
	[Activity(Theme="@style/MyTheme",Label = "Appliances")]
	public class Appliances : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Appliances);

			Spinner spinner = FindViewById<Spinner> (Resource.Id.spinner1);

			spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs> (spinner_ItemSelected);
			var adapter = ArrayAdapter.CreateFromResource (
				this, Resource.Array.burner_types, Android.Resource.Layout.SimpleSpinnerItem);

			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinner.Adapter = adapter;


			Button allergens_button = FindViewById<Button> (Resource.Id.allergens_button);
			allergens_button.Click += (s, arg) => {
				var intent = new Intent(this, typeof(Allergens));
				StartActivity(intent);
			};
			Button appliances_button = FindViewById<Button> (Resource.Id.oven_button);
			appliances_button.Click += (s, arg) => {
				var intent = new Intent(this, typeof(Appliances));
				StartActivity(intent);
			};
			Button expertise_button = FindViewById<Button> (Resource.Id.expertise_button);
			expertise_button.Click += (s, arg) => {
				var intent = new Intent(this, typeof(Expertise));
				StartActivity(intent);
			};

			//MENU VIEW
			Button menu_button = FindViewById<Button> (Resource.Id.menu_button);
			menu_button.Click += (s, arg) => {
				menu_button.SetBackgroundResource(Resource.Drawable.pressed_lines);
				PopupMenu menu = new PopupMenu (this, menu_button);
				menu.Inflate (Resource.Menu.Main_Menu);
				menu.MenuItemClick += (s1, arg1) => {
					// Console.WriteLine ("{0} selected", arg1.Item.TitleFormatted);
					if (arg1.Item.TitleFormatted.ToString() == "Browse") {
						var intent = new Intent(this, typeof(BrowseNationalitiesActivity));
						StartActivity(intent);
					}
					else if (arg1.Item.TitleFormatted.ToString() == "Plan") {
						var intent = new Intent(this, typeof(MealPlannerCalendar));
						StartActivity(intent);
					}
					else if (arg1.Item.TitleFormatted.ToString() == "Walkthrough"){
						var intent = new Intent(this, typeof(StepsActivity));
						StartActivity(intent);
					}
					else if (arg1.Item.TitleFormatted.ToString() == "Search"){
						var intent = new Intent(this, typeof(SearchActivity));
						StartActivity(intent);
					}
				};
				menu.DismissEvent += (s2, arg2) => {
					menu_button.SetBackgroundResource(Resource.Drawable.menu_lines);
					Console.WriteLine ("menu dismissed");
				};
				menu.Show ();
			};
		}
		private void spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			Spinner spinner = (Spinner)sender;

			string toast = string.Format ("{0}", spinner.GetItemAtPosition (e.Position));
			Toast.MakeText (this, toast, ToastLength.Long).Show ();
		}
	}
	[Activity(Theme="@style/MyTheme",Label = "Expertise")]
	public class Expertise : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Expertise);

			Button allergens_button = FindViewById<Button> (Resource.Id.allergens_button);
			allergens_button.Click += (s, arg) => {
				var intent = new Intent(this, typeof(Allergens));
				StartActivity(intent);
			};
			Button appliances_button = FindViewById<Button> (Resource.Id.oven_button);
			appliances_button.Click += (s, arg) => {
				var intent = new Intent(this, typeof(Appliances));
				StartActivity(intent);
			};
			Button expertise_button = FindViewById<Button> (Resource.Id.expertise_button);
			expertise_button.Click += (s, arg) => {
				var intent = new Intent(this, typeof(Expertise));
				StartActivity(intent);
			};
			//MENU VIEW
			Button menu_button = FindViewById<Button> (Resource.Id.menu_button);
			menu_button.Click += (s, arg) => {
				menu_button.SetBackgroundResource(Resource.Drawable.pressed_lines);
				PopupMenu menu = new PopupMenu (this, menu_button);
				menu.Inflate (Resource.Menu.Main_Menu);
				menu.MenuItemClick += (s1, arg1) => {
					// Console.WriteLine ("{0} selected", arg1.Item.TitleFormatted);
					if (arg1.Item.TitleFormatted.ToString() == "Browse") {
						var intent = new Intent(this, typeof(BrowseNationalitiesActivity));
						StartActivity(intent);
					}
					else if (arg1.Item.TitleFormatted.ToString() == "Plan") {
						var intent = new Intent(this, typeof(MealPlannerCalendar));
						StartActivity(intent);
					}
					else if (arg1.Item.TitleFormatted.ToString() == "Walkthrough"){
						var intent = new Intent(this, typeof(StepsActivity));
						StartActivity(intent);
					}
					else if (arg1.Item.TitleFormatted.ToString() == "Search"){
						var intent = new Intent(this, typeof(SearchActivity));
						StartActivity(intent);
					}
				};
				menu.DismissEvent += (s2, arg2) => {
					menu_button.SetBackgroundResource(Resource.Drawable.menu_lines);
					Console.WriteLine ("menu dismissed");
				};
				menu.Show ();
			};
		}
	}
}

