using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

public class TestMoveTo : MonoBehaviour
{
    float tableSize = 448;
    public GameObject thing;

    // if true, positions on table cloth will be recorded
    bool recordPositionsAA = true;

    string namehaha = "data/locations/configs_509";
    string pathCSV = "ppp.csv";
    string pathJSON = "ppp.json";

    string[] things = { "Wine Glass BB", "Charger BB", "Napkin Cloth BB", "Table Cloth Sides", //"Napkin Cloth BB", "Coffee BB",
        "Water Glass BB", "Bread Plate BB", "Dinner Knife BB", "Salad Knife BB", "Butter Knife BB",
        "Salad Fork BB", "Dinner Fork BB", "Soup Spoon BB", "Dessert Fork BB", "Dessert Spoon BB",
        "Dinner Plate BB", "Wine Glass BB (1)", "Wine Glass BB (2)", "Coffee Plate BB"};

    Dictionary<string, string> names = new Dictionary<string, string>()
        {
            {"Wine Glass BB", "wine_glass"},
            {"Charger BB", "charger"},
            {"Napkin Cloth BB", "napkin"},
            {"Table Cloth Sides", "table_cloth"},
            {"Water Glass BB", "water_glass"},
            {"Bread Plate BB", "bread_plate"},
            {"Dinner Knife BB", "dinner_knife"},
            {"Salad Knife BB", "salad_knife"},
            {"Butter Knife BB", "butter_knife"},
            {"Salad Fork BB", "salad_fork"},
            {"Dinner Fork BB", "dinner_fork"},
            {"Soup Spoon BB", "soup_spoon"},
            {"Dessert Fork BB", "dessert_fork"},
            {"Dessert Spoon BB", "dessert_spoon"},
            {"Dinner Plate BB", "dinner_plate"},
            {"Wine Glass BB (1)", "wine_glass_1"},
            {"Wine Glass BB (2)", "wine_glass_2"},
            {"Coffee Plate BB", "coffee"}
        };

    void UpdateAllFiles()
    {
        string path = Directory.GetCurrentDirectory();
        Debug.Log(path);
        try
        {
            string[] dirs = Directory.GetFiles(@"data/locations/", "positions_*");
            foreach (string dir in dirs)
            {
                recordPositionsAA = false;
                pathCSV = dir;
                MoveAllTo();
                pathJSON = pathCSV.Replace("positions", "configs").Replace("csv", "json");
                Debug.Log(pathJSON);
                RecordPositions();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("The process failed: {0}", e.ToString());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //UpdateAllFiles();

        pathCSV = "data/locations/positions_300.csv";

        // ------------- record AA
        //pathCSV = pathCSV.Replace("BB", "AA");
        //ChangeThingsToAA();
        //RecordPositions(pathCSV, things);
        //Disappear(things);

        // ------------- record BB
        //ChangeThingsToBB();
        //pathCSV = pathCSV.Replace("AA", "BB");
        //recordPositionsAA = false;
        //RecordPositions(pathCSV, things);

        MoveAllTo();
        //namehaha = "data/locations/configs_100";

        //pathCSV = pathCSV.Replace("ppp", namehaha).Replace("configs","positions");
        //pathJSON = pathJSON.Replace("ppp", namehaha);
        //RecordPositions(pathCSV, things);
    }

    void ChangeThingsToAA()
    {
        for (int i = 0; i < things.Length; i++)
        {
            things[i] = things[i].Replace("BB", "AA");
        }
    }

    void ChangeThingsToBB()
    {
        for (int i = 0; i < things.Length; i++)
        {
            things[i] = things[i].Replace("AA", "BB");
        }
    }

    void PrintArray(string[] things)
    {
        for (int i = 0; i < things.Length; i++)
        {
            Debug.Log(things[i]);
        }
    }

    void Disappear(string[] things)
    {
        foreach (string strr in things)
        {
            thing = GameObject.Find(strr);
            if (thing != null)
            {
                thing.transform.position = new Vector3(0.0f, -0.5f, 0.0f);
            }
        }
    }

    void MoveAllTo()
    {
        using (var reader = new StreamReader(pathCSV))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                thing = GameObject.Find(values[0].Replace("AA", "BB"));
                if (thing != null)
                {
                    float x = float.Parse(values[1], CultureInfo.InvariantCulture.NumberFormat);
                    float y = float.Parse(values[2], CultureInfo.InvariantCulture.NumberFormat);
                    float z = float.Parse(values[3], CultureInfo.InvariantCulture.NumberFormat);
                    float rx = float.Parse(values[4], CultureInfo.InvariantCulture.NumberFormat);
                    float ry = float.Parse(values[5], CultureInfo.InvariantCulture.NumberFormat);
                    float rz = float.Parse(values[6], CultureInfo.InvariantCulture.NumberFormat);
                    float sx = float.Parse(values[7], CultureInfo.InvariantCulture.NumberFormat);
                    float sy = float.Parse(values[8], CultureInfo.InvariantCulture.NumberFormat);
                    float sz = float.Parse(values[9], CultureInfo.InvariantCulture.NumberFormat);
                    thing.transform.position = new Vector3(x, y, z);
                    thing.transform.eulerAngles = new Vector3(rx, ry, rz);
                    thing.transform.localScale = new Vector3(sx, sy, sz);
                }
            }
        }
    }

    void RecordPositions()
    {
        var csv = new StringBuilder();

        // write to json file
        using (FileStream fs = File.Create(pathJSON))
        {
            // write first line
            byte[] line = new UTF8Encoding(true).GetBytes("{\n");
            fs.Write(line, 0, line.Length);
            //Debug.Log(things);

            foreach (string strr in things)
            {

                var str = string.Copy(strr);
                if (recordPositionsAA)
                    str = str.Replace("BB", "AA");

                thing = GameObject.Find(str);
                if (thing != null)
                {
                    //var position = thing.transform.position.ToString().Replace("(", string.Empty).Replace(")", string.Empty);
                    var x = thing.transform.position.x;
                    var y = thing.transform.position.y;
                    var z = thing.transform.position.z;

                    var rx = thing.transform.eulerAngles.x;
                    var ry = thing.transform.eulerAngles.y;
                    var rz = thing.transform.eulerAngles.z;

                    var sx = thing.transform.localScale.x;
                    var sy = thing.transform.localScale.y;
                    var sz = thing.transform.localScale.z;

                    Vector3 boxSize = thing.GetComponent<Renderer>().bounds.size;
                    var wx = boxSize.x * tableSize;
                    var wz = boxSize.z * tableSize;
                    var handle_x = x * tableSize - 0.5 * wx;
                    var handle_z = -z * tableSize - 0.5 * wz;

                    var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}",
                        str, x, y, z, rx, ry, rz, sx, sy, sz, handle_x, handle_z);

                    csv.AppendLine(newLine);
                    //Debug.Log(newLine);

                    if (recordPositionsAA)
                        str = str.Replace("AA", "BB");
                    var name1 = names[str];
                    newLine = string.Format("  \"{0}\": [{1:0.00000}, {2:0.0000}, {3:0.00000}, {4:0.0000}]", name1, handle_x, handle_z, wx, wz);
                    if (name1 != "coffee")
                        newLine = newLine + ",";

                    //if (name1 == "napkin")
                        //newLine = string.Format("  \"{0}\": [{1:0.00000}, {2:0.0000}, {3:0.00000}, {4:0.0000}]", name1, handle_x, handle_z, wy, wx) + ",";

                    line = new UTF8Encoding(true).GetBytes(newLine + "\n");
                    fs.Write(line, 0, line.Length);

                    //if (recordPositionsAA)
                    //thing.transform.position = new Vector3(0.0f, -0.5f, 0.0f);
                }
            }

            line = new UTF8Encoding(true).GetBytes("}");
            fs.Write(line, 0, line.Length);
        }

        // write to csv file
        if (recordPositionsAA)
            pathCSV = pathCSV.Replace("BB", "AA");

        File.WriteAllText(pathCSV, csv.ToString());
        ScreenshotHandler.TakeScreenshot_Static(448, 448, namehaha.Replace("configs_", "shot_")+".png");

    }

    // Update is called once per frame
    void Update()
    {

    }
}
