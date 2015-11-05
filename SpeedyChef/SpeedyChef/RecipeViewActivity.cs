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

namespace SpeedyChef
{
	[Activity (Theme="@style/MyTheme", Label = "SpeedyChef", Icon = "@drawable/icon")]			
	public class RecipeViewActivity : CustomActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//Retrieve stored recipe information
			String recName = Intent.GetStringExtra("recName");
			SetContentView (Resource.Layout.RecipeView);
			Button addToMealButton = FindViewById<Button> (Resource.Id.add_rec_to_meal_button);
			addToMealButton.Click += (object sender, EventArgs e) => {
				CachedData.Instance.mostRecentMealAdd = CachedData.Instance.mostRecentRecSel;
				if (CachedData.Instance.ActivityContext == typeof(SearchActivity)) {
					CachedData.Instance.PreviousActivity.Finish();
					this.Finish();
				}
			};
		}
	}
}

