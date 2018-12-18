using System.Collections.Generic;
using System;

namespace ecs{
public class Group{
    public event Entity.UpdateCallback OnEntityAdded;
    public event Entity.UpdateCallback OnEntityRemoved;

    readonly Matcher _matcher;
    readonly HashSet<Entity> _entities=new HashSet<Entity>(EntityEqualityComparer<Entity>.comparer);

    Entity[] _cache_entities;
    Entity.UpdateCallback _update;

    public Matcher matcher{get{return _matcher;}}
    public int count{
        get{return _entities.Count;}
    }

    public Group(Matcher matcher){
        _matcher=matcher;
        _update=Check;
    }

    public void
    Check(Entity entity){
        if(_matcher.Matches(entity)){
            Add(entity);
        }else{
            Remove(entity);
        }
    }

    public void
    Attent(Entity entity){
        entity.OnComponentAdded+=_update;
        entity.OnComponentRemoved+=_update;
    }

    public void 
    Add(Entity entity){
        var added=_entities.Add(entity);
        if(added){
            _cache_entities=null;
            if(OnEntityAdded!=null){
                OnEntityAdded(entity);
            }
        }
    }
    public void
    Remove(Entity entity){
        var removed=_entities.Remove(entity);
        if(removed){
            _cache_entities=null;
            if(OnEntityRemoved!=null){
                OnEntityRemoved(entity);
            }
        }
    }

    public Entity[]
    GetEntites(){
        if(_cache_entities==null){
            _cache_entities=new Entity[_entities.Count];
            _entities.CopyTo(_cache_entities);
        }
        return _cache_entities;
    }
}
}