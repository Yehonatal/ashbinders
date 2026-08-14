shader_type canvas_item;

uniform vec4 shadow_color : source_color = vec4(0.01, 0.02, 0.04, 0.65);
uniform float softness = 0.45;

void fragment() {
    vec2 center = vec2(0.5, 0.5);
    // 2:1 Elliptical coordinate normalization
    vec2 p = (UV - center);
    p.y *= 2.0; // Squish vertically for 2:1 isometric ground plane
    
    float dist = length(p);
    float alpha = smoothstep(0.5, 0.5 - softness, dist);
    
    COLOR = vec4(shadow_color.rgb, shadow_color.a * alpha);
}
