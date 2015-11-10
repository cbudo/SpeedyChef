using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using v7Widget = Android.Support.V7.Widget;
using System.Collections.Generic;

using System.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace SpeedyChef
{
	[Activity (Theme="@style/MyTheme", Label = "SpeedyChef", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : CustomActivity, SearchView.IOnQueryTextListener, SearchView.IOnSuggestionListener
	{
		v7Widget.RecyclerView mRecyclerView;
		v7Widget.RecyclerView.LayoutManager mLayoutManager;
		PlannedMealAdapter mAdapter;
		PlannedMealObject mObject;
		object thisLock = new object();
		JsonValue jsonDoc;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//RECYCLER VIEW
			mObject = new PlannedMealObject ();
			mAdapter = new PlannedMealAdapter (mObject);
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
			textView.SetTextColor(Android.Graphics.Color.Black);
			textView.SetHintTextColor (Android.Graphics.Color.Black);
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
				menu.MenuItemClick += this.MenuButtonClick;
				menu.DismissEvent += (s2, arg2) => {
					menu_button.SetBackgroundResource(Resource.Drawable.menu_lines);
					Console.WriteLine ("menu dismissed");
				};
				menu.Show ();
			};

			this.GenerateUpcomingMeals ("http://speedychef.azurewebsites.net/search/GenerateUpcomingMeals?user=tester&date1=" + DateTime.Now + "&date2=" + DateTime.Now.AddDays (7.0), "GenerateUpcomingMeals");
				
		}

		protected override void OnResume(){
			base.OnResume ();
			CachedData.Instance.CurrHighLevelType = this.GetType ();
		}

		public override void OnBackPressed(){
			base.OnPause ();
			CachedData.Instance.PreviousActivity = this;
			Finish ();
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

		public void GenerateUpcomingMeals(string inURL, string inMethod){
			lock (thisLock) {
				// Create an HTTP web request using the URL:
				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (inURL));
				request.ContentType = "application/json";
				request.Method = inMethod;

				// Send the request to the server and wait for the response:
				using (WebResponse response = request.GetResponse ()) {
					// Get a stream representation of the HTTP web response:
					using (Stream stream = response.GetResponseStream ()) {
						// Use this stream to build a JSON document object:
						this.jsonDoc = JsonObject.Load (stream);
					}
				}
				int tempNum = this.mObject.NumElements;
				for (int i = this.mObject.NumElements - 1; i > -1; i--) {
					this.mObject.Remove (i);
					this.mAdapter.NotifyItemRemoved (i);
				}
				for (int k = 0; k < this.jsonDoc.Count; k++) {
					string[] separation = this.jsonDoc [k].ToString ().Split (';');
					string finalSep0 = separation[0].Remove(0, 1);
					string finalSep2 = separation[2].Remove((separation[2].Length - 1));
					Tuple<string, string, string> newTuple = new Tuple<string, string, string> (finalSep0, separation [1], finalSep2);
					this.mObject.Add (newTuple);
					this.mAdapter.NotifyItemInserted (k);
				}
			}
		}
	}

	public class PlannedMealViewHolder : v7Widget.RecyclerView.ViewHolder
	{
		public TextView LeftText { get; private set; }
		public TextView MiddleText { get; private set; }
		public TextView RightText { get; private set; }

		// Locate and cache view references
		public PlannedMealViewHolder (View itemView) : base (itemView)
		{
			LeftText = itemView.FindViewById<TextView> (Resource.Id.leftTextView);
			MiddleText = itemView.FindViewById<TextView> (Resource.Id.middleTextView);
			RightText = itemView.FindViewById<TextView> (Resource.Id.rightTextView);
		}
	}

	public class PlannedMealAdapter : v7Widget.RecyclerView.Adapter
	{
		public PlannedMealObject mPMObject;

		public PlannedMealAdapter (PlannedMealObject inPMObject)
		{
			mPMObject = inPMObject;
		}

		public override v7Widget.RecyclerView.ViewHolder
		OnCreateViewHolder (ViewGroup parent, int viewType)
		{
			View itemView = LayoutInflater.From (parent.Context).
				Inflate (Resource.Layout.LinearCardView, parent, false);
			PlannedMealViewHolder vh = new PlannedMealViewHolder (itemView);
			return vh;
		}
		public override void
		OnBindViewHolder (v7Widget.RecyclerView.ViewHolder holder, int position)
		{
			PlannedMealViewHolder vh = holder as PlannedMealViewHolder;
			Tuple<string, string, string> newTuple = mPMObject.getObjectInPosition (position);
			vh.LeftText.Text = newTuple.Item1;
			vh.MiddleText.Text = newTuple.Item2;
			vh.RightText.Text = newTuple.Item3;
		}

		public override int ItemCount
		{
			get { return mPMObject.NumElements; }
		}
	}

	public class PlannedMealObject
	{
		public int NumElements;
		public List<Tuple<string, string, string>> mealList;

		public PlannedMealObject ()
		{
			mealList = new List<Tuple<string, string, string>>();
			NumElements = mealList.Count;
		}

		public Tuple<string, string, string> getObjectInPosition(int position)
		{
			return this.mealList[position];
		}

		public void Add(Tuple<string, string, string> newTuple){
			this.mealList.Add (newTuple);
			this.NumElements = this.mealList.Count;
		}

		public void Remove(int position){
			this.mealList.RemoveAt (position);
			this.NumElements -= 1;
		}
	}
}