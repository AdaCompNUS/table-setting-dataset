using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;


/* --------------------------------------------------------------------
 * 
 *   This script synthesizes the sequences of images that human workers 
 *      created on the drag-and-rotate web interface:
 *      https://nusrls.s3.amazonaws.com/HIT_set_table.html
 * 
 *   It takes in all the csv files in "data/input/", 
 *      reconfigure the table according to each csv file,
 *      and take a scrrenshot for each configuration of the table.
 * 
 *   Example input file name: HIT_34_configs_102_4.csv
 *      It is the 34 submission by human workers
 *      It starts with table configuration 102, which is the second configuration of video 1
 *      It is the fifth (start with index 0) scene of the action sequence
 *      Each row of the csv file corresponds to the location of one 
 *      The corresponding output file name is HIT_34_configs_102_4.png
 * 
 -------------------------------------------------------------------- */

public class MakePictures : MonoBehaviour
{
    public static Material[] materials = new Material[2];

    public static float tableSize = 448;
    public static GameObject thing;
    public static Renderer rend;

    public static bool recordPositionsAA = true;

    public static string inputPath = "data/input/";
    public static string outputPath = "data/output/";

    public static string[] things = { "Wine Glass BB", "Charger BB", "Napkin Cloth BB", "Table Cloth Sides", //"Napkin Cloth BB", "Coffee BB",
        "Water Glass BB", "Bread Plate BB", "Dinner Knife BB", "Salad Knife BB", "Butter Knife BB",
        "Salad Fork BB", "Dinner Fork BB", "Soup Spoon BB", "Dessert Fork BB", "Dessert Spoon BB",
        "Dinner Plate BB", "Wine Glass BB (1)", "Wine Glass BB (2)", "Coffee Plate BB"};

    public static IList<string> strList = new List<string>();


    /* --------------------------------------------------------------------
     * 
     *   MAIN
     * 
     -------------------------------------------------------------------- */


    // Start is called before the first frame update
    void Start()
    {
        //HideAllAA();

        DirectoryInfo d = new DirectoryInfo(inputPath);
        foreach (var file in d.GetFiles("*.csv"))
        {
            //Debug.Log(file.FullName + ", " + file.Name);
            strList.Add(file.Name);
        }

        materials[0] = Resources.Load<Material>("MetalPlates_36");
        materials[1] = Resources.Load<Material>("dish material 1 B");
    }



    // Update is called once per frame
    void Update()
    {
        if (strList.Count>0)
        {
            MoveAndShot(strList[0], things);
            strList.RemoveAt(0);
        }
    }


    // move each object according to 
    public static void MoveAndShot(string filePath, string[] things)
    {
        if (filePath.Contains("HIT_0_dummy"))
        {
            return;
        }

        var configNumber = filePath.Substring(filePath.IndexOf("s_"), 5);
        var configFileName = "data/locations/position" + configNumber + ".csv";

        using (var reader = new StreamReader(configFileName))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                thing = GameObject.Find(values[0]);
                if (thing != null)
                {
                    if (values[0] == "Salad Knife BB")
                    {
                        if (configNumber.Substring(2, 1) == "5")
                        {
                            thing.GetComponent<MeshRenderer>().enabled = false;
                            thing = GameObject.Find("Tea Spoon BB");
                            thing.GetComponent<MeshRenderer>().enabled = true;
                        }
                        else
                        {
                            GameObject thing2 = GameObject.Find("Tea Spoon BB");
                            thing2.GetComponent<MeshRenderer>().enabled = false;
                            thing.GetComponent<MeshRenderer>().enabled = true;
                        }
                    }

                    float x = float.Parse(values[1], CultureInfo.InvariantCulture.NumberFormat);
                    float y = float.Parse(values[2], CultureInfo.InvariantCulture.NumberFormat);
                    float z = float.Parse(values[3], CultureInfo.InvariantCulture.NumberFormat);
                    float rx = float.Parse(values[4], CultureInfo.InvariantCulture.NumberFormat);
                    float ry = float.Parse(values[5], CultureInfo.InvariantCulture.NumberFormat);
                    float rz = float.Parse(values[6], CultureInfo.InvariantCulture.NumberFormat);
                    float sx = float.Parse(values[7], CultureInfo.InvariantCulture.NumberFormat);
                    float sy = float.Parse(values[8], CultureInfo.InvariantCulture.NumberFormat);
                    float sz = float.Parse(values[9], CultureInfo.InvariantCulture.NumberFormat);
                    if ((values[0] == "Charger BB"))
                    {
                        y = (float)0.017;
                        rend = thing.GetComponent<Renderer>();
                        rend.enabled = true;
                        rend.sharedMaterial = materials[0];
                        if (configNumber.Substring(2, 1) == "1")
                        {
                            rend.sharedMaterial = materials[1];
                        }
                    }
                    else if (values[0] == "Butter Knife BB")
                    {
                        y = (float)0.032;
                    }

                    thing.transform.position = new Vector3(x, y, z);
                    thing.transform.eulerAngles = new Vector3(rx, ry, rz);
                    thing.transform.localScale = new Vector3(sx, sy, sz);
                }
            }
        }

        if(filePath.Contains("_0.csv"))
        {
            ScreenshotHandler.TakeScreenshot_Static(448, 448, outputPath + filePath.Replace("_0.csv", "_0.png"));
        }
        else
        {
            using (var reader = new StreamReader(inputPath + filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    thing = GameObject.Find(values[0]);
                    if (thing != null)
                    {
                        if (values[0] == "Salad Knife BB")
                        {
                            if (configNumber.Substring(2, 1) == "5")
                            {
                                thing.GetComponent<MeshRenderer>().enabled = false;
                                thing = GameObject.Find("Tea Spoon BB");
                                thing.GetComponent<MeshRenderer>().enabled = true;
                            }
                            else
                            {
                                GameObject thing2 = GameObject.Find("Tea Spoon BB");
                                thing2.GetComponent<MeshRenderer>().enabled = false;
                                thing.GetComponent<MeshRenderer>().enabled = true;
                            }
                        }

                        Vector3 boxSize = thing.GetComponent<Renderer>().bounds.size;

                        float x = float.Parse(values[1], CultureInfo.InvariantCulture.NumberFormat) + (float)0.5 * boxSize.x;
                        float y = thing.transform.position.y;
                        float z = -float.Parse(values[2], CultureInfo.InvariantCulture.NumberFormat) - (float)0.5 * boxSize.z;

                        float rx = thing.transform.eulerAngles.x;
                        float ry = thing.transform.eulerAngles.y + float.Parse(values[4], CultureInfo.InvariantCulture.NumberFormat);
                        float rz = thing.transform.eulerAngles.z;

                        if ((values[0] == "Charger BB"))
                        {
                            y = (float)0.017;
                            rend = thing.GetComponent<Renderer>();
                            rend.enabled = true;
                            rend.sharedMaterial = materials[0];
                            if (configNumber.Substring(2, 1) == "1")
                            {
                                rend.sharedMaterial = materials[1];
                            }
                        }
                        else if (values[0] == "Butter Knife BB")
                        {
                            y = (float)0.032;
                        }

                        thing.transform.position = new Vector3(x, y, z);
                        thing.transform.eulerAngles = new Vector3(rx, ry, rz);
                    }
                }
            }
            string name = outputPath + filePath.Replace(".csv", ".png");
            ScreenshotHandler.TakeScreenshot_Static(448, 448, name);
        }

    }

    public static void HideAllAA()
    {
        foreach (string strr in things)
        { 
            var str = string.Copy(strr);
            if (recordPositionsAA)
                str = str.Replace("BB", "AA");

            thing = GameObject.Find(str);
            if (thing != null)
                thing.SetActive(false);
        }
    }
}

public static class WaitFor
{
    public static IEnumerator Frames(int frameCount)
    {
        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
    }
}