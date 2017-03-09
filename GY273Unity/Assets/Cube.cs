using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO.Ports;

public class Cube : MonoBehaviour {

    [SerializeField]
    private Text xTxt;
    [SerializeField]
    private Text yTxt;
    [SerializeField]
    private Text zTxt;

    SerialPort sp = new SerialPort("COM4", 9600);

	// Use this for initialization
	void Start () {
        sp.Open();
        sp.ReadTimeout = 1;
	}
	
	// Update is called once per frame
	void Update () {

        if (sp.IsOpen)
        {
            Vector3 rot = new Vector3();
            
            try
            {
                string s = sp.ReadLine();
                //Debug.Log(s);


                string xRot = s.Split(" "[0])[0];
                string yRot = s.Split(" "[0])[1];
                string zRot = s.Split(" "[0])[2];

                xTxt.text = "X: " + xRot;
                yTxt.text = "Y: " + yRot;
                zTxt.text = "Z: " + zRot;

                rot.x = float.Parse(xRot);
                rot.y = float.Parse(yRot);
                rot.z = float.Parse(zRot);

            }
            catch (System.Exception)
            {

            }

            rotateObject(rot);
        }
    }

    void rotateObject(Vector3 rotation)
    {
        if(rotation != Vector3.zero)
        {
           // Debug.Log(rotation);
            transform.rotation = Quaternion.Euler(rotation);
            //transform.Rotate(rotation);
        }
    }
}
