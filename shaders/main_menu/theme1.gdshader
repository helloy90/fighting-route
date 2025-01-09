shader_type canvas_item;

uniform vec3 color: source_color = vec3(0.0, 0.3, 0.9);
uniform float speed: hint_range(0.0, 1.0, 0.1) = 0.5;

struct MetaBall {
	float radius;
	float freq;
	float amp;
};

const MetaBall[] balls = {MetaBall(0.2, 2.0, 0.9)};

float ball(vec2 uv, float radius){
	return 0.0;
}

void fragment() {
	// Called for every pixel the material is visible on.
}