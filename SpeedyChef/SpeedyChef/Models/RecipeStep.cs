using System;

namespace SpeedyChef
{
	public struct RecipeStep
	{
		public string title;
		public string imgUrl;
		public string desc;
		public int time;
		public bool timeable;
		public RecipeStepTimer timer;
	}
}

