using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* --------------------------------------------------------------------
 * 
 *   A screenshot will be taken when the user presses the space key
 *      for testing the screenshot function
 * 
 -------------------------------------------------------------------- */

public class TestScreenshot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //ScreenshotHandler.TakeScreenshot_Static(224,224);
            ScreenshotHandler.TakeScreenshot_Static(448, 448, "data/output/haha.png");
            //MakePictures.MakeAllPictures();
        }
    }
}
