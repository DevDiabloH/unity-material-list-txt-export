using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TreeMaterialNameExport : MonoBehaviour
{
    public bool addLayerNum = true;

    // path = Assets/StreamingAssets/material_tree_result.txt
    private string path = Application.streamingAssetsPath + "/material_tree_result.txt";
    private List<string> data = null;

    void WriteTxt(string filePath, List<string> messages)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Path.GetDirectoryName(filePath));
        if (!directoryInfo.Exists)
        {
            directoryInfo.Create();
        }

        FileStream fileStream;
        try
        {
            fileStream = new FileStream(filePath, FileMode.Truncate, FileAccess.Write);
        }
        catch
        {
            fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
        }
        StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.Unicode);

        foreach(string txt in messages)
        {
            writer.WriteLine(txt);
        }

        writer.Close();
    }

    public string MakeTabSpace(int layer)
    {
        string _space = "";

        for (int i = 0; i < layer - 1; i++)
        {
            _space += "\t";
        }

        return _space;
    }

    public Transform TreeRecursive(Transform body, int layer)
    {
        layer++;
        string _space = MakeTabSpace(layer);

        Transform result = null;
        for(int i=0; i<body.childCount; i++)
        {
            Transform child = body.GetChild(body.childCount - i - 1);
            TreeRecursive(child, layer);
        }

        if (null != body.GetComponent<MeshRenderer>())
        {
            // write materials
            foreach (Material mt in body.GetComponent<MeshRenderer>().sharedMaterials)
            {
                data.Add(_space + mt.name.Replace("(Instance)", ""));
            }
        }

        // write mesh renderer object name
        if (addLayerNum)
        {
            data.Add(_space + layer + "] " + body.name);
        }
        else
        {
            data.Add(_space + body.name);
        }

        return result;
    }

    public void Export()
    {
        Debug.Log("Export Start");
        data = new List<string>();
        TreeRecursive(this.transform, 0);
        data.Reverse();
        WriteTxt(path, data);
        Debug.Log("Export Complete :: " + path);
    }
}
