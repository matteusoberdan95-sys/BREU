shader_type spatial;
render_mode unshaded, cull_disabled, blend_mix, depth_draw_never, depth_test_disabled;

uniform vec4 fog_color : source_color = vec4(0.086275, 0.133333, 0.172549, 1.0);
uniform float alpha : hint_range(0.0, 1.0, 0.001) = 0.08;
uniform float softness : hint_range(0.1, 4.0, 0.01) = 1.4;
uniform float noise_strength : hint_range(0.0, 1.0, 0.01) = 0.25;
uniform float noise_scale : hint_range(0.1, 16.0, 0.1) = 3.0;

float hash(vec2 p) {
	return fract(sin(dot(p, vec2(127.1, 311.7))) * 43758.5453123);
}

float noise(vec2 p) {
	vec2 i = floor(p);
	vec2 f = fract(p);
	f = f * f * (3.0 - 2.0 * f);
	float a = hash(i);
	float b = hash(i + vec2(1.0, 0.0));
	float c = hash(i + vec2(0.0, 1.0));
	float d = hash(i + vec2(1.0, 1.0));
	return mix(mix(a, b, f.x), mix(c, d, f.x), f.y);
}

void fragment() {
	vec2 uv = UV * 2.0 - 1.0;
	float dist = length(uv);
	float radial = 1.0 - smoothstep(0.0, softness, dist);
	radial = pow(max(radial, 0.0), 2.0);

	float n = noise(UV * noise_scale);
	n = mix(1.0, n * 0.55 + 0.45, noise_strength);

	float final_alpha = radial * n * alpha;
	ALBEDO = fog_color.rgb;
	ALPHA = final_alpha;
}
