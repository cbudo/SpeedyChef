
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
		public ImageView LeftImage { get; private set; }
		public ImageView RightImage { get; private set; }
		public TextView LeftText { get; private set; }
		public TextView RightText { get; private set; }
		// Locate and cache view references

		public NationalityViewHolder (View itemView) : base (itemView)
		{
			LeftImage = itemView.FindViewById<ImageView> (Resource.Id.imageViewLeft);
			LeftText = itemView.FindViewById<TextView> (Resource.Id.textViewLeft);
			RightImage = itemView.FindViewById<ImageView> (Resource.Id.imageViewRight);
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
			int tempLeftImage = mNationalityObject.getObjectInPosition (realPosition).Item1;
			string tempLeftText = mNationalityObject.getObjectInPosition (realPosition).Item2;
			int tempRightImage = mNationalityObject.getObjectInPosition (realPosition + 1).Item1;
			string tempRightText = mNationalityObject.getObjectInPosition (realPosition + 1).Item2;
			if (tempLeftImage != -1){
				vh.LeftImage.SetImageResource(tempLeftImage);
				vh.LeftText.Text = tempLeftText;
			}
			if (tempRightImage != -1) {
				vh.RightImage.SetImageResource(tempRightImage);
				vh.RightText.Text = tempRightText;
			}
		}

		public override int ItemCount
		{
			get { return ((mNationalityObject.NumElements + 1) / 2) ; }
		}
	}

	public class NationalityObject
	{
		public int NumElements;
		public Tuple<int, string>[] NationalityArray;

		public NationalityObject ()
		{
			NationalityArray = new Tuple<int, string>[10];
			NationalityArray [0] = new Tuple<int, string>(Resource.Drawable.ItalianFood, "Italian");
			NationalityArray [1] = new Tuple<int, string>(Resource.Drawable.AmericanFood, "American");
			NationalityArray [2] = new Tuple<int, string>(Resource.Drawable.ChineseFood, "Chinese");
			NationalityArray [3] = new Tuple<int, string>(Resource.Drawable.MexicanFood, "Mexican");
			NationalityArray [4] = new Tuple<int, string>(Resource.Drawable.IndianFood, "Indian");
			NationalityArray [5] = new Tuple<int, string>(Resource.Drawable.JapaneseFood, "Japanese");
			NationalityArray [6] = new Tuple<int, string>(Resource.Drawable.FrenchFood, "French");
			NationalityArray [7] = new Tuple<int, string>(Resource.Drawable.ThaiFood, "Thai");
			NationalityArray [8] = new Tuple<int, string>(Resource.Drawable.SpanishFood, "Spanish");
			NationalityArray [9] = new Tuple<int, string>(Resource.Drawable.GreekFood, "Greek");
			NumElements = NationalityArray.Length;
		}

		public Tuple<int, string> getObjectInPosition(int position)
		{
			if (position >= NumElements) {
				return new Tuple<int, string>(-1, null);
			}
			return this.NationalityArray [position];
		}
	}
}

