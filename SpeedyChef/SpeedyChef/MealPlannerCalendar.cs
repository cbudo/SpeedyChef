
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


namespace SpeedyChef
{
	[Activity (Theme = "@style/MyTheme", Label = "MealPlannerCalendar")]			
	public class MealPlannerCalendar : Activity
	{
		/**
		 * Button currently highlighted after being clicked on 
		 **/ 
		DateButton selected = null;

		/**
		 * Layout where meal information is displayed
		 **/
		LinearLayout mealDisplay = null;

		/**
		 * Current day of app 
		 **/ 
		DateTime current = DateTime.Now;

		/**
		 * DateTime for the date to view on screen
		 **/
		DateTime viewDate = DateTime.Now;

		/**
		 * Debug text bar to use to help present data
		 **/
		TextView debug = null;

		/**
		 * Month banner that needs to be adjusted with the date
		 **/ 
		TextView monthInfo = null;

		/**
		 * Current date TextView object that will be highlighted
		 **/ 
		TextView currentDate = null;

		/**
		 * List of all buttons in display area to be selected
		 **/
		DateButton[] daysList = null;

		/**
		 * Location of button
		 **/
		RelativeLayout addBar = null;



		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.MealPlannerCalendar);

			// Provides global 
			mealDisplay = FindViewById<LinearLayout> (Resource.Id.mealDisplay);
			debug = FindViewById<TextView> (Resource.Id.debug);
			monthInfo = FindViewById<TextView> (Resource.Id.weekOf);
			daysList = new DateButton[7];
			addBar = FindViewById<RelativeLayout> (Resource.Id.addBar);
			// Makes sure day is selected before you can add a meal
			if (selected == null) {
				addBar.Visibility = Android.Views.ViewStates.Invisible;
				mealDisplay.Visibility = Android.Views.ViewStates.Invisible;
			}

			Button addButton = FindViewById<Button> (Resource.Id.addMeal);
			addButton.Click += (sender, e) => {
				Intent intent = new Intent (this, typeof(MealDesign));
				// Console.WriteLine (selected.GetDateField().ToBinary());
				// Adds Binary field to be parsed later
				intent.PutExtra ("Date", selected.GetDateField ().ToBinary ());
				StartActivity (intent);
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
						StartActivity (intent);
					} else if (arg1.Item.TitleFormatted.ToString () == "Plan") {
						var intent = new Intent (this, typeof(MealPlannerCalendar));
						StartActivity (intent);
					} else if (arg1.Item.TitleFormatted.ToString () == "Walkthrough") {
						var intent = new Intent (this, typeof(StepsActivity));
						StartActivity (intent);
					}
					else if (arg1.Item.TitleFormatted.ToString() == "Preferences"){
						var intent = new Intent(this, typeof(Allergens));
						StartActivity(intent);
					}
				};
				menu.DismissEvent += (s2, arg2) => {
					menu_button.SetBackgroundResource (Resource.Drawable.menu_lines);
					Console.WriteLine ("menu dismissed");
				};
				menu.Show ();
			};

			// Define variables
			Calendar c = Calendar.GetInstance (Java.Util.TimeZone.Default); 
			Button[] shifters = new Button[2];

			// Setting up month bar
			monthInfo.Text = current.ToString ("MMMMMMMMMM") + " of " + current.Year;

			// Retrieve buttons from layouts
			daysList [0] = new DateButton (FindViewById<Button> (Resource.Id.day1));
			daysList [1] = new DateButton (FindViewById<Button> (Resource.Id.day2));
			daysList [2] = new DateButton (FindViewById<Button> (Resource.Id.day3));
			daysList [3] = new DateButton (FindViewById<Button> (Resource.Id.day4));
			daysList [4] = new DateButton (FindViewById<Button> (Resource.Id.day5));
			daysList [5] = new DateButton (FindViewById<Button> (Resource.Id.day6));
			daysList [6] = new DateButton (FindViewById<Button> (Resource.Id.day7));
			// Assigning dates to days
			handleCalendar (current);
			// Adding action listeners
			for (int i = 0; i < daysList.Length; i++) {
				daysList [i].wrappedButton.Click += new EventHandler ((s, e) => dayClick (s, e));
			}

			// Go backwards a week button
			shifters [0] = FindViewById<Button> (Resource.Id.leftShift);
			shifters [0].Click += delegate {
				GoBackWeek ();
			};

			// Advance week button
			shifters [1] = FindViewById<Button> (Resource.Id.rightShift);
			shifters [1].Click += delegate {
				GoForwardWeek ();
			};
			debug.Text = "";
			Button b1 = FindViewById<Button> (Resource.Id.Name1);
			b1.Click += delegate {
				MealInfo (b1.Parent);
			};
			// LinearLayout ll = FindViewById<LinearLayout> (Resource.Id.MealDisplay);
			// Console.WriteLine (ll.ChildCount + " Look for me");
		}

		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			if (resultCode == Result.Ok) {
				Console.WriteLine (data.GetStringExtra ("Result"));
			}
			Console.WriteLine ("Maybe");
		}


		/**
		 * Event Handler method to help get date to pass to next object
		 **/ 
		protected async void dayClick (object sender, EventArgs e)
		{
			if (selected != null) {
				selected.wrappedButton.SetBackgroundColor (Android.Graphics.Color.ParseColor ("#1E2327"));
			}
			if (currentDate != null) {
				currentDate.SetBackgroundColor (Android.Graphics.Color.ForestGreen);
			}
			
			selected = GetDateButton ((Button)sender);
			mealDisplay.Visibility = Android.Views.ViewStates.Visible;
			// Console.WriteLine(selected.GetDateField().ToBinary());
			selected.wrappedButton.SetBackgroundColor (Android.Graphics.Color.Cyan);
			// Can click the button after an action listener finds this.
			addBar.Visibility = Android.Views.ViewStates.Visible;
			string user = "tester";
			string useDate = selected.GetDateField ().ToString ("yyyy-MM-dd");
			// System.Diagnostics.Debug.WriteLine (useDate);
			string url = "http://speedychef.azurewebsites.net/CalendarScreen/GetMealDay?user=" + user + "&date=" + useDate;
			JsonValue json = await FetchMealDay (url);
			// System.Diagnostics.Debug.WriteLine (json.ToString ());
			parseMeals (json);
		}


		/**
		 * 
		 **/
		private void parseMeals (JsonValue json)
		{
			LinearLayout mealDisplay = FindViewById<LinearLayout> (Resource.Id.MealDisplay);
			System.Diagnostics.Debug.WriteLine (json.Count);
			mealDisplay.RemoveAllViews ();
			// mealDisplay.SetBackgroundColor (Android.Graphics.Color.White);
			for (int i = 0; i < json.Count; i++) {
				mealDisplay = makeObjects (json [i], i, mealDisplay);
			}
		}

		/**
		 * 
		 * 
		 **/
		private LinearLayout makeObjects (JsonValue json, int count, LinearLayout mealDisplay)
		{
			System.Diagnostics.Debug.WriteLine (json.ToString());
//			for (int i = 0; i < mealDisplay.ChildCount; i++) {
//				mealDisplay.RemoveView (mealDisplay.GetChildAt (i));
//			}
			LinearLayout mealObject = new LinearLayout (this);
			mealObject.Orientation = Orientation.Vertical;
			mealObject.SetMinimumWidth (25);
			mealObject.SetMinimumHeight (25);
			LinearLayout.LayoutParams mealObjectLL = 
				new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
			mealObject.LayoutParameters = mealObjectLL;
			mealObject.Id = count * 20 + 5;
			LinearLayout buttonCont = new LinearLayout (this);
			//buttonCont.SetBackgroundColor (Android.Graphics.Color.White);
			buttonCont.Orientation = Orientation.Horizontal;
			buttonCont.SetMinimumWidth (25);
			buttonCont.SetMinimumHeight (100);
			LinearLayout.LayoutParams bcll = 
				new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
			bcll.SetMargins (5, 5, 5, 5);
			buttonCont.LayoutParameters = bcll;
			buttonCont.Visibility = Android.Views.ViewStates.Visible;
			buttonCont.Id = count * 20 + 6;
			Button button = new Button (this, null, Resource.Style.generalButtonStyle);
			LinearLayout.LayoutParams lp = 
				new LinearLayout.LayoutParams(
					LinearLayout.LayoutParams.MatchParent, 
					LinearLayout.LayoutParams.MatchParent);
			button.LayoutParameters = lp;
			button.Text = json ["Mealname"];
			button.Visibility = Android.Views.ViewStates.Visible;
			button.SetBackgroundColor (Android.Graphics.Color.ParseColor("#3498DB"));
			buttonCont.AddView (button);
			mealObject.AddView (buttonCont);
			LinearLayout mealInfo = new LinearLayout (this);
			mealInfo.Orientation = Orientation.Vertical;
			mealInfo.SetMinimumWidth (25);
			mealInfo.SetMinimumHeight (25);
			LinearLayout.LayoutParams mealInfoLL = 
				new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
			mealInfoLL.SetMargins (5, 5, 5, 5);
			mealInfo.LayoutParameters = mealInfoLL;
			mealInfo.Id = count * 20 + 7;
			TextView mealSize = new TextView (this);
			mealSize.SetTextAppearance (this, Android.Resource.Style.TextAppearanceSmall);
			LinearLayout.LayoutParams tvll = new LinearLayout.LayoutParams (LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
			mealSize.LayoutParameters = tvll;
			mealSize.SetTextColor (Android.Graphics.Color.ParseColor("#FFFFFF"));
			mealSize.Text = "DINERS";
			mealObject.AddView (mealSize);
			mealDisplay.AddView (mealObject);
			return mealDisplay;
		}

		/**
		 * TODO 
		 * 
		 **/
		private async Task<JsonValue> FetchMealDay (string url)
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
					Console.Out.WriteLine ("Response: {0}", jsonDoc.ToString ());

					// Return the JSON document:
					return jsonDoc;
				}
			}

		}

		/**
		 * Finds matching button in button lists
		 **/
		private DateButton GetDateButton (Button b)
		{
			for (int i = 0; i < daysList.Length; i++) {
				if (daysList [i].wrappedButton.GetHashCode () == b.GetHashCode ()) {
					return daysList [i];
				}
			}
			Console.WriteLine ("Should never get here");
			return daysList [0];
		}


		/**
		 * Use this method to go to the Design page and take 
		 * and information you need with you.
		 *
		 **/
		public void MealInfo (IViewParent parent)
		{
			LinearLayout ll = (LinearLayout)parent.Parent;
			Console.WriteLine (parent.GetType ().ToString ());
			// LinearLayout ll = (LinearLayout) parent;
			Console.WriteLine (ll.ChildCount);
			Console.WriteLine ("Button");
		}

		/**
		 * Method that handles updating all the boxes in the calendar
		 * so that the dates line up in the week.
		 * 
		 * @param DateTime Date that the week view needs to be generated around. 
		 * 				   The date can be any day of the week
		 * */
		public void handleCalendar (DateTime date)
		{
			currentDate = null;
			string day = date.AddDays (-date.DayOfWeek.GetHashCode ()).ToString ("M/d");
			DateTime temp = date.AddDays (-date.DayOfWeek.GetHashCode ());
			string weekDay = temp.ToString ("ddd");
			monthInfo.Text = temp.ToString ("MMMMMMMMMM") + " of " + temp.Year;
			for (int i = 0; i < 7; i++) {
				// Determines the day from how far away from the beginning (Sunday)
				// and displays appropriately
				day = date.AddDays (-date.DayOfWeek.GetHashCode () + i).ToString ("M/d");
				weekDay = date.AddDays (-date.DayOfWeek.GetHashCode () + i).ToString ("ddd");
				daysList [i].wrappedButton.Text = weekDay.Substring (0, 1) + "\n" + day;
				daysList [i].SetDateField (date.AddDays (-date.DayOfWeek.GetHashCode () + i));
				// Sets all the buttons to the default colors
				daysList [i].wrappedButton.SetBackgroundColor (Android.Graphics.Color.ParseColor ("#1E2327"));
				// Handles setting the highlighting of the current day on the phone
				if (i == current.DayOfWeek.GetHashCode () && date.Date.Equals (current.Date)) {
					currentDate = daysList [i].wrappedButton;
					daysList [i].wrappedButton.SetBackgroundColor (Android.Graphics.Color.ForestGreen);
				}
			}
			// Removes any selected day
			selected = null;
			mealDisplay.Visibility = Android.Views.ViewStates.Invisible;
			// Makes the add button invisible
			addBar.Visibility = Android.Views.ViewStates.Invisible;
		}

		/**
		 * Action for button when < button is clicked.
		 * 
		 * */
		public void GoBackWeek ()
		{
			viewDate = viewDate.AddDays (-7);
			handleCalendar (viewDate);
		}

		/**
		 * Action for button when > button is clicked
		 * 
		 * */
		public void GoForwardWeek ()
		{
			viewDate = viewDate.AddDays (7);
			handleCalendar (viewDate);
		}
	}

	/**
	 * Wrapper class for button to help handle passing the dates.
	 **/
	public class DateButton
	{

		private DateTime dateField;
		public Button wrappedButton;

		public DateButton (Button button)
		{
			wrappedButton = button;
			this.dateField = DateTime.Now.AddDays (-100);
		}

		public void SetDateField (DateTime date)
		{
			// Console.WriteLine ("Wrote date");
			this.dateField = date;
		}

		public DateTime GetDateField ()
		{
			return this.dateField;
		}



	}
}