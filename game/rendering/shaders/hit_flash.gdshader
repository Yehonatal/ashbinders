shader_type canvas_item;

uniform bool is_flashing = false;
uniform vec4 flash_color : source_color = vec4(1.0, 1.0, 1.0, 1.0);

void fragment() {
    vec4 base = texture(TEXTURE, UV);
    if (is_flashing) {
        COLOR = vec4(flash_color.rgb, base.a);
    } else {
        COLOR = base;
    }
}
