using System;
using System.Net;
using System.IO;
using System.Json;

namespace SpeedyChef
{
	public static class WebUtils
	{
		//The base url for all requests. This may be subject to change
		private static string baseURI = "http://speedychef.azurewebsites.net";

		public static RecipeStep[] getRecipeSteps(int mealId) {
			JsonValue returnedSteps = getJSONResponse ("/Steps?mealid=" + mealId);
			RecipeStep[] steps = new RecipeStep[returnedSteps.Count];
			for (int i = 0; i < returnedSteps.Count; i++) {
				JsonValue currentItem = returnedSteps [i];
				RecipeStep currentStep = new RecipeStep ();
				currentStep.title = currentItem ["Taskname"];
				currentStep.desc = currentItem["Taskdesc"];
				currentStep.time = currentItem ["Tasktime"];
				//currentStep.timeable = currentItem ["Tasktimeable"];
				if(i > 1 && i < 4) //For testing
					currentStep.timeable = true;
				if (currentStep.timeable)
					currentStep.timerHandler = new RecipeStepTimerHandler (currentStep.time, currentStep.title);
				steps [i] = currentStep;
			}
			return steps;
		}

		public static Recipe getRecipeViewInfo(int recId) {
			JsonValue recipeInfo = getJSONResponse ("/RecipeInfo/RecipeInfo?recid=" + recId);
			recipeInfo = recipeInfo [0];
			JsonValue recipeTasks = getJSONResponse ("/RecipeInfo/RecipeTasks?recid=" + recId);
			JsonValue recipeIngredients = getJSONResponse ("/RecipeInfo/RecipeIngredients?recid=" + recId);
			Recipe r = new Recipe ();
			r.title = recipeInfo ["Recname"];
			//r.desc = recipeInfo ["Recdesc"];
			//r.time = recipeInfo ["Rectime"];
			//r.diff = recipeInfo ["Recdiff"];
			string[] ingredients = new string[recipeIngredients.Count];
			string[] tasks = new string[recipeTasks.Count];
			for (int i = 0; i < recipeIngredients.Count; i++) {
				//ingredients [i] = "Ingredient " + i;
				string ingredient = recipeIngredients[i]["Foodname"];
				if (recipeIngredients [i] ["FoodAmount"] != null)
					ingredient += ", " + recipeIngredients [i] ["FoodAmount"];
				if (recipeIngredients [i] ["FoodAmountUnit"] != null)
					ingredient += " " + recipeIngredients [i] ["FoodAmountUnit"];
				
				ingredients [i] = ingredient;
			}

			int recTime = 0;
			for (int i = 0; i < recipeTasks.Count; i++) {
				tasks [i] = recipeTasks [i] ["Taskdesc"];
				recTime += recipeTasks [i] ["Tasktime"];
			}
			r.ingredients = ingredients;
			r.tasks = tasks;
			return r;
		}

		public static JsonValue getJSONResponse(string requestUrl) {
			var request = HttpWebRequest.Create (baseURI + requestUrl);
			request.ContentType = "application/json";
			request.Method = "GET";

			var response = request.GetResponse ();
			return JsonValue.Load (response.GetResponseStream ());
		}

	}
}

