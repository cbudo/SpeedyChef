﻿
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
using Java.Util;

namespace SpeedyChef
{
	[Activity (Label = "MealPlannerCalendar")]			
	public class MealPlannerCalendar : Activity
	{
		/**
		 * Button currently highlighted after being clicked on 
		 **/ 
		Button selected = null;

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
		Button[] daysList = null;

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
			debug = FindViewById<TextView> (Resource.Id.debug);
			monthInfo = FindViewById<TextView> (Resource.Id.weekOf);
			daysList = new DateButton[7];
			addBar = FindViewById<RelativeLayout> (Resource.Id.addBar);
			// Makes sure day is selected before you can add a meal
			if (selected == null) {
				addBar.Visibility = Android.Views.ViewStates.Invisible;
			}

			Button addButton = FindViewById<Button> (Resource.Id.addMeal);
			addButton.Click += (sender, e) => {
				//StartActivity(typeof(AddMealActivity));
				//var intent = new Intent(this, typeof(AddMealActivity));
				//StartActivity(intent);

			};

			// Define variables
			Calendar c = Calendar.GetInstance (Java.Util.TimeZone.Default); 
			Button[] shifters = new Button[2];

			// Setting up month bar
			monthInfo.Text = current.ToString ("MMMMMMMMMM") + " of " + current.Year;

			// Retrieve buttons from layouts
			daysList [0] = FindViewById<Button> (Resource.Id.day1);
			daysList [1] = FindViewById<Button> (Resource.Id.day2);
			daysList [2] = FindViewById<Button> (Resource.Id.day3);
			daysList [3] = FindViewById<Button> (Resource.Id.day4);
			daysList [4] = FindViewById<Button> (Resource.Id.day5);
			daysList [5] = FindViewById<Button> (Resource.Id.day6);
			daysList [6] = FindViewById<Button> (Resource.Id.day7);

			// Adding action listeners
			for (int i = 0; i < daysList.Length; i++) {
				daysList [i].Click += (sender, e) => {
					if (selected != null) {
						selected.SetBackgroundColor (Android.Graphics.Color.ParseColor ("#1E2327"));
					}
					if (currentDate != null) {
						currentDate.SetBackgroundColor (Android.Graphics.Color.ForestGreen);
					}
					selected = (Button)sender;
					selected.SetBackgroundColor (Android.Graphics.Color.Cyan);
					// Can click the button after an action listener finds this.
					addBar.Visibility = Android.Views.ViewStates.Visible;
				};
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
			handleCalendar (current);
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
				daysList [i].Text = weekDay.Substring (0, 1) + "\n" + day;
				// Sets all the buttons to the default colors
				daysList [i].SetBackgroundColor (Android.Graphics.Color.ParseColor ("#1E2327"));
				// Handles setting the highlighting of the current day on the phone
				if (i == current.DayOfWeek.GetHashCode () && date.Date.Equals (current.Date)) {
					currentDate = daysList [i];
					daysList [i].SetBackgroundColor (Android.Graphics.Color.ForestGreen);
				}
			}
			// Removes any selected day
			selected = null;
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

	public class DateButton : Button {

		private DateTime dateField;

		public DateButton(Context context) : base(context){
			
		}
			
		public void SetDateField(DateTime date){
			this.dateField = date;
		}

	}
}
