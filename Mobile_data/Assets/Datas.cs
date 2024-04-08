using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Unity.VisualScripting;
using System.Text;
using System;

public class Datas : MonoBehaviour
{
    //Datapaths to XML
    private string _dataPath;
    private string _xmlMembers;
    private string _jsonMembers;


    // floats and text to show on the app
    public float Direc_X;
    public float Direc_Y;
    public Text Score_x;
    public Text Score_y;

    // 5 sec delay value
    public float targetTime = 5.0f;

    //Array to store values
    float[] X_values = new float[6];
    float[] Y_values = new float[6];

    //Counter to go 1 row up for the array, and stop_count bool
    int I_val = 0;
    bool stop_count;

    void Awake()
    {
        //Logs the datapath for the data. 
        _dataPath = "AndroidManifest.xml";
        Debug.Log(_dataPath);

        _xmlMembers = _dataPath + "Data.xml";
    }

    void Update()
    {
        //Finds the X and y sensor  in the phone, and assign it a value. 
        Direc_X = Input.acceleration.x;
        Direc_Y = Input.acceleration.y;
        //Convert text to string
        Score_x.ToString();
        Score_y.ToString();
        Score_x.text = Direc_X.ToString();
        Score_y.text = Direc_Y.ToString();

        //Timer for 5 seconds begins here 
        targetTime -= Time.deltaTime;
        if ((targetTime <= 0.0f) && stop_count == false)
        {
            timerEnded();
        }

    }

    void timerEnded()
    {
        //When 5 seconds goes by the i_val goes 1 up
        I_val = I_val + 1;
        //Then it takes the current value, and puts it in the arrays for x and y
        Direc_X = X_values[I_val];
        Direc_Y = Y_values[I_val];
        //The debug logs tells the value, and displays them
        Debug.Log(X_values[I_val]);
        Debug.Log(Y_values[I_val]);
        //targettime is set to 5.0 again, to reset the process.
        targetTime = 5.0f;
        //If 5 values has been found it converts the file into XML
        if (I_val >= 5)
            stop_count = true;
            Initialize();
    }

    public void Initialize()
    {
        NewDirectory();
        CreateXML();
        ConvertToXML();
    }

    public void NewDirectory()
    {
        if (Directory.Exists(_dataPath))
        {
            Debug.Log("Directory already exists...");
            return;
        }
        //New directory is created at _datapath
        Directory.CreateDirectory(_dataPath);
        Debug.Log("New directory created!");
    }
    public void CreateXML()
    {
        // Using xmlSerializer. An instance is created that passes the type of data, which is going to be translated.
        var XmlSerializer = new XmlSerializer(typeof(X_values[]));

        // Here a filestream is used. This wraps it in a code block and closes it
        using (FileStream stream = File.Create(_xmlMembers))
        {
            XmlSerializer.Serialize(stream, X_values);
        }
    }

        public void ConvertToXML()
    { 
        // We find/check the file if it exist. 
        if (File.Exists(_xmlMembers))
        {
            // Here we do the same as above
            var xmlSerializer = new XmlSerializer(typeof(X_values[]));

            //The same here, but here we openread instead of creating a file. 
            using (FileStream stream = File.OpenRead(_xmlMembers))
            {
                // Here a variable is created to hold the deserialized values.  
                var members = ()xmlSerializer.Deserialize(stream);

            }
        }
    }
}

