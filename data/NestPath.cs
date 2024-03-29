namespace Nest4CSharp.Data;

using Nest4CSharp.Util;
using Clipper2Lib;

public class NestPath: IComparable<NestPath> {
    private List<Segment> segments;
    private List<NestPath> children;
    private NestPath parent;
    public double offsetX;
    public double offsetY;

    private int id;
    private int source ;
    private int rotation;
    public Config config ;
    public double area ;

    public int bid;


    public void add(double x , double y ){
        this.add(new Segment(x,y));
    }

    public override bool Equals(Object obj) {
        NestPath nestPath = (NestPath) obj;
        if(segments.Count() != nestPath.Count()){
            return false;
        }
        for(int  i =0 ; i <segments.Count(); i ++){
            if(!segments.ElementAt(i).Equals(nestPath.get(i))){
                return false;
            }
        }
        if(children.Count() != nestPath.getChildren().Count()){
            return false;
        }
        for(int i = 0 ; i<children.Count(); i ++){
            if(!children.ElementAt(i).Equals(nestPath.getChildren().ElementAt(i))){
                return false;
            }
        }
        return true;
    }

    public Config getConfig() {
        return config;
    }

    public void setConfig(Config config) {
        this.config = config;
    }

    /**
     * 丢弃最后一个segment
     */
    public void pop(){
        segments.RemoveAt(segments.Count()-1);
    }

    public void reverse() {
        List<Segment> rever = new List<Segment>();
        for(int i = segments.Count()-1; i >=0; i -- ){
            rever.Add(segments.ElementAt(i));
        }
        segments.Clear();
        foreach(Segment s in rever) {
            segments.Add(s);
        }
    }

    public Segment get(int i){
        return segments.ElementAt(i);
    }

    public NestPath getParent() {
        return parent;
    }

    public void SetParent(NestPath parent) {
        this.parent = parent;
    }

    public void addChildren(NestPath nestPath) {
        children.Add(nestPath);
        nestPath.SetParent(this);
    }

    
    public override String ToString() {
        String res = "";
        res += "id = "+ id+" , source = "+ source +" , rotation = " + rotation +"\n";
        int count = 0;
        foreach(Segment s in segments){
            res += "Segment " + count +"\n";
            count++;
            res+= s.ToString() +"\n";
        }
        count = 0 ;
        foreach(NestPath nestPath in children){
            res += "children "+ count +"\n";
            count++;
            res += nestPath.ToString();
        }
        return res;
    }

    public List<NestPath> getChildren() {
        return children;
    }

    public void setChildren(List<NestPath> children) {
        this.children = children;
    }

    public int getRotation() {
        return rotation;
    }

    public void setRotation(int rotation) {
        this.rotation = rotation;
    }

    public void setSegments(List<Segment> segments) {
        this.segments = segments;
    }

    public int getSource() {
        return source;
    }

    public void setSource(int source) {
        this.source = source;
    }

    public NestPath() {
        offsetX = 0;
        offsetY = 0;
        children = new List<NestPath>();
        segments = new List<Segment>();
        config = new Config();
        area = 0;
    }


    public NestPath(Config config) {
        offsetX = 0;
        offsetY = 0;
        children = new List<NestPath>();
        segments = new List<Segment>();
        area = 0;
        this.config = config;
    }

    public NestPath(NestPath srcNestPath) {
        segments = new List<Segment>();
        foreach(Segment segment in srcNestPath.getSegments() ){
            segments.Add(new Segment(segment));
        }

        this.id  = srcNestPath.id;
        this.rotation = srcNestPath.rotation;
        this.source = srcNestPath.source;
        this.offsetX = srcNestPath.offsetX;
        this.offsetY = srcNestPath.offsetY;
        this.bid = srcNestPath.bid;
        this.area = srcNestPath.area;
        children = new List<NestPath>();

        foreach(NestPath nestPath in srcNestPath.getChildren()) {
            NestPath child = new NestPath(nestPath);
            child.SetParent(this);
            children.Add(child);
        }
    }

    public static NestPath cleanNestPath(NestPath srcPath) {
        /**
         * Convert NestPath 2 Clipper
         */
        Path64 path = CommonUtil.NestPath2Path(srcPath);
        Path64 clean = Clipper.SimplifyPath(path, 0.25);
        
        /**
         *  Convert Clipper 2 NestPath
         */
        NestPath cleanPath = CommonUtil.Path2NestPath(clean);
        cleanPath.bid = srcPath.bid;
        cleanPath.setRotation(srcPath.rotation);
        return cleanPath;
    }

    /**
     * 通过平移将NestPath的最低x坐标，y坐标的值必定都是0，
     */
    public void Zerolize() {
        ZeroX();ZeroY();
    }

    private void ZeroX() {
        double xMin = Double.MaxValue;
        foreach(Segment s in segments) {
            if(xMin > s.getX() ){
                xMin = s.getX();
            }
        }
        foreach(Segment s in segments) {
            s.setX(s.getX() - xMin );
        }
    }

    private void ZeroY() {
        double yMin = Double.MaxValue;
        foreach(Segment s in segments) {
            if(yMin > s.getY() ){
                yMin = s.getY();
            }
        }
        foreach(Segment s in segments) {
            s.setY(s.getY() - yMin);
        }
    }

    public void clear(){
        segments.Clear();
    }

    public int Count(){
        return segments.Count();
    }

    public void add(Segment s){
        segments.Add(s);
    }

    public List<Segment> getSegments() {
        return segments;
    }


    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public double getOffsetX() {
        return offsetX;
    }

    public void setOffsetX(double offsetX) {
        this.offsetX = offsetX;
    }

    public double getOffsetY() {
        return offsetY;
    }

    public void setOffsetY(double offsetY) {
        this.offsetY = offsetY;
    }

    public int CompareTo(NestPath o) {
        double area0  = this.area;
        double area1 = o.area;
        if(area0 > area1) {
            return 1;
        }
        else if(area0 == area1){
            return 0;
        }
        return -1;
    }

    public double getMaxY() {
        double MaxY = Double.MinValue;
        foreach(Segment s in segments) {
            if(MaxY < s.getY()){
                MaxY = s.getY();
            }
        }
        return MaxY;
    }

    public void translate(double x,  double y) {
        foreach(Segment s in segments) {
            s.setX(s.getX() + x );
            s.setY(s.getY() + y);
        }
    }
}
