shader_type canvas_item;

uniform vec4 core_color : source_color = vec4(1.0, 0.7, 0.2, 1.0);
uniform vec4 glow_color : source_color = vec4(1.0, 0.35, 0.05, 1.0);
uniform float pulse_speed = 3.5;
uniform float intensity = 1.4;

void fragment() {
    vec2 center = vec2(0.5, 0.5);
    float dist = distance(UV, center);
    
    float pulse = sin(TIME * pulse_speed) * 0.15 + 0.85;
    float glow = 1.0 - smoothstep(0.0, 0.5 * pulse, dist);
    
    vec4 final_color = mix(glow_color, core_color, smoothstep(0.3, 0.0, dist));
    final_color.a = glow * intensity;
    
    COLOR = final_color;
}
