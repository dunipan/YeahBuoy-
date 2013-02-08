#pragma strict

var thePrefab : GameObject;
public var hitPointX;
public var hitPointY;
public var hitPointZ;


function Instantiate() 
{
	if(Input.GetMouseButtonDown(0)) { //If LMB is clicked

			Instantiate(thePrefab,Vector3(hitPointX,hitPointY,hitPointZ),Quaternion.identity);
			Debug.Log("IamsoSMRT" + hitPointX);
			}
	}
