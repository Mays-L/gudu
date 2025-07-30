using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class VersionController : MonoBehaviour
{
    // Start is called before the first frame update
    public string myVersion = "?.?";
    public string myBuildDate = "????/??/??";

    private Text myTextObj;
    private float fTime = 0.0f;
    private bool fBD = false;

    void Start()
    {
        myTextObj = this.GetComponent<Text>();
        myTextObj.text = "Version = " + myVersion + ", BuildDate = " + myBuildDate;
        myTextObj.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.V))
        {
            if (!fBD)
            {
                fTime = Time.time;
                fBD = true;
            }

            if ((Time.time-fTime > 5.0) && fBD)
            {
                myTextObj.enabled = true;
            }
        }

        if (Input.GetKeyUp(KeyCode.V))
        {
            fBD = false;
            myTextObj.enabled = false;
        }
    }
}
