namespace ecs{
public static class ECSUtils{
    public static int 
    GetHashCode(int[] arry,int factor=31){
        int hash=0;
        if(arry!=null){
            for(int i=0;i<arry.Length;++i){
                hash^=arry[i]*factor;
            }
            hash^=arry.Length*factor;
        }
        return hash;
    }
    public static bool 
    Match(int[] current,int[] other){
        if((current==null)!=(other==null))return false;
        if(current==null)return true;
        if(current.Length!=other.Length)return false;
        for(int i=0;i<current.Length;++i){
            if(current[i]!=other[i])return false;
        }
        return true;
    }
}
}