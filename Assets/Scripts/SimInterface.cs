using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimInterface : MonoBehaviour
{
    [SerializeField] InputField pValueInput;
    [SerializeField] InputField iValueInput;
    [SerializeField] InputField dValueInput;
    [SerializeField] InputField altitudeInput;
    [SerializeField] InputField fuelMassInput;
    [SerializeField] InputField hsInput;
    [SerializeField] InputField vsInput;
    [SerializeField] Button SimulateBtn;

    [SerializeField] InputField angleInput;

    
    // Start is called before the first frame update

    
    void Start()
    {
        pValueInput.text = SimulatorManager.Instance.getPValue().ToString();
        iValueInput.text = SimulatorManager.Instance.getIValue().ToString();
        dValueInput.text = SimulatorManager.Instance.getDValue().ToString();
        altitudeInput.text = SimulatorManager.Instance.getAltitude().ToString();
        fuelMassInput.text = SimulatorManager.Instance.getFuelMass().ToString();
        hsInput.text= SimulatorManager.Instance.getHs().ToString();
        vsInput.text = SimulatorManager.Instance.getVs().ToString();
        angleInput.text = SimulatorManager.Instance.getAngle().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void pressSimulate()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void readPinput(string s)
    {
        SimulatorManager.Instance.setPValue(double.Parse(s));
    }

    public void readIinput(string s)
    {
        SimulatorManager.Instance.setIValue(double.Parse(s));
    }

    public void readDinput(string s)
    {
        SimulatorManager.Instance.setDValue(double.Parse(s));
    }

    public void readAltitudeInput(string s)
    {
        SimulatorManager.Instance.setAltitude(double.Parse(s));
    }

    public void readFuelMassInput(string s)
    {
        SimulatorManager.Instance.setFuelMass(double.Parse(s));
    }

    public void readHsInput(string s)
    {
        SimulatorManager.Instance.setHs(double.Parse(s));
    }

    public void readVsInput(string s)
    {
        SimulatorManager.Instance.setVs(double.Parse(s));
    }

    public void readAngleInput(string s)
    {
        SimulatorManager.Instance.setAngle(double.Parse(s));
    }
}
