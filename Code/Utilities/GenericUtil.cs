using System;

namespace Common.Utilities
{
	public static class Find : Object
	{
		public static List<GameObject> FindGameObjectsByTag(Scene scene, string tag) 
		{
			return scene.GetAllObjects(true)
						.Where(go => go.Tags.Has(tag))
						.ToList();
		}

		public static List<GameObject> FindParentsByTag(Scene scene, string tag) 
		{
			return FindGameObjectsByTag(scene, tag)
						.Where(go => go.Parent.Tags.Has(tag) == false)
						.ToList();
		}
	}

	public static class Remove : Object
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

		public static void DestroyAll<T>(List<T> objects) where T : class
		{
			foreach (var obj in objects)
				Destroy(obj);
		}
	}
}