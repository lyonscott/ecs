using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
namespace ecs{
public class Collector{
	public delegate bool FilterHandle(Entity entity);

	readonly HashSet<Entity> _entities;
	readonly Group[] _groups;

	List<Entity> _buffer;
	Entity[] _cached;
	Entity.UpdateCallback _update_added;
	Entity.UpdateCallback _update_removed;
	FilterHandle _filter;

	public Collector(Group group): this(new[]{group}){}
	public Collector(Group[] groups){
		_groups=groups;
		_entities=new HashSet<Entity>(EntityEqualityComparer<Entity>.comparer);
		_buffer=new List<Entity>();
		_update_added=addEntity;
		_update_removed=rmEntity;
		Activate();
		Collect();
	}

	public void 
	SetFilter(FilterHandle handle){
		_filter=handle;
	}

	public bool
	Filter(Entity entity){
		if(_filter!=null){
			return _filter(entity);
		}
		return true;
	}

	public Entity[]
	Collect(){
		if(_cached==null){
			_entities.Clear();
			for(int i_g=0;i_g<_groups.Length;++i_g){
				var group=_groups[i_g];
				var entites=group.GetEntites();
				for(int i_e=0;i_e<entites.Length;++i_e){
					_entities.Add(entites[i_e]);
				}
			}
			_cached=new Entity[_entities.Count];
			_entities.CopyTo(_cached);
		}
		return _cached;
	}

	public Entity[]
	GetEntities(){
		foreach(var e in _entities){
			if(Filter(e)){
				_buffer.Add(e);
			}
		}
		var arry=_buffer.ToArray();
		_buffer.Clear();
		_entities.Clear();
		return arry;
	}

	public void 
	Activate(){
		for(int i=0;i<_groups.Length;++i){
			var group=_groups[i];
			group.OnEntityAdded-=_update_added;
			group.OnEntityAdded+=_update_added;
			group.OnEntityRemoved-=_update_removed;
			group.OnEntityRemoved+=_update_removed;
		}
	}

	public void
	Deactivate(){
		for(int i=0;i<_groups.Length;++i){
			var group=_groups[i];
			group.OnEntityAdded-=_update_added;
			group.OnEntityRemoved-=_update_removed;
		}
		_entities.Clear();
	}

	public void
	Clear(){
		_entities.Clear();
		_buffer.Clear();
	}

	void
	rmEntity(Entity e){
		_cached=null;
	}

	void
	addEntity(Entity entity){
		_cached=null;
		_entities.Add(entity);
	}
}
}