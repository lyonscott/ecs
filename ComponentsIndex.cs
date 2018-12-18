namespace ecs{
public static partial class ComponentsIndex{
    private static int _id_index=0;
    public static int 
    Total(){
        return _id_index;
    }
    public static int
    ApplyIndex(){
        _id_index++;
        return _id_index;
    }
}
}