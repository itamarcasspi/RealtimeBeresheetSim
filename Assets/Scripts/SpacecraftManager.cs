using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpacecraftManager : MonoBehaviour
{
    public static double MAX_I = 100;
    static bool firstTime = true;

    double vs = 25.5327623529411;
    double hs = 932;
    double gacc;
    double NN = 0.00421;
    double fuel = 121; //
    double engineForce;
    double engineAcceleration;
    double ang = 60; // zero is vertical (as in landing)
    double alt = 13748; // 2:25:40 (as in the simulation) // https://www.youtube.com/watch?v=JJ0VfRL9AMs
    double time = 0;
    double dt = 1; // sec
    double acc = 0; // Acceleration rate (m/s^2)
    double weight;
    double ang_rad;
    double h_acc;
    double v_acc;
    Bereshit bereshit;
    double P = 0.04;
    double I = 0.0003;
    double D = 0.2;
    double currentPidUpdate = 0.7;
    double lastPidUpdate;
    PID pid;

    double dvs;

    public static double accMax(double weight)
    {
        return get_acc(weight, true, 8);
    }

    public static double get_acc(double weight, bool main, int seconds)
    {
        double t = 0;
        if (main)
        {
            t += Bereshit.MAIN_ENG_F;
        }
        t += seconds * Bereshit.SECOND_ENG_F;
        double ans = t / weight;
        return ans;
    }

    public double toRadians(double angle)
    {
        return (Math.PI / 180) * angle;
    }

    public static double getDVS(double alt)
    {
        if (alt > 6000) return 30;
        if (alt > 1000) return 24;
        if (alt > 750) return 12;
        if (alt > 40) return 6;
        if (alt > 15) return 2;
        return 1;
    }
    //Unity fields:
    private float deltaTime;

    [SerializeField] Text showHS;
    [SerializeField] Text showVS;

    [SerializeField] Text showAltitude;
    [SerializeField] Text showAngle;
    [SerializeField] Text showFuel;
    [SerializeField] Text showPID;
    [SerializeField] Text showP;
    [SerializeField] Text showI;
    [SerializeField] Text showD;
    [SerializeField] Text showLanding;
    [SerializeField] Text showTime;
    [SerializeField] Button showPIDbtn;

    [SerializeField] InputField inputDVS;

    [SerializeField] InputField inputAngle;

    [SerializeField ] Slider timeSlider;

    [SerializeField] Button pauseButton;

    [SerializeField] Text runStatus;

    private int dvsFromInput;

    private bool landed;

    private bool SimPaused;

    private bool autoPID;

    private bool autoAngle;

    [SerializeField] Button autoAngleBtn;

    private double desiredAngle;

    private double startingAngle;

    public void setPidOn()
    {
        if(autoPID)
        {
            autoPID = false;
            showPIDbtn.GetComponentInChildren<Text>().text = "OFF";
            showPIDbtn.GetComponentInChildren<Image>().color = Color.red;
            inputDVS.interactable = true;
            inputDVS.image.color = Color.white;
            
        }
        else
        {
            autoPID = true;
            showPIDbtn.GetComponentInChildren<Text>().text = "ON";
            showPIDbtn.GetComponentInChildren<Image>().color = Color.green;
            inputDVS.interactable = false;
            inputDVS.image.color = new Color(193,193,193);
            
        }
    }

    public void setAutoAngle()
    {
        if(autoAngle)
        {
            autoAngle = false;
            autoAngleBtn.GetComponentInChildren<Text>().text = "OFF";
            autoAngleBtn.GetComponentInChildren<Image>().color = Color.red;
            inputAngle.interactable = true;
            inputAngle.image.color = Color.white;
            
        }
        else
        {
            autoAngle = true;
            autoAngleBtn.GetComponentInChildren<Text>().text = "ON";
            autoAngleBtn.GetComponentInChildren<Image>().color = Color.green;
            inputAngle.interactable = false;
            inputAngle.image.color = new Color(193,193,193);
            
        }
    }

    public void pausePress()
    {
        if(SimPaused)
        {
            Time.timeScale = 1;
            SimPaused = false;
            pauseButton.GetComponentInChildren<Text>().text = "Pause";
            runStatus.text = "Running";
            runStatus.color = Color.green;
        }
        else
        {
            Time.timeScale = 0;
            SimPaused = true;
            pauseButton.GetComponentInChildren<Text>().text = "Resume";
            runStatus.text = "Paused";
            runStatus.color = Color.red;
        }

    }

    public void MenuPress()
    {
        SceneManager.LoadScene("Menu");
    }

    
    public void readStringInput(string s)
    {
        dvsFromInput = int.Parse(s);
    }

    public void readAngleInput(string s)
    {
        desiredAngle = double.Parse(s);
    }

    public void getSliderValue()
    {
        deltaTime = timeSlider.value;
    }


    // Start is called before the first frame update
    void Start()
    {
        vs = SimulatorManager.Instance.getVs();
        hs = SimulatorManager.Instance.getHs();
        alt = SimulatorManager.Instance.getAltitude();
        ang = SimulatorManager.Instance.getAngle();
        fuel = SimulatorManager.Instance.getFuelMass();


        P = SimulatorManager.Instance.getPValue();
        I = SimulatorManager.Instance.getIValue();
        D = SimulatorManager.Instance.getDValue();

        engineForce = Bereshit.TOTAL_THRUST * NN;
        weight = Bereshit.WEIGHT_EMP + fuel;
        bereshit = new Bereshit(hs, vs, alt, ang, weight, fuel);
        pid = new PID(P, I, D);
        autoPID = true;
        landed = false;
        dvsFromInput = (int)getDVS(alt);

        //set DVS input to grey
        inputDVS.interactable = false;
        inputDVS.image.color = new Color(193,193,193);

        //set Angle input to grey
        inputAngle.interactable = false;
        inputAngle.image.color = new Color(193,193,193);

        autoAngle = true;
        

        //set PID button to green
        showPIDbtn.image.color = Color.green;

        //set autoAngle button to green
        autoAngleBtn.image.color = Color.green;

        //get delta time from slider
        deltaTime = timeSlider.value;

        //save starting angle
        startingAngle = ang;

    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        if(landed)  
        {
            return;
        }
        Time.fixedDeltaTime = deltaTime;
        double actionsPerSecond = 1 / deltaTime;
        for (int i = 0; i < actionsPerSecond; i++)
        {
            if(autoPID)
            {
                dvs = getDVS(alt);
            }
            else
            {
                dvs = dvsFromInput;
            }
            double error = vs - dvs;
            if (firstTime)
            {
                currentPidUpdate = pid.update(error, dt, vs);
                NN = 0.7;
            }
            else
            {
                lastPidUpdate = currentPidUpdate;
                currentPidUpdate = pid.update(error, dt, vs);
                double lastNN = NN;
                NN = Math.Max(Math.Min(lastNN + lastPidUpdate, 1), 0);
            }


            // over 2 km above the ground
            // lower than 2 km - horizontal speed should be close to zero
            if(!autoAngle)
            {
                if(ang > desiredAngle)
                {
                    ang -= 1;
                }
                else if(ang < desiredAngle)
                {
                    ang += 1;
                }
                
            }
            
            else if (alt < 1500)
            {
                if (alt > 1000 && ang > 40)
                {
                    ang -= 0.5;
                }
                else if (alt > 700 && ang > 1.5)
                {
                    ang -= 1.3;
                } // rotate to vertical position.
                else if (ang > 0.4)
                {
                    ang -= 0.4;

                }
                else
                {
                    ang = 0;
                }
                
                if (hs < 2)
                {
                    hs = 0;
                }
            }
            // else 
            // {
            //     if(ang < desiredAngle)
            //     {
            //         ang += 1;
            //     }
            //     else if(ang > desiredAngle)
            //     {
            //         ang -= 1;
            //     }
            // }
            // main computations
            engineForce = Bereshit.TOTAL_THRUST * NN;
            engineAcceleration = engineForce / (Bereshit.WEIGHT_EMP + fuel);
            ang_rad = toRadians(ang);
            h_acc = Math.Sin(ang_rad) * engineAcceleration;
            v_acc = Math.Cos(ang_rad) * engineAcceleration;
            gacc = Moon.getAcc(hs);

            time += dt;
            double dw = dt * Bereshit.ALL_BURN * NN;
            if (fuel > 0)
            {
                fuel -= dw;
                weight = Bereshit.WEIGHT_EMP + fuel;
                acc = NN * accMax(weight);
            }
            else
            { // ran out of fuel
                acc = 0;
            }

            //            v_acc -= gacc;
            if (hs > 0 && !firstTime)
            {
                hs = hs - h_acc;
            }
            vs = vs + gacc - v_acc;
            alt -= dt * vs;
            firstTime = false;
            
            //altitude state + landing state
            if(alt>10000)
            {
                showAltitude.color = UnityEngine.Color.red;
            }
            if(alt < 5000 && alt > 1000)
            {
                showAltitude.color = UnityEngine.Color.yellow;
            }
            else if(alt > 2 && alt < 1000)
            {
                showAltitude.color = UnityEngine.Color.white;
            }
            else if(alt < 2)
            {
                showAltitude.color = UnityEngine.Color.green;
                showLanding.text = "Landed";
                showLanding.color = UnityEngine.Color.green;
                landed = true;
                alt = 0;
                vs = 0;
                dvs = 0;
            }

            showHS.text = hs.ToString("0.0");
            showVS.text = vs.ToString("0.0");
            showAltitude.text = alt.ToString("0");
            showAngle.text = ang.ToString("0");
            showFuel.text = fuel.ToString("0");
            showPID.text = currentPidUpdate.ToString("0.00000");
            showP.text = pid.getProposal().ToString("0.000000");
            showI.text = pid.getIntegral().ToString("0.000000");
            showD.text = pid.getDerivative().ToString("0.000000");
            showTime.text = time.ToString("0");
            if(autoPID)
            {
                inputDVS.text = dvs.ToString("0");
            }
            

            
            
            

            //is VS close to DVS?
            if(Math.Abs(vs - dvs) < 1)
            {
                showVS.color = UnityEngine.Color.green;
            }
            else if(Math.Abs(vs - dvs) < 4)
            {
                showVS.color = UnityEngine.Color.yellow;
            }
            else 
            {
                showVS.color = UnityEngine.Color.red;
            }

            //fuel state
            if(fuel>100)
            {
                showFuel.color = UnityEngine.Color.green;
            }
            else if(fuel>50)
            {
                showFuel.color = UnityEngine.Color.yellow;
            }
            else if(fuel>20)
            {
                showFuel.color = UnityEngine.Color.red;
            }
            






        }
    }

}

