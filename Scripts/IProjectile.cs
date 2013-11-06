using UnityEngine;
using System.Collections;

public interface IProjectile {
	
	int Damage {get; set;}
	NetworkPlayer OwnerID {get;set;}
	
}
