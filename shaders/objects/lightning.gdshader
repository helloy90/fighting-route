shader_type spatial;

uniform sampler2D noise;
uniform float scale;
uniform float position;
uniform float time;

void vertex() {
	float offer = (texture(noise, vec2(time, VERTEX.z/2.0 + 0.5)).y - 0.5)/scale*(-VERTEX.z*VERTEX.z + 1.0);
	VERTEX.z  = (VERTEX.z + 1.0)*position - 1.0;
	VERTEX.y += offer;
	VERTEX.x += offer;
}

void fragment() {
	//ALBEDO = vec3(0.25, 0.41, 1.0);
	EMISSION = vec3(0.25, 0.41, 1.0);
	EMISSION.b  = 2.0;
}

//void light() {
	 ////Called for every pixel for every light affecting the material.
	 ////Uncomment to replace the default light processing function with this one.
//}
