using System.Collections.Generic;
using System;

namespace ecs{
public class Matcher{
    private static HashSet<int> _tmp_set=new HashSet<int>();

    private int[] _indices_all;
    private int[] _indices_any;
    private int[] _indices_none;

    public bool Matches(Entity entity){
        return (_indices_all==null || entity.HasComponent(_indices_all))&&
        (_indices_any==null || entity.HasAnyComponent(_indices_any))&&
        (_indices_none==null || !entity.HasAnyComponent(_indices_none));
    }

    public void
    SetAllOf(params int[] indices){
        _indices_all=Distinct(indices);
        _is_hash_cached=false;
    }
    public void
    SetAnyOf(params int[] indices){
        _indices_any=Distinct(indices);
        _is_hash_cached=false;
    }
    public void
    SetNoneOf(params int[] indices){
        _indices_none=Distinct(indices);
        _is_hash_cached=false;
    }

    int _hash;
    bool _is_hash_cached;
    public override int
    GetHashCode(){
        if(!_is_hash_cached){
            _hash=GetType().GetHashCode();
            _hash^=ECSUtils.GetHashCode(_indices_all,2);
            _hash^=ECSUtils.GetHashCode(_indices_any,4);
            _hash^=ECSUtils.GetHashCode(_indices_none,8);
            _is_hash_cached=true;
        }
        return _hash;
    }
    public override bool
    Equals(object obj){
        if(obj==null||
        obj.GetType()!=GetType()||
        obj.GetHashCode()!=GetHashCode()){
            return false;
        }
        var matcher=(Matcher)obj;
        if(!ECSUtils.Match(matcher._indices_all,_indices_all))return false;
        if(!ECSUtils.Match(matcher._indices_any,_indices_any))return false;
        if(!ECSUtils.Match(matcher._indices_none,_indices_none))return false;
        return true;
    }

    public static int[]
    Distinct(int[] indices){
        for(int i=0;i<indices.Length;++i){
            _tmp_set.Add(indices[i]);
        }
        var arry=new int[_tmp_set.Count];
        _tmp_set.CopyTo(arry);
        _tmp_set.Clear();
        Array.Sort(arry);
        return arry;
    }

    public static Matcher 
    AllOf(params int[] indices){
        var matcher=new Matcher();
        matcher.SetAllOf(indices);
        return matcher;
    }
    public static Matcher
    AnyOf(params int[] indices){
        var matcher=new Matcher();
        matcher.SetAnyOf(indices);
        return matcher;
    }
    public static Matcher
    NoneOf(params int[] indices){
        var matcher=new Matcher();
        matcher.SetNoneOf(indices);
        return matcher;
    }
}
}