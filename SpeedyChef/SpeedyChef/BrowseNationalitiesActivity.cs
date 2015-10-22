
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
	public class BrowseNationalitiesActivity : Activity
	{
		v7Widget.RecyclerView mRecyclerView;
		v7Widget.RecyclerView.LayoutManager mLayoutManager;
		NationalityAdapter mAdapter;
		NationalityObject mObject;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//RECYCLER VIEW
			mObject = new NationalityObject ();
			mAdapter = new NationalityAdapter (mObject);
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
				};
				menu.DismissEvent += (s2, arg2) => {
					menu_button.SetBackgroundResource(Resource.Drawable.menu_lines);
					Console.WriteLine ("menu dismissed");
				};
				menu.Show ();
			};

		}
	}

	public class NationalityViewHolder : v7Widget.RecyclerView.ViewHolder
	{
		public TextView LeftText { get; private set; }
		public TextView RightText { get; private set; }
		// Locate and cache view references

		public NationalityViewHolder (View itemView) : base (itemView)
		{
			LeftText = itemView.FindViewById<TextView> (Resource.Id.textViewLeft);
			RightText = itemView.FindViewById<TextView> (Resource.Id.textViewRight);
		}
	}

	public class NationalityAdapter : v7Widget.RecyclerView.Adapter
	{
		public NationalityObject mNationalityObject;

		public NationalityAdapter (NationalityObject inNationalityObject)
		{
			mNationalityObject = inNationalityObject;
		}

		public override v7Widget.RecyclerView.ViewHolder
		OnCreateViewHolder (ViewGroup parent, int viewType)
		{
			View itemView = LayoutInflater.From (parent.Context).
				Inflate (Resource.Layout.SideBySideView, parent, false);
			NationalityViewHolder vh = new NationalityViewHolder (itemView);
			return vh;
		}
		public override void
		OnBindViewHolder (v7Widget.RecyclerView.ViewHolder holder, int position)
		{
			NationalityViewHolder vh = holder as NationalityViewHolder;
			int realPosition = position * 2;
			string tempLeft = mNationalityObject.getObjectInPosition (realPosition);
			string tempRight = mNationalityObject.getObjectInPosition (realPosition + 1);
			if (tempLeft == null){
				tempLeft = "";
			}
			if (tempRight == null) {
				tempRight = "";
			}
			vh.LeftText.Text = tempLeft;
			vh.RightText.Text = tempRight;
		}

		public override int ItemCount
		{
			get { return ((mNationalityObject.NumElements + 1) / 2) ; }
		}
	}

	public class NationalityObject
	{
		public int NumElements;
		public string[] NationalityArray;

		public NationalityObject ()
		{
			NationalityArray = new string[11];
			NationalityArray [0] = "hello";
			NationalityArray [1] = "this";
			NationalityArray [2] = "is";
			NationalityArray [3] = "a";
			NationalityArray [4] = "test";
			NationalityArray [5] = "six";
			NationalityArray [6] = "seven";
			NationalityArray [7] = "eight";
			NationalityArray [8] = "nine";
			NationalityArray [9] = "ten";
			NationalityArray [10] = "eleven";
			NumElements = NationalityArray.Length;
		}

		public string getObjectInPosition(int position)
		{
			if (position >= NumElements) {
				return null;
			}
			return this.NationalityArray [position];
		}
	}
}

