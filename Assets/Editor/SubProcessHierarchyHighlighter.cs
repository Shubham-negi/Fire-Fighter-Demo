using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class HierarchyColorHighlighter
{
    static HierarchyColorHighlighter()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (obj == null) return;

        // Color for ProcessManager
        if (obj.GetComponent<ProcessManager>() != null)
        {
            EditorGUI.DrawRect(selectionRect, new Color(0f, 0.75f, 1f, 0.7f)); // Light Blue
            return;
        }

        // Color for Process
        Process process = obj.GetComponent<Process>();
        if (process != null)
        {
            if (process.isStarted && !process.isCompleted)
            {
                EditorGUI.DrawRect(selectionRect, new Color(0.8f, 0.8f, 0f, 0.6f)); // Yellow for Active Process
            }
            else if (process.isCompleted)
            {
                EditorGUI.DrawRect(selectionRect, new Color(0f, 0.5f, 0.8f, 0.6f)); // Blue for Completed Process
            }
            else
            {
                EditorGUI.DrawRect(selectionRect, new Color(1f, 0.5f, 0f, 0.4f)); // Orange for Idle Process
            }
            return;
        }

        // Color for SubProcess
        SubProcess subprocess = obj.GetComponent<SubProcess>();
        if (subprocess != null)
        {
            if (subprocess.isStarted && !subprocess.isCompleted)
            {
                EditorGUI.DrawRect(selectionRect, new Color(0.1f, 1f, 0.1f, 0.6f)); // Green for Active SubProcess
            }
            else if (subprocess.isCompleted)
            {
                EditorGUI.DrawRect(selectionRect, new Color(0.6f, 0f, 1f, 0.6f)); // Purple for Completed SubProcess
            }
            else
            {
                EditorGUI.DrawRect(selectionRect, new Color(1f, 0.75f, 0.2f, 0.4f)); // Light Orange for Idle SubProcess
            }
        }
    }
}
