using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulatorManager : MonoBehaviour
{
    public static SimulatorManager Instance { get; private set; }
    // Start is called before the first frame update
    private double pValue;
    private double iValue;

    private double dValue;

    private double altitude;

    private double fuelMass;

    private double hs;

    private double vs;

    private double Angle;

    
    
 

    void Awake()
    {
        pValue = 0.04;
        iValue = 0.0003;
        dValue = 0.2;
        altitude = 13748;
        fuelMass = 121;
        hs = 932;
        vs = 25.5327623529411;
        Angle = 60;
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            //Debug.Log("SimulatorManager Instance created");
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    //getters for all private fields
    public double getPValue()
    {
        return pValue;
    }

    public double getIValue()
    {
        return iValue;
    }

    public double getDValue()
    {
        return dValue;
    }

    public double getAltitude()
    {
        return altitude;
    }

    public double getFuelMass()
    {
        return fuelMass;
    }

    public double getHs()
    {
        return hs;
    }

    public double getVs()
    {
        return vs;
    }

    public double getAngle()
    {
        return Angle;
    }

    //setters for all private fields
    public void setPValue(double p)
    {
        pValue = p;
    }

    public void setIValue(double i)
    {
        iValue = i;
    }

    public void setDValue(double d)
    {
        dValue = d;
    }

    public void setAltitude(double a)
    {
        altitude = a;
    }

    public void setFuelMass(double f)
    {
        fuelMass = f;
    }

    public void setHs(double h)
    {
        hs = h;
    }

    public void setVs(double v)
    {
        vs = v;
    }

    public void setAngle(double a)
    {
        Angle = a;
    }





    void Start()
    {
        pValue = 0.04;
        iValue = 0.0003;
        dValue = 0.2;
        altitude = 13748;
        fuelMass = 121;
        hs = 932;
        vs = 25.5327623529411;
        Angle = 60;



    }

    
    


}
