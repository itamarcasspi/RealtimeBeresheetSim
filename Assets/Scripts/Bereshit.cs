public class Bereshit {

    public static double WEIGHT_EMP = 165; // kg
    public static double WEIGHT_FUEL = 420; // kg
    public static  double WEIGHT_FULL = WEIGHT_EMP + WEIGHT_FUEL; // kg
    // https://davidson.weizmann.ac.il/online/askexpert/%D7%90%D7%99%D7%9A-%D7%9E%D7%98%D7%99%D7%A1%D7%99%D7%9D-%D7%97%D7%9C%D7%9C%D7%99%D7%AA-%D7%9C%D7%99%D7%A8%D7%97
    public static double MAIN_ENG_F = 430; // N
    public static double SECOND_ENG_F = 25; // N
    public static double MAIN_BURN = 0.15; //liter per sec, 12 liter per m'
    public static double SECOND_BURN = 0.009; //liter per sec 0.6 liter per m'
    public static double ALL_BURN = MAIN_BURN + 8 * SECOND_BURN;
    public static double TOTAL_THRUST = 630;
    private double hs;
    private double vs;
    private double alt;
    private double ang;
    private double fullWeight;
    private double fuel;
    public Bereshit(double hs, double vs, double alt, double ang, double fullWeight, double fuel) {
        this.hs = hs;
        this.vs = vs;
        this.alt = alt;
        this.ang = ang;
        this.fullWeight = fullWeight;
        this.fuel = fuel;
    }

    public double getHs() {return hs;}
    public void setHs(double hs) {this.hs = hs;}
    public double getVs() {return vs;}
    public void setVs(double vs) {this.vs = vs;}
    public double getAlt() {return alt;}
    public void setAlt(double alt) {this.alt = alt;}
    public double getAng() {return ang;}
    public void setAng(double ang) {this.ang = ang;}
    public double getFullWeight() {return fullWeight;}
    public void setFullWeight(double fullWeight) {this.fullWeight = fullWeight;}
    public double getFuel() {return fuel;}
    public void setFuel(double fuel) {this.fuel = fuel;}
}

