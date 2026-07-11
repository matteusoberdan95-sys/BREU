shader_type spatial;
render_mode unshaded, cull_disabled, depth_draw_never, depth_test_disabled, fog_disabled;

uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_linear;
uniform sampler2D depth_texture : hint_depth_texture, repeat_disable, filter_nearest;

uniform bool debug_depth = false;
uniform bool debug_sky = false;
uniform vec4 fog_color : source_color = vec4(0.219608, 0.301961, 0.356863, 1.0);
uniform float fog_start : hint_range(0.0, 20.0, 0.1) = 2.5;
uniform float fog_density : hint_range(0.0, 0.5, 0.001) = 0.095;
uniform float fog_strength : hint_range(0.0, 1.0, 0.01) = 0.88;
uniform float fog_power : hint_range(0.1, 4.0, 0.01) = 0.72;
uniform float horizon_y : hint_range(0.0, 1.0, 0.01) = 0.50;
uniform float horizon_width : hint_range(0.05, 1.0, 0.01) = 0.42;
uniform float noise_scale : hint_range(0.1, 16.0, 0.1) = 4.0;
uniform float noise_strength : hint_range(0.0, 1.0, 0.01) = 0.08;
uniform float drift_speed : hint_range(0.0, 0.1, 0.001) = 0.018;
uniform vec3 sky_dark_color : source_color = vec3(0.003922, 0.011765, 0.027451);
uniform float sky_override_strength : hint_range(0.0, 1.0, 0.01) = 1.0;

float hash(vec2 p) {
	return fract(sin(dot(p, vec2(127.1, 311.7))) * 43758.5453123);
}

float noise(vec2 p) {
	vec2 i = floor(p);
	vec2 f = fract(p);

	float a = hash(i);
	float b = hash(i + vec2(1.0, 0.0));
	float c = hash(i + vec2(0.0, 1.0));
	float d = hash(i + vec2(1.0, 1.0));

	vec2 u = f * f * (3.0 - 2.0 * f);

	return mix(a, b, u.x) +
		(c - a) * u.y * (1.0 - u.x) +
		(d - b) * u.x * u.y;
}

float fbm(vec2 p) {
	float v = 0.0;
	float amp = 0.5;

	for (int i = 0; i < 4; i++) {
		v += noise(p) * amp;
		p *= 2.0;
		amp *= 0.5;
	}

	return v;
}

void vertex() {
	POSITION = vec4(VERTEX.xy, 1.0, 1.0);
}

void fragment() {
	vec2 uv = SCREEN_UV;
	vec3 scene_color = texture(screen_texture, uv).rgb;
	float raw_depth = texture(depth_texture, uv).r;
	bool is_sky = raw_depth <= 0.0005 || raw_depth >= 0.9995;
	vec3 output_color = scene_color;

	if (debug_sky && is_sky) {
		output_color = vec3(1.0, 0.0, 1.0);
	} else if (is_sky) {
		output_color = mix(scene_color, sky_dark_color, sky_override_strength);
	} else {
		vec3 ndc = vec3(uv * 2.0 - 1.0, raw_depth);
		vec4 view = INV_PROJECTION_MATRIX * vec4(ndc, 1.0);
		view.xyz /= view.w;
		float dist = max(-view.z, 0.0);

		if (debug_depth) {
			float d = clamp(dist / 60.0, 0.0, 1.0);
			output_color = vec3(d);
		} else {
			float fog_amount = 1.0 - exp(-max(dist - fog_start, 0.0) * fog_density);
			fog_amount = pow(clamp(fog_amount, 0.0, 1.0), fog_power);
			fog_amount *= fog_strength;

			float horizon_mask = exp(-pow((uv.y - horizon_y) / horizon_width, 2.0));
			float top_fade = smoothstep(0.02, 0.18, uv.y);
			float bottom_fade = 1.0 - smoothstep(0.82, 1.0, uv.y);
			float mask = mix(0.55, 1.0, horizon_mask) * top_fade * bottom_fade;

			vec2 n_uv = uv * noise_scale + vec2(TIME * drift_speed, TIME * drift_speed * 0.25);
			float n = fbm(n_uv);
			float n_mod = mix(1.0 - noise_strength, 1.0 + noise_strength, n);

			fog_amount = clamp(fog_amount * mask * n_mod, 0.0, 0.86);
			output_color = mix(scene_color, fog_color.rgb, fog_amount);
		}
	}

	ALBEDO = output_color;
}
