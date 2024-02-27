namespace Nest4CSharp.Data;


public class ParallelData {
    public NfpKey key ;
    public List<NestPath> value ;

    public ParallelData() {
        value = new List<NestPath>();
    }

    public ParallelData(NfpKey key, List<NestPath> value) {
        this.key = key;
        this.value = value;
    }

    public NfpKey getKey() {
        return key;
    }

    public void setKey(NfpKey key) {
        this.key = key;
    }

    public List<NestPath> getValue() {
        return value;
    }

    public void setValue(List<NestPath> value) {
        this.value = value;
    }
}