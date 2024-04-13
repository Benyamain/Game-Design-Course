#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

// Editor only classes.
namespace Editor
{
    /// <summary>
    /// Class that exists in the editor only to clear player preferences.
    /// </summary>
    public static class ClearPlayerPreferences
    {
        /// <summary>
        /// Clear player preferences from the menu under "Edit > Clear Player Preferences".
        /// </summary>
        [MenuItem("Edit/Clear Player Preferences", priority = int.MaxValue)]
        private static void Perform()
        {
            // Delete all player preferences.
            PlayerPrefs.DeleteAll();
        }
    }
}
#endif