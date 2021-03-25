using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using UnityEngine;

public class LegoManager : MonoBehaviour
{
    public GameObject lego;
    [HideInInspector]
    public GameObject[,] legoArray;
    [HideInInspector]
    public Material[,] legoColor;
    public Gradient gradient;
    [HideInInspector]
    public float[,] legoInput;
    [Range(0f,1f)]
    public float gapLength;
    private float legoWidth;
    public bool useGradient;
    public StreamWriter sw;
    public Process process;
    public ProcessStartInfo proc;
    //private Color defaultColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        sw = new StreamWriter("/Users/loyichun/project/interface/test.txt");
        legoArray = new GameObject[10, 10];
        legoInput = new float[10, 10];
        legoWidth = lego.transform.lossyScale.x;
        legoColor = new Material[10, 10];
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
                legoArray[i, j] = Instantiate(lego,Vector3.zero,Quaternion.identity);
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
                legoColor[i, j] = legoArray[i, j].GetComponent<MeshRenderer>().material;
    }
    
    void StartProcess()
    {
        try
        {
            proc = new ProcessStartInfo();
            proc.FileName = "/bin/bash";
            proc.WorkingDirectory = "/Users/loyichun/project/interface";
            proc.Arguments = "trans.sh";
            Process.Start(proc);
        }
        catch(Exception e)
        {
            UnityEngine.Debug.LogError("Failed to launch trans.sh...");
        }
    }

    void WriteData()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                double val = legoArray[i, j].transform.position.y + 0.75;
                val *= 10;
                if (i == 9 && j == 9)
                {
                    sw.Write(Math.Round(val) + "\n");
                }
                else
                {
                    sw.Write(Math.Round(val) + ",");
                }
            }
        }
        // Thread
        StartProcess();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateLego();
        // output
        //WriteData();
    }

    void UpdateLego()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                legoInput[i, j] = Mathf.Clamp(legoInput[i, j], 0, 1.5f);
                if (useGradient)
                    legoColor[i, j].color = gradient.Evaluate(legoInput[i, j] / 1.5f);
            }
        }    
                
        for (int i = 0; i < 10; i++)
            for (int j = 0; j < 10; j++)
                legoArray[i, j].transform.position = new Vector3((5 - i - 0.5f) * (legoWidth + gapLength),
                                                                 -0.75f + legoInput[i,j],
                                                                 (5 - j - 0.5f) * (legoWidth + gapLength));
    }
}
