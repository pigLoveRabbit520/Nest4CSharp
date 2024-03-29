namespace Nest4CSharp.Util;

using Nest4CSharp.Data;
using Nest4CSharp.Util.Coor;
using Clipper2Lib;

public class CommonUtil {


    public static NestPath Path2NestPath(Path64 path) {
        NestPath nestPath = new NestPath();
        for(int  i = 0; i < path.Count() ; i ++) {
            Point64 lp = path.ElementAt(i);
            NestCoor coor = CommonUtil.toNestCoor(lp.X, lp.Y);
            nestPath.add(new Segment(coor.getX() , coor.getY()));
        }
        return nestPath;
    }

    public static Path64 NestPath2Path(NestPath nestPath) {
        Path64 path = new Path64();
        foreach(Segment s in nestPath.getSegments()) {
            ClipperCoor coor = CommonUtil.toClipperCoor(s.getX() , s.getY());
            Point64 lp = new Point64(coor.getX() , coor.getY());
            path.Add(lp);
        }
        return path;
    }

    /**
     * 坐标转换
     * @param x
     * @param y
     * @return
     */
    public static ClipperCoor toClipperCoor(double x , double y ) {
        return new ClipperCoor((long)(x * Config.CLIIPER_SCALE) , (long)(y * Config.CLIIPER_SCALE));
    }

    /**
     * 坐标转换
     * @param x
     * @param y
     * @return
     */
    public static NestCoor toNestCoor(long x , long y ) {
        return new NestCoor((x/Config.CLIIPER_SCALE) , (y/Config.CLIIPER_SCALE));
    }


    /**
     * 为Clipper下的Path添加点
     * @param x
     * @param y
     * @param path
     */
    private static void addPoint(long x, long y , Path64 path) {
        Point64  ip = new Point64(x,y);
        path.Add(ip);
    }


    /**
     * binPath是作为底板的NestPath , polys则为板件的Path列表
     * 这个方法是为了将binPath和polys在不改变自身形状，角度的情况下放置在一个坐标系内，保证两两之间不交叉
     * @param binPath
     * @param polys
     */
    public static void ChangePosition(NestPath binPath , List<NestPath> polys){

    }

    /**
     *  将NestPath列表转换成父子关系的树
     * @param list
     * @param idstart
     * @return
     */
    public static int toTree(List<NestPath> list , int idstart){
        List<NestPath> parents = new List<NestPath>();
        int id = idstart;
        /**
         * 找出所有的内回环
         */
        for(int i = 0 ; i<list.Count(); i ++){
            NestPath p = list.ElementAt(i);
            bool isChild = false;
            for(int  j = 0 ; j < list.Count(); j++){
                if(j == i ){
                    continue;
                }

                if(GeometryUtil.pointInPolygon(p.getSegments().ElementAt(0) , list.ElementAt(j)) == true ){
                    list.get(j).getChildren().add(p);
                    p.setParent(list.get(j));
                    isChild = true;
                    break;
                }
            }
            if(!isChild){
                parents.add(p);
            }
        }
        /**
         *  将内环从list列表中去除
         */
        for(int i = 0; i <list.size() ; i ++){
            if(parents.indexOf(list.get(i)) < 0 ){
                list.remove(i);
                i--;
            }
        }

        for(int i = 0; i <parents.size() ; i ++){
            parents.get(i).setId(id);
            id ++;
        }

        for(int i = 0 ; i<parents.size() ; i ++){
            if(parents.get(i).getChildren().size() > 0 ){
                id = toTree(parents.get(i).getChildren(),id);
            }
        }
        return id;
    }

    public static NestPath clipperToNestPath(Path polygon){
        NestPath normal = new NestPath();
        for(int i = 0; i <polygon.size() ; i++){
            NestCoor nestCoor = toNestCoor(polygon.get(i).getX() , polygon.get(i).getY());
            normal.add(new Segment(nestCoor.getX() , nestCoor.getY()));
        }
        return normal;
    }

    public static void offsetTree(List<NestPath> t , double offset ){
        for(int i =0 ; i <t.size() ; i ++){
            List<NestPath> offsetPaths = polygonOffset(t.get(i) , offset);
            if(offsetPaths.size() == 1 ){
                t.get(i).clear();
                NestPath from = offsetPaths.get(0);

                for(Segment s : from.getSegments()){
                    t.get(i).add(s);
                }
            }
            if(t.get(i).getChildren().size() > 0 ){

                offsetTree(t.get(i).getChildren() , -offset);
            }
        }
    }

    public static List<NestPath> polygonOffset(NestPath polygon , double offset){
        List<NestPath> result = new List<NestPath>();
        if(offset == 0 || GeometryUtil.almostEqual(offset,0)){
            /**
             * return EmptyResult
             */
            return result;
        }
        Path p = new Path();
        for(Segment s : polygon.getSegments()){
            ClipperCoor cc = CommonUtil.toClipperCoor(s.getX(),s.getY());
            p.add(new Point.LongPoint( cc.getX() ,cc.getY()));
        }

        int miterLimit = 2;
        ClipperOffset co = new ClipperOffset(miterLimit , polygon.config.CURVE_TOLERANCE * Config.CLIIPER_SCALE);
        co.addPath(p, Clipper.JoinType.ROUND , Clipper.EndType.CLOSED_POLYGON);

        Paths newpaths = new Paths();
        co.execute(newpaths , offset * Config.CLIIPER_SCALE);


        /**
         * 这里的length是1的话就是我们想要的
         */
        for(int i = 0 ; i <newpaths.size() ; i ++){
            result.add(CommonUtil.clipperToNestPath(newpaths.get(i)));
        }

        if(offset > 0 ){
            NestPath from = result.get(0);
            if(GeometryUtil.polygonArea(from) > 0 ){
                from.reverse();
            }
            from.add(from.get(0));from.getSegments().remove(0);
        }


        return result;
    }


    /**
     * 对应于JS项目中的getParts
     */
    public static List<NestPath> BuildTree(List<NestPath> parts ,double curve_tolerance){
        List<NestPath> polygons = new List<NestPath>();
        for(int i =0 ; i<parts.size();i++){
            NestPath cleanPoly = NestPath.cleanNestPath(parts.get(i));
            cleanPoly.bid = parts.get(i).bid;
            if(cleanPoly.size() > 2 &&  Math.abs(GeometryUtil.polygonArea(cleanPoly)) > curve_tolerance * curve_tolerance){
                cleanPoly.setSource(i);

                polygons.add(cleanPoly);
            }
        }

        CommonUtil.toTree(polygons,0);
        return polygons;
    }
}
