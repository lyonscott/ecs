using System.Collections.Generic;
using System;
using UnityEngine;

namespace ecs{
public class Context{
    public delegate void UpdateCallback<TCallback>(Context context,TCallback obj);
    public event UpdateCallback<Entity> OnEntityCreated;
    public event UpdateCallback<Entity> OnEntityRemoved;
    public event UpdateCallback<Group> OnGroupCreated;
    public event UpdateCallback<Group> OnGroupRemoved;

    readonly HashSet<Entity> _entities=new HashSet<Entity>(EntityEqualityComparer<Entity>.comparer);
    readonly Dictionary<Matcher,Group> _groups_by_matcher=new Dictionary<Matcher, Group>();
    readonly Dictionary<int,Entity> _sole_entites=new Dictionary<int,Entity>();
    readonly List<Group> _groups=new List<Group>();
    Entity[] _cache_entites;
    int _index_entities=0;

    public Context(){
    }

    public Entity
    CreateEntity(){
        _index_entities++;
        Entity entity=new Entity(_index_entities);
        _entities.Add(entity);
        _cache_entites=null;
        for(int i=0;i<_groups.Count;++i){
            _groups[i].Attent(entity);
        }
        return entity;
    }

    public void
    SetSoleEntity(int index,Entity entity){
        if(!_sole_entites.ContainsKey(index)){
            _sole_entites.Add(index,entity);
        }
    }
    
    public Entity
    GetSoleEntity(int index){
        if(_sole_entites.ContainsKey(index)){
            return _sole_entites[index];
        }
        return null;
    }

    public bool 
    RemoveEntity(Entity entity){
        var removed=_entities.Remove(entity);
        if(removed){
            _cache_entites=null;
            return true;
        }
        return false;
    }

    public bool
    HasEntity(Entity entity){
        return _entities.Contains(entity);
    }

    public Entity[] 
    GetEntites(){
        if(_cache_entites==null){
            _cache_entites=new Entity[_entities.Count];
            _entities.CopyTo(_cache_entites);
        }
        return _cache_entites;
    }
    public Entity[]
    GetEntites(Matcher matcher){
        var group=GetGroup(matcher);
        return group.GetEntites();
    }

    public Group
    GetGroup(Matcher matcher){
        Group group;
        if(!_groups_by_matcher.TryGetValue(matcher,out group)){
            group=new Group(matcher);
            var entities=GetEntites();
            for(int i=0;i<entities.Length;++i){
                var entity=entities[i];
                group.Attent(entity);
                group.Check(entity);
            }
            _groups_by_matcher.Add(matcher,group);
            _groups.Add(group);
        }
        return group;
    }
}
}