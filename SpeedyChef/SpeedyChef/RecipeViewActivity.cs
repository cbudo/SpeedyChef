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
		public int Recid;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//this.Recid = CachedData.Instance.mostRecentRecSel;

			/*//MENU VIEW
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
			};*/
			int varTest = CachedData.Instance.mostRecentRecSel;
			Recipe recipe = WebUtils.getRecipeViewInfo (varTest);
			System.Diagnostics.Debug.WriteLine (recipe.title);
			FindViewById<TextView> (Resource.Id.recipe_view_title).Text = recipe.title;
			//FindViewById<TextView> (Resource.Id.recipe_view_time).Text = recipe.time + " minutes";

			ViewGroup ingredientView = (ViewGroup) FindViewById (Resource.Id.recipe_view_ingredients);

			for (int i = 0; i < recipe.ingredients.Length; i++) {
				Console.WriteLine ("Adding Ingredient " + recipe.ingredients [i]);
				TextView tv = new TextView (this);
				//tv.Visibility = ViewStates.Visible;
				//tv.TextSize = 20;
				tv.SetTextColor (Resources.GetColor(Resource.Color.gray_icon));
				tv.TextSize = 14;
				tv.Text = recipe.ingredients [i];
				ingredientView.AddView (tv);
			}

			ViewGroup taskView = (ViewGroup) FindViewById (Resource.Id.recipe_view_steps);

			for (int i = 0; i < recipe.tasks.Length; i++) {
				Console.WriteLine ("Adding Step " + recipe.tasks [i]);
				TextView tv = new TextView (this);
				tv.SetTextColor (Resources.GetColor(Resource.Color.gray_icon));
				tv.TextSize = 14;
				tv.Text = recipe.tasks [i];
				taskView.AddView (tv);
			}
			Button addToMealButton = FindViewById<Button> (Resource.Id.add_rec_to_meal_button);
			addToMealButton.Click += (object sender, EventArgs e) => {
				CachedData.Instance.mostRecentMealAdd = CachedData.Instance.mostRecentRecSel;
				if (CachedData.Instance.PreviousActivity.GetType() == typeof(SearchActivity)) {
					CachedData.Instance.PreviousActivity.Finish();
					CachedData.Instance.PreviousActivity.SetResult(Result.Ok);
					SetResult(Result.Ok, CachedData.Instance.PreviousActivity.Intent);
					Finish();
				}
			};
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
	}
}

