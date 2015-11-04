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
		PlannedMealAdapter mAdapter;
		PlannedMealObject mObject;

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
				menu.MenuItemClick += (s1, arg1) => {
					// Console.WriteLine ("{0} selected", arg1.Item.TitleFormatted);
					if (arg1.Item.TitleFormatted.ToString() == "Browse") {
						var intent = new Intent(this, typeof(BrowseNationalitiesActivity));
						CachedData.Instance.ActivityContext = this.GetType();
						StartActivity(intent);
					}
					else if (arg1.Item.TitleFormatted.ToString() == "Plan") {
						var intent = new Intent(this, typeof(MealPlannerCalendar));
						CachedData.Instance.ActivityContext = this.GetType();
						StartActivity(intent);
					}
					else if (arg1.Item.TitleFormatted.ToString() == "Walkthrough"){
						var intent = new Intent(this, typeof(StepsActivity));

						CachedData.Instance.ActivityContext = this.GetType();

						intent.PutExtra("AgendaId", 1);

						StartActivity(intent);
					}
					else if (arg1.Item.TitleFormatted.ToString() == "Search"){
						var intent = new Intent(this, typeof(SearchActivity));
						CachedData.Instance.ActivityContext = this.GetType();
						StartActivity(intent);
					}
					else if (arg1.Item.TitleFormatted.ToString() == "Preferences"){
						var intent = new Intent(this, typeof(Allergens));
						CachedData.Instance.ActivityContext = this.GetType();
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

	public class PlannedMealViewHolder : v7Widget.RecyclerView.ViewHolder
	{
		public TextView mealDescription { get; private set; }
		// Locate and cache view references
		public PlannedMealViewHolder (View itemView) : base (itemView)
		{
			mealDescription = itemView.FindViewById<TextView> (Resource.Id.textView);
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
			vh.mealDescription.Text = mPMObject.getObjectInPosition(position);
		}

		public override int ItemCount
		{
			get { return mPMObject.NumElements; }
		}
	}

	public class PlannedMealObject
	{
		public int NumElements;
		public string[] mealArray;

		public PlannedMealObject ()
		{
			mealArray = new string[5];
			mealArray [0] = "10/28 Mom's Spaghetti";
			mealArray [1] = "10/30 Halloween Cake w/ Candy Corn";
			mealArray [2] = "10/31 All Saints Day Omelette";
			mealArray [3] = "11/2 Wedding Present (Brownies)";
			mealArray [4] = "11/3 Uncle Chuck's Chicken Noodle Soup";
			NumElements = mealArray.Length;
		}

		public string getObjectInPosition(int position)
		{
			return this.mealArray [position];
		}
	}

	public class CachedData
	{

		public Tuple<int, string>[] ItalianFood { get; set; }
		public Tuple<int, string>[] ChineseFood { get; set; }
		public Tuple<int, string>[] FrenchFood { get; set; }
		public Tuple<int, string>[] SpanishFood { get; set; }
		public Tuple<int, string>[] MexicanFood { get; set; }
		public Tuple<int, string>[] GreekFood { get; set; }
		public Tuple<int, string>[] ThaiFood { get; set; }
		public Tuple<int, string>[] IndianFood { get; set; }
		public Tuple<int, string>[] JapaneseFood { get; set; }
		public Tuple<int, string>[] AmericanFood { get; set; }
		public Dictionary<string, Tuple<int, string>[]> TupleDict { get; set; }
		public string SelectedNationality { get; set; }
		public string SelectedSubgenre { get; set; }
		public System.Type ActivityContext { get; set; }

		private CachedData()
		{
			ItalianFood = new Tuple<int, string>[4];
			TupleDict = new Dictionary<string, Tuple<int, string>[]> ();
			TupleDict.Add ("Italian", ItalianFood);
			ItalianFood [0] = new Tuple<int, string> (Resource.Drawable.ItalianDesserts, "Italian Desserts");
			ItalianFood [1] = new Tuple<int, string> (Resource.Drawable.ItalianMeats, "Italian Meats");
			ItalianFood [2] = new Tuple<int, string> (Resource.Drawable.ItalianVegetables, "Italian Vegetables");
			ItalianFood [3] = new Tuple<int, string>(Resource.Drawable.ItalianPastas, "Italian Pastas");

			ChineseFood = new Tuple<int, string>[4];
			TupleDict.Add ("Chinese", ChineseFood);
			ChineseFood [0] = new Tuple<int, string> (Resource.Drawable.ChineseDesserts, "Chinese Desserts");
			ChineseFood [1] = new Tuple<int, string> (Resource.Drawable.ChineseMeats, "Chinese Meats"); 
			ChineseFood [2] = new Tuple<int, string> (Resource.Drawable.ChineseVegetables, "Chinese Vegetables");
			ChineseFood [3] = new Tuple<int, string> (Resource.Drawable.ChineseRice, "Chinese Rice");

			FrenchFood = new Tuple<int, string>[4];
			TupleDict.Add ("French", FrenchFood);
			FrenchFood[0] = new Tuple<int, string> (Resource.Drawable.FrenchDesserts, "French Desserts");
			FrenchFood[1] = new Tuple<int, string> (Resource.Drawable.FrenchMeats, "French Meats");
			FrenchFood[2] = new Tuple<int, string> (Resource.Drawable.FrenchVegetables, "French Vegetables");
			FrenchFood[3] = new Tuple<int, string> (Resource.Drawable.FrenchSnails, "French Snails");

			SpanishFood = new Tuple<int, string>[4];
			TupleDict.Add ("Spanish", SpanishFood);
			SpanishFood[0] = new Tuple<int, string> (Resource.Drawable.SpanishDesserts, "Spanish Desserts"); 
			SpanishFood[1] = new Tuple<int, string> (Resource.Drawable.SpanishMeats, "Spanish Meats");
			SpanishFood[2] = new Tuple<int, string> (Resource.Drawable.SpanishVegetables, "Spanish Vegetables");
			SpanishFood[3] = new Tuple<int, string> (Resource.Drawable.SpanishOmelettes, "Spanish Omelettes");

			MexicanFood = new Tuple<int, string>[4];
			TupleDict.Add ("Mexican", MexicanFood);
			MexicanFood[0] = new Tuple<int, string> (Resource.Drawable.MexicanDesserts, "Mexican Desserts");
			MexicanFood[1] = new Tuple<int, string> (Resource.Drawable.MexicanMeats, "Mexican Meats");
			MexicanFood[2] = new Tuple<int, string> (Resource.Drawable.MexicanVegetables, "Mexican Vegetables");
			MexicanFood[3] = new Tuple<int, string> (Resource.Drawable.MexicanWraps, "Mexican Wraps");

			AmericanFood = new Tuple<int, string>[4];
			TupleDict.Add ("American", AmericanFood);
			AmericanFood[0] = new Tuple<int, string> (Resource.Drawable.AmericanDesserts, "American Desserts");
			AmericanFood[1] = new Tuple<int, string> (Resource.Drawable.AmericanMeats, "American Meats");
			AmericanFood[2] = new Tuple<int, string> (Resource.Drawable.AmericanVegetables, "American Vegetables");
			AmericanFood[3] = new Tuple<int, string> (Resource.Drawable.AmericanHamburgers, "American Hamburgers");

			JapaneseFood = new Tuple<int, string>[4];
			TupleDict.Add ("Japanese", JapaneseFood);
			JapaneseFood[0] = new Tuple<int, string> (Resource.Drawable.JapaneseDesserts, "Japanese Desserts");
			JapaneseFood[1] = new Tuple<int, string> (Resource.Drawable.JapaneseMeats, "Japanese Meats");
			JapaneseFood[2] = new Tuple<int, string> (Resource.Drawable.JapaneseVegetables, "Japanese Vegetables");
			JapaneseFood[3] = new Tuple<int, string> (Resource.Drawable.JapaneseSushi, "Japanese Sushi");

			GreekFood = new Tuple<int, string>[4];
			TupleDict.Add ("Greek", GreekFood);
			GreekFood[0] = new Tuple<int, string> (Resource.Drawable.GreekDesserts, "Greek Desserts");
			GreekFood[1] = new Tuple<int, string> (Resource.Drawable.GreekMeats, "Greek Meats");
			GreekFood[2] = new Tuple<int, string> (Resource.Drawable.GreekVegetables, "Greek Vegetables");
			GreekFood[3] = new Tuple<int, string> (Resource.Drawable.GreekGyros, "Greek Gyros");

			ThaiFood = new Tuple<int, string>[4];
			TupleDict.Add ("Thai", ThaiFood);
			ThaiFood[0] = new Tuple<int, string> (Resource.Drawable.ThaiDesserts, "American Desserts");
			ThaiFood[1] = new Tuple<int, string> (Resource.Drawable.ThaiMeats, "American Meats");
			ThaiFood[2] = new Tuple<int, string> (Resource.Drawable.ThaiVegetables, "American Vegetables");
			ThaiFood[3] = new Tuple<int, string> (Resource.Drawable.ThaiFoodEx, "Thai Food");

			IndianFood = new Tuple<int, string>[4];
			TupleDict.Add ("Indian", IndianFood);
			IndianFood[0] = new Tuple<int, string> (Resource.Drawable.IndianDesserts, "Indian Desserts");
			IndianFood[1] = new Tuple<int, string> (Resource.Drawable.IndianMeats, "Indian Meats");
			IndianFood[2] = new Tuple<int, string> (Resource.Drawable.IndianVegetables, "Indian Vegetables");
			IndianFood[3] = new Tuple<int, string> (Resource.Drawable.IndianCurry, "Indian Curry");

			SelectedNationality = "American";
		}

		private static volatile CachedData _instance;
		private static object syncRoot = new Object();

		public static CachedData Instance
		{
			get
			{
				if (_instance == null) 
				{
					lock (syncRoot) 
					{
						if (_instance == null) 
							_instance = new CachedData();
					}
				}

				return _instance;
			}
		}
	}
}