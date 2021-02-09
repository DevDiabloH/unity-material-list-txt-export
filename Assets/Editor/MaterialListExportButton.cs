using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TreeMaterialNameExport))]
public class MaterialListExportButton : Editor
{
    private TreeMaterialNameExport comp;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Export"))
        {
            comp = (TreeMaterialNameExport)target;
            comp.Export();
        }
    }
}
