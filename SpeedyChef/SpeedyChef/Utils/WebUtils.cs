using System;
using System.Net;
using System.IO;

namespace SpeedyChef
{
	public static class WebUtils
	{
		//TODO change this
		private static string baseURI = "CHANGE ME";

		public static RecipeStep[] getRecipeSteps() {
			string response = getJSONResponse ("/Steps");
			return new RecipeStep[1];
		}

		public static string getJSONResponse(string requestUrl) {
			var request = HttpWebRequest.Create (baseURI + requestUrl);
			request.ContentType = "application/json";
			request.Method = "GET";

			var response = request.GetResponse ();
			StreamReader streamReader = new StreamReader (response.GetResponseStream());
			string responseData = streamReader.ReadToEnd ();
			return responseData;
		}

	}
}

