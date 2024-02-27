namespace Nest4CSharp.Util;

/**
 * @author yisa
 */
public class Config {
    public  static int CLIIPER_SCALE = 10000;
    public  static double CURVE_TOLERANCE  = 0.02;
    public  double SPACING ;
    public  int POPULATION_SIZE;
    public  int MUTATION_RATE ;
    private  bool CONCAVE ;
    public   bool USE_HOLE ;


    public Config() {
        CLIIPER_SCALE = 10000;
        CURVE_TOLERANCE = 0.3;
        SPACING = 10;
        POPULATION_SIZE = 10;
        MUTATION_RATE = 10;
        CONCAVE = false;
        USE_HOLE = false;
    }

    public bool isCONCAVE() {
        return CONCAVE;
    }

    public bool isUSE_HOLE() {
        return USE_HOLE;
    }
}
