shader_type canvas_item;
render_mode blend_mix;

uniform bool debug_visible = false;
uniform vec4 fog_color : source_color = vec4(0.086275, 0.133333, 0.172549, 1.0);
uniform float alpha : hint_range(0.0, 1.0, 0.001) = 0.18;
uniform float noise_scale : hint_range(0.1, 16.0, 0.1) = 2.4;
uniform float noise_strength : hint_range(0.0, 1.0, 0.01) = 0.40;
uniform float drift_speed_x : hint_range(0.0, 0.1, 0.001) = 0.018;
uniform float drift_speed_y : hint_range(0.0, 0.05, 0.001) = 0.003;
uniform float horizon_start : hint_range(0.0, 1.0, 0.01) = 0.22;
uniform float horizon_end : hint_range(0.0, 1.0, 0.01) = 0.82;
uniform float bottom_fade : hint_range(0.0, 1.0, 0.01) = 0.88;
uniform float top_fade : hint_range(0.0, 1.0, 0.01) = 0.08;

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

float fbm(vec2 p) {
	float value = 0.0;
	float amplitude = 0.5;
	for (int i = 0; i < 4; i++) {
		value += amplitude * noise(p);
		p *= 2.02;
		amplitude *= 0.5;
	}
	return value;
}

void fragment() {
	vec4 output_color;

	if (debug_visible) {
		output_color = vec4(0.25, 0.45, 1.0, 0.45);
	} else {
		vec2 uv = UV;
		vec2 drift = vec2(TIME * drift_speed_x, TIME * drift_speed_y);
		float n = fbm(uv * noise_scale + drift);
		n = mix(1.0, n, noise_strength);

		float horizon = smoothstep(horizon_start, (horizon_start + horizon_end) * 0.5, uv.y);
		horizon *= 1.0 - smoothstep((horizon_start + horizon_end) * 0.5, horizon_end, uv.y);
		horizon = max(horizon, 0.15);

		float top_mask = smoothstep(0.0, top_fade, uv.y);
		float bottom_mask = 1.0 - smoothstep(bottom_fade, 1.0, uv.y);
		float mask = horizon * top_mask * bottom_mask;

		float final_alpha = alpha * n * mask;
		output_color = vec4(fog_color.rgb, final_alpha);
	}

	COLOR = output_color;
}
