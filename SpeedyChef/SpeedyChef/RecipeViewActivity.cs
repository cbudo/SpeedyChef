
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
	[Activity (Label = "RecipeViewActivity")]			
	public class RecipeViewActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//Retrieve stored recipe information
			String recName = Intent.GetStringExtra("recName");
			SetContentView (Resource.Layout.RecipeView);
		}
	}
}

