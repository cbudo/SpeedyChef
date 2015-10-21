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
	public class MainActivity : Activity, SearchView.IOnQueryTextListener, SearchView.IOnSuggestionListener
	{
		v7Widget.RecyclerView mRecyclerView;
		v7Widget.RecyclerView.LayoutManager mLayoutManager;
		TestAdapter mAdapter;
		TestObject mObject;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//RECYCLER VIEW
			mObject = new TestObject ();
			mAdapter = new TestAdapter (mObject);
			SetContentView (Resource.Layout.Main);
			mRecyclerView = FindViewById<v7Widget.RecyclerView> (Resource.Id.recyclerView);
			mRecyclerView.SetAdapter (mAdapter);
			mLayoutManager = new v7Widget.LinearLayoutManager (this);
			mRecyclerView.SetLayoutManager (mLayoutManager);

			//SEARCH VIEW
			SearchView searchView = FindViewById<SearchView> (Resource.Id.main_search);
			searchView.SetBackgroundColor (Android.Graphics.Color.White);
			searchView.SetOnQueryTextListener ((SearchView.IOnQueryTextListener) this);
			int id = Resources.GetIdentifier("android:id/search_src_text", null, null);
			TextView textView = (TextView) searchView.FindViewById(id);
			textView.SetTextColor(Android.Graphics.Color.LightSkyBlue);
			textView.SetHintTextColor (Android.Graphics.Color.LightSkyBlue);
			textView.SetHint (Resource.String.Main_Search_Hint);
			searchView.SetQueryHint ("Search Recipes...");
			LinearLayout search_container = FindViewById<LinearLayout> (Resource.Id.search_container);
			search_container.Click += (sender, e) => {
				if (searchView.Iconified != false){
					searchView.Iconified = false;
				}
			};

			//MENU VIEW
			Button menu_button = FindViewById<Button> (Resource.Id.menu_button);
			menu_button.Click += (s, arg) => {
				menu_button.SetBackgroundResource(Resource.Drawable.pressed_lines);
				PopupMenu menu = new PopupMenu (this, menu_button);
				menu.Inflate (Resource.Menu.Main_Menu);
				menu.MenuItemClick += (s1, arg1) => {
					// Console.WriteLine ("{0} selected", arg1.Item.TitleFormatted);
					if (arg1.Item.TitleFormatted.ToString() == "Search") {
						var intent = new Intent(this, typeof(MainSearchActivity));
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

		public bool OnQueryTextChange(string input)
		{
			System.Console.WriteLine (input);
			return true;
		}

		public bool OnQueryTextSubmit(string input)
		{
			System.Console.WriteLine (input);
			return true;
		}

		public bool OnSuggestionSelect (int position)
		{
			return false;
		}

		public bool OnSuggestionClick (int position)
		{
			return false;
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