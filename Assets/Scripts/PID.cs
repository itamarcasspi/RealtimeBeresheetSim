/**
 * PID Class to control spaceship landing,
 * Need to define some constants and the desired speed,
 * We created this class because we have some variables that's need control
 * DVS = Desired Vertical Speed, DHS = Desired Horizontal Speed;
 */
public class PID {
    // Define Constants for each pid
    private static double EPS = 0.00001;
    private double currentVs;
    private double lastVs;
    private double P;
    private double D;
    private double I;
    private double integral;
    private bool firstTime;
    private double derivative;
    private double proposal;
    private double controlOut;

    public double getDerivative() {
        return derivative;
    }

    public double getProposal() {
        return proposal;
    }

    public double getControlOut() {
        return controlOut;
    }

    public double update(double error, double dt, double vs) {
        if (firstTime) {
            firstTime = false;
            derivative = 0;
            proposal = error;
            integral = proposal;
            currentVs = vs;
            controlOut = P * proposal + (D * derivative) + (I * integral);
            return controlOut + EPS;
        }
        lastVs = currentVs;
        currentVs = vs;
        proposal = error;
        integral += error;
        derivative = currentVs - lastVs;
        controlOut = P * proposal + (D * derivative) + (I * integral);
        return controlOut + EPS;
    }




    public PID(double cp, double ci, double cd) {
        firstTime = true;
        this.P = cp;
        this.I = ci;
        this.D = cd;

    }


    public double getIntegral() {
        return integral;
    }


    
}

