shader_type spatial;
render_mode diffuse_burley, specular_schlick_ggx;

uniform sampler2D top_albedo : source_color, filter_linear_mipmap_anisotropic, repeat_enable;
uniform sampler2D bottom_albedo : source_color, filter_linear_mipmap_anisotropic, repeat_enable;
uniform sampler2D bottom_roughness : hint_roughness_r, filter_linear_mipmap_anisotropic, repeat_enable;
uniform sampler2D bottom_normal : hint_normal, filter_linear_mipmap_anisotropic, repeat_enable;
uniform vec3 top_tint : source_color = vec3(0.63, 0.56, 0.46);
uniform vec3 bottom_tint : source_color = vec3(0.72, 0.69, 0.61);
uniform float ceramic_scale = 0.72;
uniform float ceiling_scale = 0.42;

varying vec3 world_position;
varying vec3 world_normal;

void vertex() {
	world_position = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
	world_normal = normalize(MODEL_NORMAL_MATRIX * NORMAL);
}

void fragment() {
	bool top_face = world_normal.y > 0.35;
	bool bottom_face = world_normal.y < -0.35;
	if (top_face) {
		vec2 uv = world_position.xz * ceramic_scale;
		ALBEDO = texture(top_albedo, uv).rgb * top_tint;
		ROUGHNESS = 0.96;
	} else if (bottom_face) {
		vec2 uv = world_position.xz * ceiling_scale;
		ALBEDO = texture(bottom_albedo, uv).rgb * bottom_tint;
		ROUGHNESS = max(0.94, texture(bottom_roughness, uv).r);
		NORMAL_MAP = texture(bottom_normal, uv).rgb;
		NORMAL_MAP_DEPTH = 1.05;
	} else {
		// Box side faces were visible as bright, flickering frames around the
		// stair shaft. The physical slab remains unchanged; only its thin side
		// skin is discarded while top floor and lower ceiling stay rendered.
		discard;
	}
}
