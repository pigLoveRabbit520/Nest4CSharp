namespace Nest4CSharp.Data;

using Clipper2Lib;

public class Vector {
    public double x;
    public double y;
    public int id;
    public int rotation;
    public Paths64 nfp;

    public Vector(double x, double y, int id, int rotation) {
        this.x = x;
        this.y = y;
        this.id = id;
        this.rotation = rotation;
        this.nfp = new Paths64();
    }

    public Vector(double x, double y, int id, int rotation, Paths64 nfp) {
        this.x = x;
        this.y = y;
        this.id = id;
        this.rotation = rotation;
        this.nfp = nfp;
    }

    public Vector() {
        nfp = new Paths64();
    }

    public override String ToString() {
        return  "x = "+ x+" , y = "+y ;
    }
}