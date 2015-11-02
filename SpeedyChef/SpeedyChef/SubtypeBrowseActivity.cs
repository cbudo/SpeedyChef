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
using v7Widget = Android.Support.V7.Widget;

namespace SpeedyChef
{
	[Activity (Theme="@style/MyTheme", Label = "Browse Nationalities", Icon = "@drawable/icon")]
	public class SubtypeBrowseActivity : Activity
	{
		v7Widget.RecyclerView mRecyclerView;
		v7Widget.RecyclerView.LayoutManager mLayoutManager;
		SideBySideAdapter mAdapter;
		SideBySideObject mObject;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//RECYCLER VIEW
			mObject = new SideBySideObject (CachedData.Instance.TupleDict[CachedData.Instance.SelectedNationality]);
			mAdapter = new SideBySideAdapter (mObject, this, true);
			SetContentView (Resource.Layout.BrowseNationalities);
			mRecyclerView = FindViewById<v7Widget.RecyclerView> (Resource.Id.recyclerView);
			mRecyclerView.SetAdapter (mAdapter);
			mLayoutManager = new v7Widget.LinearLayoutManager (this);
			mRecyclerView.SetLayoutManager (mLayoutManager);

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
					else if (arg1.Item.TitleFormatted.ToString() == "Preferences"){
						var intent = new Intent(this, typeof(Allergens));
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

