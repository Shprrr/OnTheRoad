namespace UnityEngine
{
    static class GameObjectExtension
    {
        /// <summary>
        /// Removes all children of a gameobject. Not recursive.
        /// </summary>
        /// <param name="gameObject"></param>
        public static void DestroyAllChildren(this GameObject gameObject)
        {
            foreach (Transform child in gameObject.transform)
            {
                Object.Destroy(child.gameObject);
            }
        }
    }
}
