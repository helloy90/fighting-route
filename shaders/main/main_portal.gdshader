shader_type spatial;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	if(UV.y > 0.5 && UV.x < 0.5){
		ALBEDO = vec3(0.0, 0.0, 0.0);
		ALPHA = 0.1;
	}
	if(UV.y < 0.5){
		ALBEDO = vec3(0.0, 0.0, 0.0);
		ALPHA = UV.y*1.0/0.5;
	}
	if(UV.y > 0.5 && UV.x > 0.5){
		ALBEDO = vec3(0.0, 0.0, 0.0);
	}
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
