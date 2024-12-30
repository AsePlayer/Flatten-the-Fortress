using System;

namespace Common.Utilities
{
	public sealed class GameObjectFinder : Component
	{
		public List<GameObject> FindGameObjectsByTag(string tag) 
		{
			return Scene.GetAllObjects(true)
						.Where(go => go.Tags.Has(tag))
						.ToList();
		}

		public List<GameObject> FindParentsByTag(string tag) 
		{
			return FindGameObjectsByTag(tag)
						.Where(go => go.Parent.Tags.Has(tag) == false)
						.ToList();
		}
	}

	public sealed class Remove : Component
	{
		public static void Destroy<T>(T obj) where T : class
		{
			if (obj is GameObject gameObject)
				gameObject.Destroy();

			else if (obj is Component component)
				component.Destroy();
				
			else 
				throw new ArgumentException($"Unsupported type for destruction: {typeof(T)}");
		}
	}
}