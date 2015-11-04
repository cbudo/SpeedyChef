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
	public class SearchActivity : Activity, SearchView.IOnQueryTextListener, SearchView.IOnSuggestionListener
	{
		v7Widget.RecyclerView mRecyclerView;
		v7Widget.RecyclerView.LayoutManager mLayoutManager;
		RecipeAdapter mAdapter;
		RecipeObject mObject;
		Button filter_button;
		JsonValue jsonDoc;
		string ordertype;
		string asc;
		string mostRecentKeywords;
		private Object thisLock = new Object();


		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			this.mostRecentKeywords = "";
			this.asc = "Asc";
			this.ordertype = "Diff";

			//RECYCLER VIEW
			mObject = new RecipeObject ();
			mAdapter = new RecipeAdapter (mObject, this);
			SetContentView (Resource.Layout.Search);
			mRecyclerView = FindViewById<v7Widget.RecyclerView> (Resource.Id.recyclerView);
			mRecyclerView.SetAdapter (mAdapter);
			mLayoutManager = new v7Widget.LinearLayoutManager (this);
			mRecyclerView.SetLayoutManager (mLayoutManager);

			//SEARCH VIEW
			SearchView searchView = FindViewById<SearchView> (Resource.Id.main_search);
			searchView.SetBackgroundColor (Android.Graphics.Color.White);
			searchView.SetOnQueryTextListener ((SearchView.IOnQueryTextListener)this);
			int id = Resources.GetIdentifier ("android:id/search_src_text", null, null);
			TextView textView = (TextView)searchView.FindViewById (id);
			textView.SetTextColor (Android.Graphics.Color.Black);
			textView.SetHintTextColor (Android.Graphics.Color.Black);
			searchView.SetQueryHint ("Search Recipes...");
			LinearLayout search_container = FindViewById<LinearLayout> (Resource.Id.search_container);
			search_container.Click += (sender, e) => {
				if (searchView.Iconified != false) {
					searchView.Iconified = false;
				}
			};

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
						CachedData.Instance.ActivityContext = this.GetType();
						StartActivity (intent);
					} else if (arg1.Item.TitleFormatted.ToString () == "Plan") {
						var intent = new Intent (this, typeof(MealPlannerCalendar));
						CachedData.Instance.ActivityContext = this.GetType();
						StartActivity (intent);
					} else if (arg1.Item.TitleFormatted.ToString () == "Walkthrough") {
						var intent = new Intent (this, typeof(StepsActivity));
						CachedData.Instance.ActivityContext = this.GetType();
						StartActivity (intent);
					}
					else if (arg1.Item.TitleFormatted.ToString() == "Preferences"){
						var intent = new Intent(this, typeof(Allergens));
						CachedData.Instance.ActivityContext = this.GetType();
						StartActivity(intent);
					}
					else if (arg1.Item.TitleFormatted.ToString() == "Search"){
					}
				};
				menu.DismissEvent += (s2, arg2) => {
					menu_button.SetBackgroundResource (Resource.Drawable.menu_lines);
					Console.WriteLine ("menu dismissed");
				};
				menu.Show ();
			};

			this.filter_button = FindViewById<Button> (Resource.Id.filter_button);
			RegisterForContextMenu (filter_button);
			filter_button.Click += (s, arg) => {
				((Button) s).ShowContextMenu();
			};
				
			if(CachedData.Instance.ActivityContext == typeof(SubtypeBrowseActivity)){
				System.Diagnostics.Debug.WriteLine("Here");
				string selectionInput = CachedData.Instance.SelectedNationality + "," + CachedData.Instance.SelectedSubgenre;
				string url = "http://speedychef.azurewebsites.net/search/searchbyunion?inputKeywords=" + this.mostRecentKeywords + "&ordertype=" + this.ordertype + "&ascending=" + this.asc + "&subgenre=" + selectionInput;
				this.ProcessSingleSearchQuery (url);
			}

		}

		public override void OnBackPressed(){
			base.OnPause ();
			CachedData.Instance.ActivityContext = this.GetType ();
		}

		public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo) {
			base.OnCreateContextMenu (menu, v, menuInfo);

			MenuInflater menuInflater = new MenuInflater (this);
			menuInflater.Inflate (Resource.Menu.Filter_Menu, menu);
		}

		public override bool OnContextItemSelected (IMenuItem item)
		{
			base.OnContextItemSelected(item);
			string selectionInput = CachedData.Instance.SelectedNationality + "," + CachedData.Instance.SelectedSubgenre;
			switch (item.ItemId) {
			case Resource.Id.Time:
				{
					this.filter_button.Text = "Time";
					this.ordertype = "Time";
					if (CachedData.Instance.ActivityContext.GetType () == typeof(SubtypeBrowseActivity)) {
						string url = "http://speedychef.azurewebsites.net/search/searchbyunion?inputKeywords=" + this.mostRecentKeywords + "&ordertype=" + this.ordertype + "&ascending=" + this.asc + "&subgenre=" + selectionInput;
						this.ProcessSingleSearchQuery (url);
					} else {
						string url = "http://speedychef.azurewebsites.net/search/search?inputKeywords=" + this.mostRecentKeywords + "&ordertype=" + this.ordertype + "&ascending=" + this.asc;
						this.ProcessSingleSearchQuery (url);
					}
					break;
				}
			case Resource.Id.Difficulty:
				{
					this.filter_button.Text = "Difficulty";
					this.ordertype = "Diff";
					if (CachedData.Instance.ActivityContext.GetType () == typeof(SubtypeBrowseActivity)) {
						string url = "http://speedychef.azurewebsites.net/search/searchbyunion?inputKeywords=" + this.mostRecentKeywords + "&ordertype=" + this.ordertype + "&ascending=" + this.asc + "&subgenre=" + selectionInput;
						this.ProcessSingleSearchQuery (url);
					} else {
						string url = "http://speedychef.azurewebsites.net/search/search?inputKeywords=" + this.mostRecentKeywords + "&ordertype=" + this.ordertype + "&ascending=" + this.asc;
						this.ProcessSingleSearchQuery (url);
					}
					break;
				}
			case Resource.Id.Both:
				{
					this.filter_button.Text = "Both";
					this.ordertype = "Both";
					if (CachedData.Instance.ActivityContext.GetType () == typeof(SubtypeBrowseActivity)) {
						string url = "http://speedychef.azurewebsites.net/search/searchbyunion?inputKeywords=" + this.mostRecentKeywords + "&ordertype=" + this.ordertype + "&ascending=" + this.asc + "&subgenre=" + selectionInput;
						this.ProcessSingleSearchQuery (url);
					} else {
						string url = "http://speedychef.azurewebsites.net/search/search?inputKeywords=" + this.mostRecentKeywords + "&ordertype=" + this.ordertype + "&ascending=" + this.asc;
						this.ProcessSingleSearchQuery (url);
					}
					break;
				}
			default:
				break;
			}

			return true;
		}

		public bool OnQueryTextChange(string input)
		{
			if (CachedData.Instance.ActivityContext.GetType() == typeof(SubtypeBrowseActivity)) {
				string selectionInput = CachedData.Instance.SelectedNationality + "," + CachedData.Instance.SelectedSubgenre;
				this.mostRecentKeywords = input.Replace (" ", ",");
				string url = "http://speedychef.azurewebsites.net/search/searchbyunion?inputKeywords=" + this.mostRecentKeywords + "&ordertype=" + this.ordertype + "&ascending=" + this.asc + "&subgenre=" + selectionInput;
				System.Diagnostics.Debug.WriteLine (url);
				this.ProcessSingleSearchQuery (url);
				return true;
			} else {
				this.mostRecentKeywords = input.Replace (" ", ",");
				string url = "http://speedychef.azurewebsites.net/search/search?inputKeywords=" + this.mostRecentKeywords + "&ordertype=" + this.ordertype + "&ascending=" + this.asc;
				System.Diagnostics.Debug.WriteLine (url);
				this.ProcessSingleSearchQuery (url);
				return true;
			}
		}

		public void ProcessSingleSearchQuery(string inURL){
			lock (thisLock) {
				string url = "http://speedychef.azurewebsites.net/search/search?inputKeywords=" + this.mostRecentKeywords + "&ordertype=" + this.ordertype + "&ascending=" + this.asc;
				System.Diagnostics.Debug.WriteLine (url);

				// Create an HTTP web request using the URL:
				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (url));
				request.ContentType = "application/json";
				request.Method = "SEARCH";

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
					this.mObject.Add (new Tuple<string, string, int> (this.jsonDoc [k] ["Recname"], this.jsonDoc [k] ["Recdesc"], this.jsonDoc [k] ["Recdiff"]));
					this.mAdapter.NotifyItemInserted (k);
				}
			}
		}

		public bool OnQueryTextSubmit(string input)
		{
			if (CachedData.Instance.ActivityContext.GetType() == (typeof(SubtypeBrowseActivity))) {
				string selectionInput = CachedData.Instance.SelectedNationality + "," + CachedData.Instance.SelectedSubgenre;
				this.mostRecentKeywords = input.Replace (" ", ",");
				string url = "http://speedychef.azurewebsites.net/search/searchbyunion?inputKeywords=" + this.mostRecentKeywords + "&ordertype=" + this.ordertype + "&ascending=" + this.asc + "&subgenre=" + selectionInput;
				System.Diagnostics.Debug.WriteLine (url);
				this.ProcessSingleSearchQuery (url);
				return true;
			}  else {
				this.mostRecentKeywords = input.Replace (" ", ",");
				string url = "http://speedychef.azurewebsites.net/search/search?inputKeywords=" + this.mostRecentKeywords + "&ordertype=" + this.ordertype + "&ascending=" + this.asc;
				System.Diagnostics.Debug.WriteLine (url);
				this.ProcessSingleSearchQuery (url);
				return true;
			}
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

	public class RecipeViewHolder : v7Widget.RecyclerView.ViewHolder
	{
		public TextView LeftText { get; private set; }
		public TextView RightText { get; private set; }
		public Activity callingActivity;
		// Locate and cache view references

		public RecipeViewHolder (View itemView, Activity callingActivity) : base (itemView)
		{
			this.callingActivity = callingActivity;
			LeftText = itemView.FindViewById<TextView> (Resource.Id.textViewLeft);
			RightText = itemView.FindViewById<TextView> (Resource.Id.textViewRight);
		}
	}

	public class RecipeAdapter : v7Widget.RecyclerView.Adapter
	{
		public RecipeObject mRObject;
		public Activity callingActivity; 

		public RecipeAdapter (RecipeObject inRObject, Activity inActivity)
		{
			this.mRObject = inRObject;
			this.callingActivity = inActivity;
		}

		public override v7Widget.RecyclerView.ViewHolder
		OnCreateViewHolder (ViewGroup parent, int viewType)
		{
			View itemView = LayoutInflater.From (parent.Context).
				Inflate (Resource.Layout.TwoByTwoResultView, parent, false);
			RecipeViewHolder vh = new RecipeViewHolder (itemView, this.callingActivity);
			return vh;
		}

		public override void
		OnBindViewHolder (v7Widget.RecyclerView.ViewHolder holder, int position)
		{
			RecipeViewHolder vh = holder as RecipeViewHolder;
			Tuple<string, string, int> tupleInQuestion = mRObject.getObjectInPosition (position);
			string tempLeftText = tupleInQuestion.Item1;
			string tempRightText = tupleInQuestion.Item2;
			if (tupleInQuestion.Item3 == 5) {
				vh.LeftText.SetBackgroundColor (Android.Graphics.Color.Red);
				vh.RightText.SetBackgroundColor (Android.Graphics.Color.Red);
			} else if (tupleInQuestion.Item3 == 4) {
				vh.LeftText.SetBackgroundColor (Android.Graphics.Color.Orange);
				vh.RightText.SetBackgroundColor (Android.Graphics.Color.Orange);
			} else if (tupleInQuestion.Item3 == 3) {
				vh.LeftText.SetBackgroundColor (Android.Graphics.Color.Yellow);
				vh.RightText.SetBackgroundColor (Android.Graphics.Color.Yellow);
			}
			else if (tupleInQuestion.Item3 == 2) {
				vh.LeftText.SetBackgroundColor (Android.Graphics.Color.GreenYellow);
				vh.RightText.SetBackgroundColor (Android.Graphics.Color.GreenYellow);
			} else if (tupleInQuestion.Item3 == 1) {
				vh.LeftText.SetBackgroundColor (Android.Graphics.Color.Green);
				vh.RightText.SetBackgroundColor (Android.Graphics.Color.Green);
			}
			vh.LeftText.Text = tempLeftText;
			vh.RightText.Text = tempRightText;
		}

		public override int ItemCount
		{
			get { return mRObject.NumElements ; }
		}
	}

	public class RecipeObject
	{
		public int NumElements;
		public List<Tuple<string, string, int>> RecipeList;
		private Object thisLock = new Object();

		public RecipeObject ()
		{
			this.RecipeList = new List<Tuple<string, string, int>>();
			this.NumElements = this.RecipeList.Count;
		}

		public RecipeObject (List<Tuple<string, string, int>> inList)
		{
			this.RecipeList = inList;
			this.NumElements = this.RecipeList.Count;
		}

		public Tuple<string, string, int> getObjectInPosition(int position)
		{
			return this.RecipeList [position];
		}
			
		public void Add(Tuple<string, string, int> newTuple){
			this.RecipeList.Add (newTuple);
			this.NumElements = this.RecipeList.Count;
		}

		public void Remove(int position){
			this.RecipeList.RemoveAt (position);
			this.NumElements -= 1;
		}
	}
}