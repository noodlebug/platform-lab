using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : PlayerEntity
{
	[Header("Classic Physics")]  
    //  for normal movement, set all to 1  
    public ClassicPhysics.Modifiers physicsModifiers;

    private ClassicPhysics classicPhysics;
	protected override void Awake()
    {
        base.Awake();
        classicPhysics = new ClassicPhysics(this.gameObject, physicsModifiers);
    }

	protected override void Update()
    {
        //classicPhysics.Input(;

        base.Update();
    }

    private void FixedUpdate()
    {
        classicPhysics._FixedUpdate();
    }
}
