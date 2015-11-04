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

		public static RecipeStep[] getRecipeSteps(int AgendaId) {
			JsonValue returnedSteps = getJSONResponse ("/Steps");
			RecipeStep[] steps = new RecipeStep[returnedSteps.Count];
			for (int i = 0; i < returnedSteps.Count; i++) {
				JsonValue currentItem = returnedSteps [i];
				RecipeStep currentStep = new RecipeStep ();
				currentStep.title = currentItem ["taskName"];
				currentStep.desc = currentItem["taskDesc"];
				currentStep.time = currentItem ["taskTime"];
				currentStep.time *= 60;
				currentStep.timeable = currentItem ["taskTimeable"];
				//currentStep.timeable = true; //For testing
				if (currentStep.timeable)
					currentStep.timerHandler = new RecipeStepTimerHandler (currentStep.time, currentStep.title);
				steps [i] = currentStep;
			}
			return steps;
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

