
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
	public class BrowseNationalitiesActivity : CustomActivity
	{
		v7Widget.RecyclerView mRecyclerView;
		v7Widget.RecyclerView.LayoutManager mLayoutManager;
		SideBySideAdapter mAdapter;
		SideBySideObject mObject;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//RECYCLER VIEW
			mObject = new SideBySideObject ();
			mAdapter = new SideBySideAdapter (mObject, this, false);
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
				menu.MenuItemClick += this.MenuButtonClick;
				menu.DismissEvent += (s2, arg2) => {
					menu_button.SetBackgroundResource(Resource.Drawable.menu_lines);
					Console.WriteLine ("menu dismissed");
				};
				menu.Show ();
			};

		}
	}

	public class SideBySideViewHolder : v7Widget.RecyclerView.ViewHolder
	{
		public ImageView LeftImage { get; private set; }
		public ImageView RightImage { get; private set; }
		public TextView LeftText { get; private set; }
		public TextView RightText { get; private set; }
		public Activity callingActivity;
		// Locate and cache view references

		public SideBySideViewHolder (View itemView, Activity callingActivity, bool searchLanding) : base (itemView)
		{
			LeftImage = itemView.FindViewById<ImageView> (Resource.Id.imageViewLeft);
			LeftText = itemView.FindViewById<TextView> (Resource.Id.textViewLeft);
			this.callingActivity = callingActivity;
			RightImage = itemView.FindViewById<ImageView> (Resource.Id.imageViewRight);
			RightText = itemView.FindViewById<TextView> (Resource.Id.textViewRight);
			if (!searchLanding) {
				this.attributeClicks ();
			} 
			else {
				this.searchClicks ();
			}
		}

		public void attributeClicks()
		{
			LeftImage.Click += (sender, e) => {
				CachedData.Instance.SelectedNationality = LeftText.Text;
				var intent = new Intent(callingActivity, typeof(SubtypeBrowseActivity));
				CachedData.Instance.ActivityContext = this.callingActivity.GetType();
				callingActivity.StartActivity(intent);
			};
			RightImage.Click += (sender, e) => {
				CachedData.Instance.SelectedNationality = RightText.Text;
				var intent = new Intent(callingActivity, typeof(SubtypeBrowseActivity));
				CachedData.Instance.ActivityContext = this.callingActivity.GetType();
				callingActivity.StartActivity(intent);
			};
		}

		public void searchClicks()
		{
			LeftImage.Click += (sender, e) => {
				CachedData.Instance.SelectedSubgenre = LeftText.Text;
				var intent = new Intent(callingActivity, typeof(SearchActivity));
				CachedData.Instance.ActivityContext = this.callingActivity.GetType();
				callingActivity.StartActivity(intent);
			};
			RightImage.Click += (sender, e) => {
				CachedData.Instance.SelectedSubgenre = RightText.Text;
				var intent = new Intent(callingActivity, typeof(SearchActivity));
				CachedData.Instance.ActivityContext = this.callingActivity.GetType();
				callingActivity.StartActivity(intent);
			};
		}
	}

	public class SideBySideAdapter : v7Widget.RecyclerView.Adapter
	{
		public SideBySideObject mSideBySideObject;
		public Activity callingActivity;
		public bool searchLanding;

		public SideBySideAdapter (SideBySideObject inSideBySideObject, Activity inActivity, bool searchLanding)
		{
			this.mSideBySideObject = inSideBySideObject;
			this.callingActivity = inActivity;
			this.searchLanding = searchLanding;
		}

		public override v7Widget.RecyclerView.ViewHolder
		OnCreateViewHolder (ViewGroup parent, int viewType)
		{
			View itemView = LayoutInflater.From (parent.Context).
				Inflate (Resource.Layout.SideBySideView, parent, false);
			SideBySideViewHolder vh = new SideBySideViewHolder (itemView, this.callingActivity, this.searchLanding);
			return vh;
		}

		public override void
		OnBindViewHolder (v7Widget.RecyclerView.ViewHolder holder, int position)
		{
			SideBySideViewHolder vh = holder as SideBySideViewHolder;
			int realPosition = position * 2;
			int tempLeftImage = mSideBySideObject.getObjectInPosition (realPosition).Item1;
			string tempLeftText = mSideBySideObject.getObjectInPosition (realPosition).Item2;
			int tempRightImage = mSideBySideObject.getObjectInPosition (realPosition + 1).Item1;
			string tempRightText = mSideBySideObject.getObjectInPosition (realPosition + 1).Item2;
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
			get { return ((mSideBySideObject.NumElements + 1) / 2) ; }
		}
	}

	public class SideBySideObject
	{
		public int NumElements;
		public Tuple<int, string>[] SideBySideArray;

		public SideBySideObject ()
		{
			SideBySideArray = new Tuple<int, string>[10];
			SideBySideArray [0] = new Tuple<int, string>(Resource.Drawable.ItalianFood, "Italian");
			SideBySideArray [1] = new Tuple<int, string>(Resource.Drawable.AmericanFood, "American");
			SideBySideArray [2] = new Tuple<int, string>(Resource.Drawable.ChineseFood, "Chinese");
			SideBySideArray [3] = new Tuple<int, string>(Resource.Drawable.MexicanFood, "Mexican");
			SideBySideArray [4] = new Tuple<int, string>(Resource.Drawable.IndianFood, "Indian");
			SideBySideArray [5] = new Tuple<int, string>(Resource.Drawable.JapaneseFood, "Japanese");
			SideBySideArray [6] = new Tuple<int, string>(Resource.Drawable.FrenchFood, "French");
			SideBySideArray [7] = new Tuple<int, string>(Resource.Drawable.ThaiFood, "Thai");
			SideBySideArray [8] = new Tuple<int, string>(Resource.Drawable.SpanishFood, "Spanish");
			SideBySideArray [9] = new Tuple<int, string>(Resource.Drawable.GreekFood, "Greek");
			NumElements = SideBySideArray.Length;
		}

		public SideBySideObject (Tuple<int, string>[] inArray)
		{
			SideBySideArray = inArray;
			NumElements = SideBySideArray.Length;
		}

		public Tuple<int, string> getObjectInPosition(int position)
		{
			if (position >= NumElements) {
				return new Tuple<int, string>(-1, null);
			}
			return this.SideBySideArray [position];
		}
	}
}

