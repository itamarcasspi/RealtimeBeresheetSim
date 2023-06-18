using System;

public class Moon {
	// from: https://he.wikipedia.org/wiki/%D7%94%D7%99%D7%A8%D7%97
	public static double RADIUS = 3475*1000; // meters
	public static double ACC = 1.622;// m/s^2
	public static double EQ_SPEED = 1700;// m/s
	
	public static double getAcc(double speed) {
		double n = Math.Abs(speed)/EQ_SPEED;
		double ans = (1-n)*ACC;
		return ans;
	}
}
