using UnityEngine;
using System.Collections;

public interface IPendant  {
	Vector3 getCenterOfMass();
	Vector3 getSuspensionPoint();
	Vector3 getMinBound();
	Vector3 getMaxBound();
	float getVoxelMass();

}