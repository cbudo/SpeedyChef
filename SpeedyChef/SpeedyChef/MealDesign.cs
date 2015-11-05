
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Json;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;

/// <summary>
/// Meal designer page.
/// </summary>
namespace SpeedyChef
{
	[Activity (Theme = "@style/MyTheme", Label = "MealDesign")]			
	public class MealDesign : CustomActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.MealDesign);
			// Create your application here
			long binaryDate = Intent.GetLongExtra ("Date", 0);
			DateTime date = DateTime.FromBinary (binaryDate);
			string useDate = date.ToString ("yyyy-MM-dd");
			string mealname = Intent.GetStringExtra ("Name");
			int mealId = Intent.GetIntExtra ("mealId", -1);
			int mealSize = Intent.GetIntExtra ("Mealsize", 0);
			System.Diagnostics.Debug.WriteLine (mealId);
			// System.Diagnostics.Debug.WriteLine (mealname + " I am here");

			// Changes meal name to passed in name
			EditText mealName = FindViewById<EditText> (Resource.Id.mealName);
			if (mealname != null) {
				mealName.Text = mealname;
			}

			//MENU VIEW
			Button menu_button = FindViewById<Button> (Resource.Id.menu_button);
			menu_button.Click += (s, arg) => {
				menu_button.SetBackgroundResource (Resource.Drawable.pressed_lines);
				PopupMenu menu = new PopupMenu (this, menu_button);
				menu.Inflate (Resource.Menu.Main_Menu);
				menu.MenuItemClick += this.MenuButtonClick;
				menu.DismissEvent += (s2, arg2) => {
					menu_button.SetBackgroundResource (Resource.Drawable.menu_lines);
					Console.WriteLine ("menu dismissed");
				};
				menu.Show ();
			};


			SeekBar sb = FindViewById<SeekBar> (Resource.Id.dinerCounter);
			TextView sbt = FindViewById<TextView> (Resource.Id.seekBarCount);
			sb.Progress = mealSize;
			sbt.Text = sb.Progress.ToString ();
			sb.ProgressChanged += (sender, e) => {
				if (e.FromUser) {
					sbt.Text = e.Progress.ToString ();
				}
			};
			Window.SetSoftInputMode (SoftInput.StateAlwaysHidden);
			Button returnButton = FindViewById<Button> (Resource.Id.returnButton);
			returnButton.Click += (sender, e) => {
				int[] recIds;
				if (mealId == -1) {
					// Adds to the procedure (No updating)
					recIds = GetRecIds ();
				} else {
					// Updates the items for the meal id
					recIds = GetRecIds ();
				}
				Intent i = new Intent (this, typeof(MealPlannerCalendar));
				i.PutExtra ("Result", "Passing back works");
				SetResult (Result.Ok, i);
				Finish ();
			};
			Button searchButton = FindViewById<Button> (Resource.Id.searchButton);
			searchButton.Click += (object sender, EventArgs e) => {
				// PRINTS
				System.Diagnostics.Debug.WriteLine ("SEARCHING PAGE");
				Intent i = new Intent (this, typeof(SearchActivity));
				CachedData.Instance.MealDesignMealId = mealId;
				StartActivityForResult (i, -1);
				// TODO

				// Connects to search page
			};
			Button removeButton = FindViewById<Button> (Resource.Id.removeButton);
			removeButton.Click += delegate {
				// PRINTS
				Intent i = new Intent (this, typeof(MealPlannerCalendar));
				if (mealId != -1) {
					// Change to meal id
					string user = "tester";
					string url = 
						"http://speedychef.azurewebsites.net/CalendarScreen/RemoveMealFromTables?user="
						+ user + "&mealid=" + mealId;
					GetMealRemoved (url);
				}
				i.PutExtra ("MealRemoved", mealId);
				SetResult (Result.Ok, i);
				Finish ();
				
				// Remove meal with mealId TODO
			};
			if (mealId == -1) {
				removeButton.Clickable = false;
			} 
			LinearLayout mealsArea = FindViewById<LinearLayout> (Resource.Id.mealsArea);
			LoadRecipes (mealsArea, mealId);
			//mealsArea.AddView (rl);
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			if (resultCode == Result.Ok && requestCode == -1) {
				System.Diagnostics.Debug.WriteLine("");
			}
		}

		/// <summary>
		/// Gets the recipe identifiers from the MealArea.
		/// </summary>
		/// <returns>The recipe identifiers.</returns>
		private int[] GetRecIds ()
		{
			LinearLayout mealsArea = FindViewById<LinearLayout> (Resource.Id.mealsArea);
			//System.Diagnostics.Debug.WriteLine (mealsArea.ChildCount);
			int[] ids = new int[mealsArea.ChildCount];
			for (int c = 0; c < mealsArea.ChildCount; c++) {
				// System.Diagnostics.Debug.WriteLine (((RecipeLayouts)mealsArea.GetChildAt (c)).recipeId);
				ids [c] = ((RecipeLayouts)mealsArea.GetChildAt (c)).recipeId;
			}
			return ids;
		}

		/// <summary>
		/// Gets the meal removed.
		/// </summary>
		/// <param name="url">URL.</param>
		private async void GetMealRemoved (string url)
		{
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (url));
			request.ContentType = "";
			request.Method = "GET";

			// Send the request to the server and wait for the response:
			using (WebResponse response = await request.GetResponseAsync ()) {
			}
		}


		/// <summary>
		///  Loads the recipes based on the meal id and has place to put the recipe results
		/// </summary>
		/// <param name="mealArea">Meal area to put results.</param>
		/// <param name="mealId">Meal identifier to find recipes related to.</param>
		private async void LoadRecipes (LinearLayout mealArea, int mealId)
		{
			string user = "tester";

			string url = "http://speedychef.azurewebsites.net/" +
			             "CalendarScreen/GetRecipesForMeal?user=" + user + "&mealId=" + mealId;
			JsonValue json = await FetchMealData (url);
			ParseRecipes (mealArea, mealId, json);
		}
			
		/// <summary>
		/// Loops through the Json and creates RecipeLayout objects for the 
		/// recipes related to the meal.
		/// </summary>
		/// <param name="mealArea">Meal area (view) to append objects to.</param>
		/// <param name="mealId">Meal identifier recipes are related to.</param>
		/// <param name="json">Json with information of zero or more recipes.</param>
		private void ParseRecipes (LinearLayout mealArea, int mealId, JsonValue json)
		{

			for (int i = 0; i < json.Count; i++) {
				MakeRecipeObjects (mealArea, mealId, json [i]);
			}
		}
			
		/// <summary>
		/// Makes the recipe objects for each recipe id.
		/// </summary>
		/// <param name="mealArea">Meal area (LinearLayout) to add object to.</param>
		/// <param name="mealId">Meal identifier of object.</param>
		/// <param name="json">Json values for a single recipe.</param>
		private void MakeRecipeObjects (LinearLayout mealArea, int mealId, JsonValue json)
		{
			RecipeLayouts rl = new RecipeLayouts (this, json ["Recname"], json ["Recid"], mealId);
			mealArea.AddView (rl);
		}

		 
		/// <summary>
		/// Fetchs the meal data in form of JSON from database using URL. 
		/// Called mainly with stored procedures.
		/// </summary>
		/// <returns>The meal data (Json).</returns>
		/// <param name="url">URL to call the API.</param>
		private async Task<JsonValue> FetchMealData (string url)
		{
			// Create an HTTP web request using the URL:
			HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create (new Uri (url));
			request.ContentType = "application/json";
			request.Method = "GET";

			// Send the request to the server and wait for the response:
			using (WebResponse response = await request.GetResponseAsync ()) {
				// Get a stream representation of the HTTP web response:
				using (Stream stream = response.GetResponseStream ()) {
					// Use this stream to build a JSON document object:
					JsonValue jsonDoc = await Task.Run (() => JsonObject.Load (stream));
					// Console.Out.WriteLine ("Response: {0}", jsonDoc.ToString ());

					// Return the JSON document:
					return jsonDoc;
				}
			}

		}
	}


	/// <summary>
	/// Container class used to present recipes for a meal and the option to 
	/// remove the container itself. Contained in a LinearLayout object and 
	/// holds 2 buttons with different functionality.
	/// </summary>
	public class RecipeLayouts : LinearLayout
	{

		/// <summary>
		/// Button object to remove recipe from a meal. Both from display 
		/// and from the database.
		/// </summary>
		private Button removeButton;

		/// <summary>
		/// The recipe info button.
		/// </summary>
		private Button recipeInfo;

		/// <summary>
		/// Gets or sets the recipe identifier.
		/// </summary>
		/// <value>The recipe identifier.</value>
		public int recipeId { get; set; }

		/// <summary>
		/// Gets or sets the name of the recipe.
		/// </summary>
		/// <value>The name of the recipe.</value>
		public string recipeName { get; set; }

		/// <summary>
		/// Gets or sets the meal identifier.
		/// </summary>
		/// <value>The meal identifier.</value>
		public int mealId { get; set; }


		/// <summary>
		/// Initializes a new instance of the <see cref="SpeedyChef.RecipeLayouts"/> class.
		/// </summary>
		/// <param name="context">Context of the layout.</param>
		/// <param name="recName">Recipe name of the object.</param>
		/// <param name="recId">Recipe identifier of the object.</param>
		/// <param name="mealId">Meal identifier of the object.</param>
		public RecipeLayouts (Context context, string recName, int recId, int mealId) : base (context)
		{
			
			this.removeButton = new Button (context, null, Resource.Style.generalButtonStyle);
			this.recipeInfo = new Button (context, null, Resource.Style.generalButtonStyle);
			this.recipeName = recName;
			this.recipeId = recId;
			this.mealId = mealId;
			CreateLPs ();
			CreateRLPs ();
			SetPropertiesLayout ();
			SetPropertiesRemove ();
			SetPropertiesInfo ();
			this.removeButton.Click += (object sender, EventArgs e) => {
				// PRINTS
				Console.WriteLine ("Remove button clicked");
				Console.WriteLine (this.recipeName);
				Console.WriteLine (this.recipeId);
				Console.WriteLine (this.mealId);
				// Removes from mealID (Has necessary ids, i think) TODO
				if (this.mealId != -1) {
					
				}
				// TODO


				this.Visibility = ViewStates.Gone;
				// Console.WriteLine ("Disposed, hopefully.");
			};
			this.recipeInfo.Click += (object sender, EventArgs e) => {
				// PRINTS
				Console.WriteLine ("Recipe info button clicked");
				Console.WriteLine (this.recipeId);
				// Goes to meal preview TODO
				Intent i = new Intent (context, typeof(RecipeViewActivity));
				i.PutExtra ("Recid", this.recipeId);
				context.StartActivity (i);
			};
			this.AddView (this.removeButton);
			this.AddView (this.recipeInfo);
		}

		/// <summary>
		/// Sets the properties layout of the object itself.
		/// </summary>
		private void SetPropertiesLayout ()
		{
			this.Orientation = Orientation.Horizontal;
			this.SetMinimumHeight (100);
			this.SetMinimumWidth (25);
		}

		/// <summary>
		/// Sets the properties for remove button.
		/// </summary>
		private void SetPropertiesRemove ()
		{
			this.removeButton.SetMinimumHeight (100);
			this.removeButton.SetMinimumWidth (25);
			this.removeButton.Text = "Remove";
			this.removeButton.SetTextAppearance (this.removeButton.Context, Resource.Style.generalButtonStyle);
			this.removeButton.SetBackgroundResource (Resource.Color.orange_header);
			this.removeButton.Gravity = GravityFlags.Center;
			this.removeButton.SetPadding (5, 5, 5, 5);
		}

		/// <summary>
		/// Sets the properties of recipe info button.
		/// </summary>
		private void SetPropertiesInfo ()
		{
			this.recipeInfo.SetMinimumHeight (100);
			this.recipeInfo.SetMinimumWidth (25);
			this.recipeInfo.Gravity = GravityFlags.Center;
			this.recipeInfo.Text = recipeName;
			this.recipeInfo.SetTextAppearance (this.recipeInfo.Context, Resource.Style.generalButtonStyle);
			this.recipeInfo.SetBackgroundResource (Resource.Color.orange_header);
		}

		/// <summary>
		/// Sets the layout parameters for each button
		/// </summary>
		private void CreateRLPs ()
		{
			LinearLayout.LayoutParams lllp = new 
				LinearLayout.LayoutParams (LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
			lllp.SetMargins (5, 5, 5, 5);
			this.removeButton.LayoutParameters = lllp;
			LinearLayout.LayoutParams llri = new 
				LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
			llri.SetMargins (5, 5, 5, 5);
			this.recipeInfo.LayoutParameters = llri;
		}

		/// <summary>
		/// Sets the layout parameters of the object
		/// </summary>
		private void CreateLPs ()
		{
			LinearLayout.LayoutParams lllp = new 
				LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
			lllp.SetMargins (5, 5, 5, 5);
			this.LayoutParameters = lllp;
		}
	}
}