
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
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Graphics.Drawables;
using Android.Util;

namespace SpeedyChef
{
	[Activity (Theme="@style/MyTheme", Label = "SpeedyChef", Icon = "@drawable/icon")]
	public class CustomActivity : FragmentActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
		}

		public void MenuButtonClick (object s, PopupMenu.MenuItemClickEventArgs arg){
			if (arg.Item.TitleFormatted.ToString () == "Browse" && typeof(BrowseNationalitiesActivity) != CachedData.Instance.ActivityContext) {
				var intent = new Intent (this, typeof(BrowseNationalitiesActivity));
				CachedData.Instance.ActivityContext = this.GetType ();
				StartActivity (intent);
			} else if (arg.Item.TitleFormatted.ToString () == "Plan" && typeof(MealPlannerCalendar) != CachedData.Instance.ActivityContext) {
				var intent = new Intent (this, typeof(MealPlannerCalendar));
				CachedData.Instance.ActivityContext = this.GetType ();
				StartActivity (intent);
			} else if (arg.Item.TitleFormatted.ToString () == "Walkthrough" && typeof(StepsActivity) != CachedData.Instance.ActivityContext) {
				var intent = new Intent (this, typeof(StepsActivity));
				CachedData.Instance.ActivityContext = this.GetType ();
				intent.PutExtra ("AgendaId", 1);
				StartActivity (intent);
			} else if (arg.Item.TitleFormatted.ToString () == "Search" && typeof(SearchActivity) != CachedData.Instance.ActivityContext) {
				var intent = new Intent (this, typeof(SearchActivity));
				CachedData.Instance.ActivityContext = this.GetType ();
				StartActivity (intent);
			} else if (arg.Item.TitleFormatted.ToString () == "Preferences" && typeof(Allergens) != CachedData.Instance.ActivityContext) {
				var intent = new Intent (this, typeof(Allergens));
				CachedData.Instance.ActivityContext = this.GetType ();
				StartActivity (intent);
			} else if (arg.Item.TitleFormatted.ToString () == "Home" && typeof(SearchActivity) != CachedData.Instance.ActivityContext) {
				var intent = new Intent (this, typeof(MainActivity));
				CachedData.Instance.ActivityContext = this.GetType ();
				StartActivity (intent);
			}
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
		public int mostRecentRecSel { get; set; }
		public int mostRecentMealAdd { get; set; }
		public Activity PreviousActivity { get; set; }
		public int MealDesignMealId { get; set; }

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

