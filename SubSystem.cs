using System.Collections.Generic;

namespace ecs{
public class SubSystem :ISystem{
	protected readonly Context _context;

	public SubSystem(Context context){
		_context=context;
	}
	protected virtual bool Filter(Entity entity){
		return true;
	}
	public virtual void Init(){}
	public virtual void PreUpdate(float dt){}
	public virtual void Execute(){}
	public virtual void Update(float dt){}
	public virtual void LateUpdate(float dt){}
	public virtual void Tick(float dt){}
}
}