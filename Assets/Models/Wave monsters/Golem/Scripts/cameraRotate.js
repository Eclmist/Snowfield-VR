#pragma strict

private var rotationVelocity : Vector2;



function Update () {

	if(Input.GetMouseButton(0) || Input.GetMouseButton(1)){
		rotationVelocity.x += Mathf.Pow(Mathf.Abs(Input.GetAxis("Mouse X")),1.5) * Mathf.Sign(Input.GetAxis("Mouse X"));
		rotationVelocity.y -= Input.GetAxis("Mouse Y") * 0.04;
	}
	transform.position.y += rotationVelocity.y;
	transform.RotateAround(Vector3.zero, Vector3.up, rotationVelocity.x );
	rotationVelocity = Vector2.Lerp(rotationVelocity, Vector2.zero, Time.deltaTime * 10.0);
	transform.position.y = Mathf.Clamp(transform.position.y, -5 , 10);
	transform.LookAt(Vector3(0,1,0));
}