namespace Nest4CSharp.Data;

public class NfpKey {

    int A;
    int B;
    bool inside;
    int Arotation;
    int Brotation;

    public NfpKey(int a, int b, bool inside, int arotation, int brotation) {
        A = a;
        B = b;
        this.inside = inside;
        Arotation = arotation;
        Brotation = brotation;
    }

    public NfpKey() {
    }

    public int getA() {
        return A;
    }

    public void setA(int a) {
        A = a;
    }

    public int getB() {
        return B;
    }

    public void setB(int b) {
        B = b;
    }

    public bool isInside() {
        return inside;
    }

    public void setInside(bool inside) {
        this.inside = inside;
    }

    public int getArotation() {
        return Arotation;
    }

    public void setArotation(int arotation) {
        Arotation = arotation;
    }

    public int getBrotation() {
        return Brotation;
    }

    public void setBrotation(int brotation) {
        Brotation = brotation;
    }
}
