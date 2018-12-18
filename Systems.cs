using System.Collections.Generic;

namespace ecs{
public class Systems: ISystem{
    protected readonly List<ISystem> _sub_systems;
    public Systems(){
        _sub_systems=new List<ISystem>();
    }

    public virtual Systems
    AddSystem(ISystem system){
        _sub_systems.Add(system);
        return this;
    }

    public virtual void
    Init(){
        for(int i=0;i<_sub_systems.Count;++i){
            _sub_systems[i].Init();
        }
    }
    public virtual void
    PreUpdate(float dt){
        for(int i=0;i<_sub_systems.Count;++i){
            _sub_systems[i].PreUpdate(dt);
        }
    }
    public virtual void
    Update(float dt){
        for(int i=0;i<_sub_systems.Count;++i){
            _sub_systems[i].Update(dt);
        }
    }

    public virtual void
    Execute(){
        for(int i=0;i<_sub_systems.Count;++i){
            _sub_systems[i].Execute();
        }
    }

    public virtual void
    LateUpdate(float dt){
        for(int i=0;i<_sub_systems.Count;++i){
            _sub_systems[i].LateUpdate(dt);
        }
    }
    public virtual void
    Tick(float dt){
        for(int i=0;i<_sub_systems.Count;++i){
            _sub_systems[i].Tick(dt);
        }
    }
}
}