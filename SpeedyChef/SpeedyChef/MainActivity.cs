using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using v7Widget = Android.Support.V7.Widget;
using System.Collections.Generic;

namespace SpeedyChef
{
	[Activity (Theme="@style/MyTheme", Label = "SpeedyChef", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		v7Widget.RecyclerView mRecyclerView;
		v7Widget.RecyclerView.LayoutManager mLayoutManager;
		TestAdapter mAdapter;
		TestObject mObject;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			mObject = new TestObject ();

			mAdapter = new TestAdapter (mObject);
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			mRecyclerView = FindViewById<v7Widget.RecyclerView> (Resource.Id.recyclerView);

			mRecyclerView.SetAdapter (mAdapter);

			mLayoutManager = new v7Widget.LinearLayoutManager (this);
			mRecyclerView.SetLayoutManager (mLayoutManager);

			Button menu_button = FindViewById<Button> (Resource.Id.menu_button);

			menu_button.Click += (s, arg) => {
				menu_button.SetBackgroundResource(Resource.Drawable.pressed_lines);
				PopupMenu menu = new PopupMenu (this, menu_button);
				menu.Inflate (Resource.Menu.Main_Menu);

				menu.MenuItemClick += (s1, arg1) => {
					Console.WriteLine ("{0} selected", arg1.Item.TitleFormatted);
				};

				menu.DismissEvent += (s2, arg2) => {
					menu_button.SetBackgroundResource(Resource.Drawable.menu_lines);
					Console.WriteLine ("menu dismissed");
				};

				menu.Show ();
			};
		}
	}

	public class TestViewHolder : v7Widget.RecyclerView.ViewHolder
	{
		public TextView Caption { get; private set; }
		// Locate and cache view references
		public TestViewHolder (View itemView) : base (itemView)
		{
			Caption = itemView.FindViewById<TextView> (Resource.Id.textView);
		}
	}

	public class TestAdapter : v7Widget.RecyclerView.Adapter
	{
		public TestObject mTestObject;

		public TestAdapter (TestObject inTestObject)
		{
			mTestObject = inTestObject;
		}

		public override v7Widget.RecyclerView.ViewHolder
		OnCreateViewHolder (ViewGroup parent, int viewType)
		{
			View itemView = LayoutInflater.From (parent.Context).
				Inflate (Resource.Layout.TestCardView, parent, false);
			TestViewHolder vh = new TestViewHolder (itemView);
			return vh;
		}

		public override void
		OnBindViewHolder (v7Widget.RecyclerView.ViewHolder holder, int position)
		{
			TestViewHolder vh = holder as TestViewHolder;
			vh.Caption.Text = mTestObject.getObjectInPosition(position);
		}

		public override int ItemCount
		{
			get { return mTestObject.NumElements; }
		}
	}

	public class TestObject
	{
		public int NumElements;
		public string[] CaptionArray;

		public TestObject ()
		{
			int numCaptions = 300;
			CaptionArray = new string[numCaptions];
			for (int i = 0; i < numCaptions; i++){
				CaptionArray [i] = i.ToString();
			}
			NumElements = CaptionArray.Length;
		}

		public string getObjectInPosition(int position)
		{
			return this.CaptionArray [position];
		}
	}
}


