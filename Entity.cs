using System.Collections.Generic;
using System.Linq;
using System;

namespace ecs{
public partial class Entity: IEntity{
    public delegate void UpdateCallback(Entity entity);

    public event UpdateCallback OnComponentAdded;
    public event UpdateCallback OnComponentRemoved;

    private int _id;
    public int id{ 
        get{return _id;}
    }
    private Dictionary<int,IComponent> _components;


    public Entity(int id=0){
        _id=id;
        _components=new Dictionary<int, IComponent>();
    }

    public bool HasComponent(params int[] coms){
        for(int i=0; i<coms.Length; ++i){
            if(!_components.ContainsKey(coms[i]))return false;
        }
        return true;
    }

    public bool HasAnyComponent(params int[] coms){
        for(int i=0; i<coms.Length; ++i){
            if(_components.ContainsKey(coms[i]))return true;
        }
        return false;
    }

    public IComponent GetComponent(int id){
        if(_components.ContainsKey(id)){
            return _components[id];
        }
        return null;
    }

    public IComponent[] GetComponents(){
        return _components.Values.ToArray();
    }

    public void AddComponent(int index,IComponent component,bool update=true){
        if(!_components.ContainsKey(index)){
            _components.Add(index,component);
            if(OnComponentAdded!=null && update){
                OnComponentAdded(this);
            }
        }
    }

    public T AddComponent<T>(int index,bool update=true) where T:IComponent,new(){
        if(!_components.ContainsKey(index)){
            var component=new T();
            _components.Add(index,component);
            if(OnComponentAdded!=null && update){
                OnComponentAdded(this);
            }
        }
        return (T)_components[index];
    }

    public void RemoveComponent(int index,bool update=true){
        if(HasComponent(index)){
            var compoent=_components[index];
            _components.Remove(index);
            if(OnComponentRemoved!=null){
                OnComponentRemoved(this);
            }
        }
    }

    public void Destroy(){
        _components.Clear();
    }
}

public class EntityEqualityComparer<T>: IEqualityComparer<T> where T:class,IEntity{
    public static readonly IEqualityComparer<T> comparer=new EntityEqualityComparer<T>();
    
    public bool Equals(T one,T other){
        return one==other;
    }

    public int GetHashCode(T obj){
        return obj.id;
    }
}
}