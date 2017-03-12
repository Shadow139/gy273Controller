using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO.Ports;

public class Cube : MonoBehaviour {

    //Textfields for Euler
    [SerializeField]
    private Text xRotTxt;
    [SerializeField]
    private Text yRotTxt;
    [SerializeField]
    private Text zRotTxt;

    //Textfields for Quaternions
    [SerializeField]
    private Text xQuatTxt;
    [SerializeField]
    private Text yQuatTxt;
    [SerializeField]
    private Text zQuatTxt;
    [SerializeField]
    private Text wQuatTxt;

    //Textfields for Event Acceleration
    [SerializeField]
    private Text xEventTxt;
    [SerializeField]
    private Text yEventTxt;
    [SerializeField]
    private Text zEventTxt;

    //Textfields for Line Acceleration
    [SerializeField]
    private Text xLineTxt;
    [SerializeField]
    private Text yLineTxt;
    [SerializeField]
    private Text zLineTxt;

    //Textfields for Joystick
    [SerializeField]
    private Text joyBtnTxt;
    [SerializeField]
    private Text joyXTxt;
    [SerializeField]
    private Text joyYTxt;

    //Textfields for Calibration
    [SerializeField]
    private Text magTxt;
    [SerializeField]
    private Text accTxt;
    [SerializeField]
    private Text gyrTxt;
    [SerializeField]
    private Text sysTxt;

    [SerializeField]
    private GameObject compass;

    [SerializeField]
    private Image joystickImg;
    [SerializeField]
    private Image joystockBG;

    public string PortName = "";
    public int Baud = 38400;
    public int RebootDelay = 3;

    //SerialPort sp = new SerialPort("COM1", 38400);
    SerialPort sp;

    Vector3 accelerationOld;
    Vector3 accelerationDelta;

    float prev = 0;

    // Use this for initialization
    void Start () {
        if (PortName == null || PortName.Length == 0 && guessPortName().Length > 0)
        {
            PortName = guessPortName();
        }
        Debug.Log(PortName);
        sp = new SerialPort(PortName, Baud);

        sp.Open();
        sp.ReadTimeout = 1;
	}


    // Update is called once per frame
    void Update() {

        if (sp.IsOpen && sp != null)
        {   
            try
            {
                string line = sp.ReadLine();
                
                //string[] dataChunks = line.Split('|');

                //foreach (var s in dataChunks)
                //{

                    string[] split = line.Split(' ');

                    if (split[0].Length > 0)
                    {

                        float x, y, z, w;
                        int a, b, c, d;
                        switch (split[0])
                        {
                            case "Q":
                                x = float.Parse(split[1]);
                                y = float.Parse(split[2]);
                                z = float.Parse(split[3]);
                                w = float.Parse(split[4]);


                                xQuatTxt.text = "Xq: " + x;
                                yQuatTxt.text = "Yq: " + y;
                                zQuatTxt.text = "Zq: " + z;
                                wQuatTxt.text = "Wq: " + w;

                                transform.rotation = new Quaternion(y, -z, -x, w);

                                break;

                            case "A":
                                x = float.Parse(split[1]);
                                y = float.Parse(split[2]);
                                z = float.Parse(split[3]);

                                xEventTxt.text = "XAccEvent: " + x;
                                yEventTxt.text = "YAccEvent: " + y;
                                zEventTxt.text = "ZAccEvent: " + z;


                                break;

                            case "L":
                                x = float.Parse(split[1]);
                                y = float.Parse(split[2]);
                                z = float.Parse(split[3]);



                                xLineTxt.text = "XAccLine: " + x;
                                yLineTxt.text = "YAccLine: " + y;
                                zLineTxt.text = "ZAccLine: " + z;


                                break;

                            case "J":
                                a = int.Parse(split[1]);
                                b = int.Parse(split[2]);
                                c = int.Parse(split[3]);

                                joyBtnTxt.text = "JoystickBtn: " + a;
                                joyXTxt.text = "JoystickX: " + b;
                                joyYTxt.text = "JoystickY: " + c;

                                if (a == 0)
                                {
                                    joystickImg.color = Color.green;
                                    transform.position = Vector3.zero;
                                }
                                else
                                {
                                    joystickImg.color = Color.red;
                                }

                                Vector2 pos;
                                pos.x = b;
                                pos.y = c;

                                pos.x = (pos.x - 512) / 1024.0f;
                                pos.y = (pos.y - 512) / 1024.0f;

                                joystickImg.rectTransform.anchoredPosition = new Vector3(pos.y * joystockBG.rectTransform.sizeDelta.y,
                                    pos.x * joystockBG.rectTransform.sizeDelta.x);

                                break;

                            case "C":
                                a = int.Parse(split[1]);
                                b = int.Parse(split[2]);
                                c = int.Parse(split[3]);
                                d = int.Parse(split[4]);

                                magTxt.text = "mag: " + a;
                                accTxt.text = "acc: " + c;
                                gyrTxt.text = "gyr: " + d;
                                sysTxt.text = "sys: " + a;
                                break;

                            default:
                                break;
                        }
                    }
                //}
                

                /*if (s.Contains("H"))
                {
                    string compRot = s.Split(" "[0])[1];
                    float compassRotation = float.Parse(compRot);

                    compass.transform.rotation = Quaternion.Euler(0f,compassRotation,0f);
                    return;
                }

                if (s.Contains("Inertial"))
                {
                    string xRot = s.Split(" "[0])[1];
                    string yRot = s.Split(" "[0])[2];
                    string zRot = s.Split(" "[0])[3];

                    xRotTxt.text = "X: " + yRot;
                    yRotTxt.text = "Y: " + xRot;
                    zRotTxt.text = "Z: " + zRot;

                    rot.x = -float.Parse(yRot);
                    rot.y = float.Parse(xRot);
                    rot.z = float.Parse(zRot);
                }


                if (s.Contains("Quaternion"))
                {
                    string xQuat = s.Split(" "[0])[1];
                    string yQuat = s.Split(" "[0])[2];
                    string zQuat = s.Split(" "[0])[3];
                    string wQuat = s.Split(" "[0])[3];

                    xQuatTxt.text = "Xq: " + xQuat;
                    yQuatTxt.text = "Yq: " + yQuat;
                    zQuatTxt.text = "Zq: " + zQuat;
                    zQuatTxt.text = "Wq: " + wQuat;


                    quat.x = -float.Parse(xQuat);
                    quat.y = float.Parse(yQuat);
                    quat.z = float.Parse(zQuat);
                    quat.z = float.Parse(wQuat);
                }

                if (s.Contains("eventAcc"))
                {
                    string xAcc = s.Split(" "[0])[1];
                    string yAcc = s.Split(" "[0])[2];
                    string zAcc = s.Split(" "[0])[3];

                    xEventTxt.text = "XAccEvent: " + xAcc;
                    yEventTxt.text = "YAccEvent: " + yAcc;
                    zEventTxt.text = "ZAccEvent: " + zAcc;

                    eventAcc.x = -float.Parse(xAcc);
                    eventAcc.y = float.Parse(yAcc);
                    eventAcc.z = float.Parse(zAcc);
                    accelerationDelta = accelerationOld - eventAcc;
                    //moveObject(accelerationDelta);

                    accelerationOld = eventAcc;
                    Debug.Log(accelerationDelta);
                }

                if (s.Contains("lineAcc"))
                {
                    string xAcc = s.Split(" "[0])[1];
                    string yAcc = s.Split(" "[0])[2];
                    string zAcc = s.Split(" "[0])[3];

                    xLineTxt.text = "XAccLine: " + xAcc;
                    yLineTxt.text = "YAccLine: " + yAcc;
                    zLineTxt.text = "ZAccLine: " + zAcc;

                    lineAcc.x = -float.Parse(xAcc);
                    lineAcc.y = float.Parse(zAcc);
                    lineAcc.z = float.Parse(yAcc);

                    //moveObject(lineAcc);
                }

                if (s.Contains("Joy"))
                {
                    string JoystickBtn = s.Split(" "[0])[1];
                    string xJoystick = s.Split(" "[0])[2];
                    string yJoystick = s.Split(" "[0])[3];

                    joyBtnTxt.text = "JoystickBtn: " + JoystickBtn;
                    joyXTxt.text = "JoystickX: " + xJoystick;
                    joyYTxt.text = "JoystickY: " + yJoystick;

                    int btn = int.Parse(JoystickBtn);

                    if (btn == 0)
                    {
                        joystickImg.color = Color.green;
                        transform.position = Vector3.zero;
                    }
                    else
                    {
                        joystickImg.color = Color.red;
                    }

                    Vector2 pos;
                    pos.x = float.Parse(xJoystick);
                    pos.y = float.Parse(yJoystick);

                    pos.x = (pos.x - 512) / 1024.0f;
                    pos.y = (pos.y - 512) / 1024.0f;
                    
                    joystickImg.rectTransform.anchoredPosition = new Vector3(pos.y * joystockBG.rectTransform.sizeDelta.y,
                        pos.x * joystockBG.rectTransform.sizeDelta.x);
                }
                */

            }
            catch (System.Exception e)
            {
                //bug.Log("FAIL");
            }

            //rotateObject(rot);
            
        }
    }
    void readEuler()
    {

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

    void moveObject(Vector3 movement)
    {
        transform.Translate(movement);
    }

    public static string guessPortName()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.OSXPlayer:
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXDashboardPlayer:
            case RuntimePlatform.LinuxPlayer:
                return guessPortNameUnix();

            default:
                return guessPortNameWindows();
        }

        //return guessPortNameUnix();
    }

    public static string guessPortNameWindows()
    {        
        var devices = System.IO.Ports.SerialPort.GetPortNames();

        if (devices.Length == 0) // 
        {
            return "COM3"; // probably right 50% of the time		
        }
        else if(devices.Length >= 2)
        {
            return devices[1];
        }
        else
            return devices[0];
    }

    public static string guessPortNameUnix()
    {
        var devices = System.IO.Ports.SerialPort.GetPortNames();

        if (devices.Length == 0) // try manual enumeration
        {
            devices = System.IO.Directory.GetFiles("/dev/");
        }
        string dev = ""; ;
        foreach (var d in devices)
        {
            if (d.StartsWith("/dev/tty.usb") || d.StartsWith("/dev/ttyUSB"))
            {
                dev = d;
                Debug.Log("Guessing that arduino is device " + dev);
                break;
            }
        }
        return dev;
    }
}
